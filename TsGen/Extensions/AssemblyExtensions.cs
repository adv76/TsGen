using System.Reflection;
using TsGen.Attributes;
using TsGen.Models;

namespace TsGen.Extensions
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<TypeDef> GenerateTypeDefs(this Assembly assembly)
            => TypescriptGenerator.GenerateTypeDefs(assembly);

        public static IEnumerable<TypeFile> GenerateTypeFiles(this Assembly assembly)
            => TypescriptGenerator.GenerateTypeFiles(assembly);
    }
}
