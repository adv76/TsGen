using System.Reflection;
using TsGen.Models;

namespace TsGen.Extensions
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<TypeDef> GenerateTypeDefs(this Assembly assembly, GeneratorSettingsBase generatorSettings)
            => TypescriptGenerator.GenerateTypeDefs(assembly, generatorSettings);

        public static IEnumerable<TypeFile> GenerateTypeFiles(this Assembly assembly, GeneratorSettingsBase generatorSettings)
            => TypescriptGenerator.GenerateTypeFiles(assembly, generatorSettings);

        public static void GenerateAndOutputTypeFiles(this Assembly assembly, GeneratorSettingsBase generatorSettings)
            => TypescriptGenerator.GenerateAndOutputTypeFiles(assembly, generatorSettings);

        public static Task GenerateAndOutputTypeFilesAsync(this Assembly assembly, GeneratorSettingsBase generatorSettings)
            => TypescriptGenerator.GenerateAndOutputTypeFilesAsync(assembly, generatorSettings);
    }
}
