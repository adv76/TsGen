using System.Reflection;
using TsGen.Models;

namespace TsGen.Extensions
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<TypeDefinition> GenerateTypeDefs(this Assembly assembly, TsGenSettings generatorSettings)
            => TypescriptGenerator.GenerateTypeDefs(assembly, generatorSettings);

        public static IEnumerable<TypeFile> GenerateTypeFiles(this Assembly assembly, TsGenSettings generatorSettings)
            => TypescriptGenerator.GenerateTypeFiles(assembly, generatorSettings);

        public static void GenerateAndOutputTypeFiles(this Assembly assembly, TsGenSettings generatorSettings)
            => TypescriptGenerator.GenerateAndOutputTypeFiles(assembly, generatorSettings);

        public static Task GenerateAndOutputTypeFilesAsync(this Assembly assembly, TsGenSettings generatorSettings)
            => TypescriptGenerator.GenerateAndOutputTypeFilesAsync(assembly, generatorSettings);
    }
}
