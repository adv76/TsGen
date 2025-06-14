using System.Diagnostics;
using System.Reflection;
using System.Text;
using TsGen.Attributes;
using TsGen.Enums;
using TsGen.Extensions;
using TsGen.Models;

namespace TsGen
{
    /// <summary>
    /// Static functions for building TypeScript types
    /// </summary>
    public static class TypescriptGenerator
    {
        // Sets the maximum number of times to try to resolve types. If you have types that use other types that use other
        // types and so on, you could get stuck in an infinite loop. This sets the maximum number of times to recurse.
        private const int MaxRecursionIterations = 10;

        /// <summary>
        /// Generates Type Definitions for an assembly
        /// </summary>
        /// <remarks>
        /// All types annotated with the <see cref="TsGenAttribute"/> will be generated as well as any types
        /// that those types rely on
        /// </remarks>
        /// <param name="assembly">The assembly to search for types</param>
        /// <param name="generatorSettings">The generator settings to use</param>
        /// <returns>An Enumerable of <see cref="TypeDefinition"/></returns>
        public static IEnumerable<TypeDefinition> GenerateTypeDefs(Assembly assembly, TsGenSettings generatorSettings)
            => GenerateTypeDefs(
                assembly
                    .GetTypes()
                    .Where(t => Attribute.IsDefined(t, typeof(TsGenAttribute)))
                    .Union(generatorSettings.AdditionalTypes), 
                generatorSettings);

        /// <summary>
        /// Generates Type Definitions from a list of types
        /// </summary>
        /// <param name="types">The types to generate</param>
        /// <param name="generatorSettings">The generator settings to use</param>
        /// <returns>An Enumerable of <see cref="TypeDefinition"/></returns>
        public static IEnumerable<TypeDefinition> GenerateTypeDefs(IEnumerable<Type> types, TsGenSettings generatorSettings)
            => GenerateTypeDefsInternal(types.Union(generatorSettings.AdditionalTypes), generatorSettings);

        /// <summary>
        /// Generates type files for an assembly
        /// </summary>
        /// <remarks>
        /// Uses <see cref="GenerateTypeDefs(Assembly, TsGenSettings)"/> to generate the type definitions and then builds
        /// the type files from the definitions
        /// </remarks>
        /// <param name="assembly">The assembly to search for types</param>
        /// <param name="generatorSettings">The generator settings to use</param>
        /// <returns>An Enumerable of <see cref="TypeFile"/></returns>
        public static IEnumerable<TypeFile> GenerateTypeFiles(Assembly assembly, TsGenSettings generatorSettings)
        {
            var typeDefs = GenerateTypeDefs(assembly, generatorSettings);
            return BuildTypeFiles(typeDefs, generatorSettings);
        }

        /// <summary>
        /// Generates type files for a list of types
        /// </summary>
        /// <remarks>
        /// Uses <see cref="GenerateTypeDefs(IEnumerable{Type}, TsGenSettings)"/> to generate the type definitions and
        /// then builds the type files from the defintions
        /// </remarks>
        /// <param name="types">The types to generate ts files for</param>
        /// <param name="generatorSettings">The generator settings to use</param>
        /// <returns>An Enumerable of <see cref="TypeFile"/></returns>
        public static IEnumerable<TypeFile> GenerateTypeFiles(IEnumerable<Type> types, TsGenSettings generatorSettings)
        {
            var typeDefs = GenerateTypeDefs(types, generatorSettings);
            return BuildTypeFiles(typeDefs, generatorSettings);
        }

        private static List<TypeFile> BuildTypeFiles(IEnumerable<TypeDefinition> typeDefs, TsGenSettings generatorSettings)
        {
            var typeFiles = new List<TypeFile>();
            foreach (var typeGroup in typeDefs.GroupBy(t => t.Type.Namespace))
            {
                var ns = typeGroup.Key ?? "Other";
                var nsPath = generatorSettings.ExportStructure == ExportStructure.DirectoryBased
                    ? Path.Combine(ns.Replace('.', '/'), "index.ts")
                    : ns + ".ts";

                var typeFile = new TypeFile()
                {
                    Namespace = ns,
                    RelativeFilePath = nsPath
                };

                var deps = typeGroup.SelectMany(t => t.DependentTypes).Where(n => n.Namespace != typeGroup.Key).GroupBy(t => t.Namespace);
                foreach (var dep in deps)
                {
                    var depNs = dep.Key ?? "Other";

                    typeFile.Imports.Add(BuildImport(ns, depNs, dep, generatorSettings));
                }

                foreach (var type in typeGroup)
                {
                    typeFile.TypeMap[type.Type] = type.TypeText;
                }

                typeFiles.Add(typeFile);
            }
            return typeFiles;
        }

