using System.Reflection;
using TsGen;
using TsGen.Extensions;

var files = Assembly.GetExecutingAssembly().GenerateTypeFiles(new DefaultGeneratorSettings());

foreach (var file in files)
{
    var path = file.ToFile(new DefaultGeneratorSettings().OutputDirectory, out var fileContents);

    Console.WriteLine(path);
    Console.WriteLine();
    Console.WriteLine(fileContents);
    Console.WriteLine("---------------------------------");
    Console.WriteLine();
}
