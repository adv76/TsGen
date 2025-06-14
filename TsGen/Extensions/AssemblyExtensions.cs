using System.Reflection;
using TsGen.Models;

namespace TsGen.Extensions
{
    /// <summary>
    /// Assembly extensions for generating types from an assembly
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Generates type definitions for an assembly
        /// </summary>
        /// <param name="assembly">The assembly to search for types</param>
        /// <param name="generatorSettings">The generator settings to use</param>
        /// <returns>An Enumerable of Type Definitions</returns>
        public static IEnumerable<TypeDefinition> GenerateTypeDefs(this Assembly assembly, TsGenSettings generatorSettings)
            => TypescriptGenerator.GenerateTypeDefs(assembly, generatorSettings);

        /// <summary>
        /// Generates type files for an assembly
        /// </summary>
        /// <param name="assembly">The assembly to search for types</param>
        /// <param name="generatorSettings">The generator settings to use</param>
        /// <returns>An Enumerable of Type Files</returns>
        public static IEnumerable<TypeFile> GenerateTypeFiles(this Assembly assembly, TsGenSettings generatorSettings)
            => TypescriptGenerator.GenerateTypeFiles(assembly, generatorSettings);

        /// <summary>
        /// Generates Typescript files and outputs them to disk
        /// </summary>
        /// <param name="assembly">The assembly to search for types</param>
        /// <param name="generatorSettings">The generator settings to use</param>
        public static void GenerateAndOutputTypeFiles(this Assembly assembly, TsGenSettings generatorSettings)
            => TypescriptGenerator.GenerateAndOutputTypeFiles(assembly, generatorSettings);

        /// <summary>
        /// Generates Typescript files and asynchronously outputs them to disk
        /// </summary>
        /// <param name="assembly">The assembly to search for types</param>
        /// <param name="generatorSettings">The generator settings to use</param>
        /// <returns>A task for the operation</returns>
        public static Task GenerateAndOutputTypeFilesAsync(this Assembly assembly, TsGenSettings generatorSettings)
            => TypescriptGenerator.GenerateAndOutputTypeFilesAsync(assembly, generatorSettings);
    }
}
