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
            var tsFiles = assembly.GenerateTypeFiles();

            foreach (var file in tsFiles)
            {
                var path = file.ToFile(GeneratorSettings.BaseDirectory, out var fileContents);

                Console.WriteLine(path);
                Console.WriteLine();
                Console.WriteLine(fileContents);
                Console.WriteLine("---------------------------------");
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine($"Build Result: {buildResult.TargetResults.Build.Result}");
        }
    }
}
