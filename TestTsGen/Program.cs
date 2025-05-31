using System.Reflection;
using TsGen;
using TsGen.Extensions;

var files = Assembly.GetExecutingAssembly().GenerateTypeFiles(new TsGenSettings());

foreach (var file in files)
{
    var path = file.ToFile(new TsGenSettings().OutputDirectories[0], out var fileContents);

    Console.WriteLine(path);
    Console.WriteLine();
    Console.WriteLine(fileContents);
    Console.WriteLine("---------------------------------");
    Console.WriteLine();
}
