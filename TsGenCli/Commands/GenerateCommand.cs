using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using TsGen;
using TsGen.Extensions;

namespace TsGenCli.Commands
{
    public sealed class GenerateCommandSettings : CommandSettings
    {
        [CommandOption("-o|--out-dir")]
        [Description("Specifies the base output directory to save to")]
        [DefaultValue(null)]
        public string? OutputDirectory { get; set; }

        [CommandOption("-p|--project-file")]
        [Description("Specifies the project to build (default is the project in this directory)")]
        [DefaultValue(null)]
        public string? ProjectFile { get; set; }

        public override ValidationResult Validate()
        {
            if (OutputDirectory is not null)
            {
                var fullPath = Path.GetFullPath(OutputDirectory);
                if (!Directory.Exists(fullPath))
                {
                    return ValidationResult.Error("The output directory specified does not exist.");
                }
            }

            if (ProjectFile is not null)
            {
                var fullPath = Path.GetFullPath(ProjectFile);
                if (!File.Exists(fullPath))
                {
                    return ValidationResult.Error("The project file specified does not exist.");
                }
            }

            return ValidationResult.Success();
        }
    }

    public class GenerateCommand : AsyncCommand<GenerateCommandSettings>
    {
        public override async Task<int> ExecuteAsync(CommandContext context, GenerateCommandSettings settings)
        {
            List<string> args;

            if (settings.ProjectFile is not null)
            {
                args = ["build", settings.ProjectFile, "-getTargetResult:Build", "-p:EnableDynamicLoading=true"];
            }
            else
            {
                args = ["build", "-getTargetResult:Build", "-p:EnableDynamicLoading=true"];
            }

            var processInfo = new ProcessStartInfo("dotnet", args)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            var process = Process.Start(processInfo);
            if (process is not null)
            {
                AnsiConsole.MarkupLine("[yellow]Building...[/]");

                await process.WaitForExitAsync();
                if (process.ExitCode != 0)
                {
                    AnsiConsole.MarkupLine("[red]Build failed.[/]");
                    AnsiConsole.WriteLine();
                    AnsiConsole.MarkupLine("[red]Error:[/]");

                    var errorText = await process.StandardOutput.ReadToEndAsync();

                    AnsiConsole.MarkupLineInterpolated($"[red]{errorText}[/]");

                    return process.ExitCode;
                }

                var msBuildResultText = await process.StandardOutput.ReadToEndAsync();
                var buildResult = JsonSerializer.Deserialize<MsBuildOutputRecord>(msBuildResultText);

                if (buildResult is not null)
                {
                    if (buildResult.TargetResults.Build.Result == "Success")
                    {
                        AnsiConsole.MarkupLine("[Green]Build Succeeded![/]");

                        var assemblyPath = buildResult.TargetResults.Build.Items[0].FullPath;
                        var assembly = Assembly.LoadFrom(assemblyPath);

                        var genSettingsTypes = assembly.GetTypes()
                            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(TsGenSettings)));

                        TsGenSettings generatorSettings;

                        int count = genSettingsTypes.Count();
                        if (count == 0)
                        {
                            AnsiConsole.MarkupLine("[yellow]No generator settings found.[/]");
                            AnsiConsole.MarkupLine("[yellow]Using DefaultGeneratorSettings instead.[/]");

                            generatorSettings = new TsGenSettings();
                        }
                        else if (count == 1)
                        {
                            var settingsType = genSettingsTypes.First();

                            AnsiConsole.MarkupLineInterpolated($"[Yellow]Using {settingsType.Name} to generate types.[/]");

                            generatorSettings = (TsGenSettings)Activator.CreateInstance(settingsType)!;
                        }
                        else
                        {
                            var settingsType = genSettingsTypes.First();

                            AnsiConsole.MarkupLine("[Yellow]Multiple generator settings were found.[/]");
                            AnsiConsole.MarkupLineInterpolated($"[Yellow]Using {settingsType.Name} to generate types.[/]");

                            generatorSettings = (TsGenSettings)Activator.CreateInstance(settingsType)!;
                        }

                        if (settings.OutputDirectory is not null)
                        {
                            generatorSettings.OutputDirectory = settings.OutputDirectory;
                        }

                        var tsFiles = assembly.GenerateTypeFiles(generatorSettings);

                        await assembly.GenerateAndOutputTypeFilesAsync(generatorSettings);

                        AnsiConsole.MarkupLine("[Green]Type Generation Complete![/]");

                        return 0;
                    }
                    else
                    {
                        AnsiConsole.MarkupLineInterpolated($"[Red]Build Result: {buildResult.TargetResults.Build.Result}[/]");
                    }
                }
            }

            return -1;
        }
    }
}
