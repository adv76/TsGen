using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using TsGen;
using TsGen.Extensions;
using TsGenCli;

var processInfo = new ProcessStartInfo()
{
    FileName = "dotnet",
    ArgumentList = { "build", "-getTargetResult:Build" },
    RedirectStandardOutput = true,
    UseShellExecute = false
};

var process = Process.Start(processInfo);
if (process is not null)
{
    Console.WriteLine("Building...");

    await process.WaitForExitAsync();

    var msBuildResultText = process.StandardOutput.ReadToEnd();
    var buildResult = JsonSerializer.Deserialize<MsBuildOutputRecord>(msBuildResultText);

    if (buildResult is not null)
    {
        if (buildResult.TargetResults.Build.Result == "Success")
        {
            Console.WriteLine("Build Succeeded!");

            var assemblyPath = buildResult.TargetResults.Build.Items[0].FullPath;

            var assembly = Assembly.LoadFile(assemblyPath);

            var genSettingsTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(GeneratorSettingsBase)));

            GeneratorSettingsBase generatorSettings;

            int count = genSettingsTypes.Count();
            if (count == 0)
            {
                Console.WriteLine("No generator settings gound.");
                Console.WriteLine("Using DefaultGeneratorSettings instead.");

                generatorSettings = new DefaultGeneratorSettings();
            }
            else if (count == 1)
            {
                var settingsType = genSettingsTypes.First();

                Console.WriteLine($"Using {settingsType.Name} to generate types.");

                generatorSettings = (GeneratorSettingsBase)Activator.CreateInstance(settingsType)!;
            }
            else
            {
                var settingsType = genSettingsTypes.First();

                Console.WriteLine("Multiple generator settings were found.");
                Console.WriteLine($"Using {settingsType.Name} to generate types.");

                generatorSettings = (GeneratorSettingsBase)Activator.CreateInstance(settingsType)!;
            }

            var tsFiles = assembly.GenerateTypeFiles(generatorSettings);

            await assembly.GenerateAndOutputTypeFilesAsync(generatorSettings);

            //foreach (var file in tsFiles)
            //{
            //    var path = file.ToFile(generatorSettings.OutputDirectory, out var fileContents);

            //    Console.WriteLine(path);
            //    Console.WriteLine();
            //    Console.WriteLine(fileContents);
            //    Console.WriteLine("---------------------------------");
            //    Console.WriteLine();
            //}
        }
        else
        {
            Console.WriteLine($"Build Result: {buildResult.TargetResults.Build.Result}");
        }
    }
}
