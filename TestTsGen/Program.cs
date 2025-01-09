using System.Reflection;
using TsGen;
using TsGen.Extensions;

var files = Assembly.GetExecutingAssembly().GenerateTypeFiles();

foreach (var file in files)
{
    var path = file.ToFile(GeneratorSettings.BaseDirectory, out var fileContents);

    Console.WriteLine(path);
    Console.WriteLine();
    Console.WriteLine(fileContents);
    Console.WriteLine("---------------------------------");
    Console.WriteLine();
}