        private static string BuildImport(string currentNamespace, string importNamespace, IEnumerable<Type> types, TsGenSettings generatorSettings)
        {
            var currentPath = currentNamespace.Replace('.', '/');
            var importPath = importNamespace.Replace('.', '/');

            var typeImports = types.Where(t => !t.IsEnum);
            var fullImports = types.Where(t => t.IsEnum);

            var bldr = new StringBuilder();
            bldr.Append("import ");

            if (fullImports.Any())
            {
                bldr.Append("{ ");
                bldr.AppendJoin(", ", fullImports.Select(t => t.Name.Sanitize()));

                if (typeImports.Any())
                {
                    bldr.Append(", type ");
                    bldr.AppendJoin(", type ", typeImports.Select(t => t.Name.Sanitize()));
                }

                bldr.Append(" }");
            }
            else
            {
                bldr.Append("type { ");
                bldr.AppendJoin(", ", typeImports.Select(t => t.Name.Sanitize()));
                bldr.Append(" }");
            }

            bldr.Append(" from \"");
            if (generatorSettings.ExportStructure == ExportStructure.DirectoryBased)
            {
                bldr.Append(Path.GetRelativePath(currentPath, importPath).Replace('\\', '/'));
            }
            else
            {
                bldr.Append("./");
                bldr.Append(importNamespace);
                Debug.WriteLine(importNamespace);
            }
            bldr.Append("\";");

            return bldr.ToString();
        }

        /// <summary>
        /// Outputs TypeScript files to disk
        /// </summary>
        /// <remarks>
        /// Generates a list of <see cref="TypeFile"/> using <see cref="GenerateTypeFiles(Assembly, TsGenSettings)"/>
        /// and saves them to disk. The disk location is set in the generator settings.
        /// </remarks>
        /// <param name="assembly">The assembly to search for types.</param>
        /// <param name="generatorSettings">The generator settings to use</param>
        public static void GenerateAndOutputTypeFiles(Assembly assembly, TsGenSettings generatorSettings)
        {
            var typeFiles = GenerateTypeFiles(assembly, generatorSettings);

            foreach (var dir in generatorSettings.OutputDirectories)
            {
                foreach (var file in typeFiles)
                {
                    var path = file.ToFile(dir, out var fileContents);

                    Directory.CreateDirectory(Path.GetDirectoryName(path)!); // TODO Fix
                    File.WriteAllText(path, fileContents);
                }
            }
        }

        /// <summary>
        /// Asynchronously outputs TypeScript files to disk
        /// </summary>
        /// <remarks>
        /// Generates a list of <see cref="TypeFile"/> using <see cref="GenerateTypeFiles(IEnumerable{Type}, TsGenSettings)"/>
        /// and saves them to disk. The disk location is set in the generator settings.
        /// </remarks>
        /// <param name="assembly">The assembly to search for type</param>
        /// <param name="generatorSettings">The generator settings to use</param>
        /// <returns>A task for the operation</returns>
        public static async Task GenerateAndOutputTypeFilesAsync(Assembly assembly, TsGenSettings generatorSettings)
        {
            var typeFiles = GenerateTypeFiles(assembly, generatorSettings);

            foreach (var dir in generatorSettings.OutputDirectories)
            {
                foreach (var file in typeFiles)
                {
                    var path = file.ToFile(dir, out var fileContents);

                    Directory.CreateDirectory(Path.GetDirectoryName(path)!); // TODO Fix
                    await File.WriteAllTextAsync(path, fileContents);
                }
            }
        }

        private static IEnumerable<TypeDefinition> GenerateTypeDefsInternal(IEnumerable<Type> types, TsGenSettings generatorSettings, int level = 0)
        {
            var typeDefs = types.Select(t => t.GetTypeBuilder(generatorSettings).Build(t, true, generatorSettings));

            var dependentTypes = typeDefs
                .SelectMany(td => td.DependentTypes);

            if (dependentTypes.Any() && level < MaxRecursionIterations)
            {
                typeDefs = typeDefs.UnionBy(GenerateTypeDefsInternal(dependentTypes, generatorSettings, level + 1), td => td.Type);
            }

            return typeDefs;
        }
    }
}
