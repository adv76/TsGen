using System.Reflection;
using TsGen.Attributes;
using TsGen.Extensions;
using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen
{
    public static class TypescriptGenerator
    {
        private const int MaxRecursionIterations = 5;

        public static IEnumerable<TypeDef> GenerateTypeDefs(Assembly assembly, TsGenSettings generatorSettings)
            => GenerateTypeDefsInternal(assembly.GetTypes().Where(t => Attribute.IsDefined(t, typeof(TsGenAttribute))), generatorSettings);

        public static IEnumerable<TypeDef> GenerateTypeDefs(IEnumerable<Type> types, TsGenSettings generatorSettings)
            => GenerateTypeDefsInternal(types, generatorSettings);

        public static IEnumerable<TypeFile> GenerateTypeFiles(Assembly assembly, TsGenSettings generatorSettings)
        {
            var typeFiles = new List<TypeFile>();

            var typeDefs = GenerateTypeDefs(assembly, generatorSettings);
            foreach (var typeGroup in typeDefs.GroupBy(t => t.Type.Namespace))
            {
                var ns = typeGroup.Key ?? string.Empty;
                var nsPath = ns.Replace('.', '/');

                var typeFile = new TypeFile()
                {
                    Namespace = ns,
                    RelativePath = nsPath
                };

                var deps = typeGroup.SelectMany(t => t.DependentTypes).Where(n => n.Namespace != typeGroup.Key).GroupBy(t => t.Namespace);
                foreach (var dep in deps)
                {
                    var depNs = dep.Key ?? string.Empty;
                    var depPath = depNs.Replace('.', '/');

                    typeFile.Imports.Add($"import {{ {string.Join(", ", dep.Select(dep => dep.Name.Sanitize()).Distinct())} }} from \"{Path.GetRelativePath(ns, depPath).Replace('\\', '/')}\";");
                }

                foreach (var type in typeGroup)
                {
                    typeFile.TypeMap[type.Type] = type.TypeText;
                }

                typeFiles.Add(typeFile);
            }

            return typeFiles;
        }

        public static IEnumerable<TypeFile> GenerateTypeFiles(IEnumerable<Type> types, TsGenSettings generatorSettings)
        {
            var typeFiles = new List<TypeFile>();

            var typeDefs = GenerateTypeDefs(types, generatorSettings);
            foreach (var typeGroup in typeDefs.GroupBy(t => t.Type.Namespace))
            {
                var ns = typeGroup.Key ?? string.Empty;
                var nsPath = ns.Replace('.', '/');

                var typeFile = new TypeFile()
                {
                    Namespace = ns,
                    RelativePath = nsPath
                };

                var deps = typeGroup.SelectMany(t => t.DependentTypes).Where(n => n.Namespace != typeGroup.Key).GroupBy(t => t.Namespace);
                foreach (var dep in deps)
                {
                    var depNs = dep.Key ?? string.Empty;
                    var depPath = depNs.Replace('.', '/');

                    typeFile.Imports.Add($"import {{ { string.Join(", ", dep.Select(dep => dep.Name.Sanitize()).Distinct()) } }} from {Path.GetRelativePath(ns, depPath).Replace('\\', '/')};");
                }
                
                foreach (var type in typeGroup)
                {
                    typeFile.TypeMap[type.Type] = type.TypeText;
                }

                typeFiles.Add(typeFile);
            }

            return typeFiles;
        }

        public static void GenerateAndOutputTypeFiles(Assembly assembly, TsGenSettings generatorSettings)
        {
            var typeFiles = GenerateTypeFiles(assembly, generatorSettings);

            foreach (var file in typeFiles)
            {
                var path = file.ToFile(generatorSettings.OutputDirectory, out var fileContents);

                Directory.CreateDirectory(Path.GetDirectoryName(path)!); // TODO Fix
                File.WriteAllText(path, fileContents);
            }
        }

        public static async Task GenerateAndOutputTypeFilesAsync(Assembly assembly, TsGenSettings generatorSettings)
        {
            var typeFiles = GenerateTypeFiles(assembly, generatorSettings);

            foreach (var file in typeFiles)
            {
                var path = file.ToFile(generatorSettings.OutputDirectory, out var fileContents);

                Directory.CreateDirectory(Path.GetDirectoryName(path)!); // TODO Fix
                await File.WriteAllTextAsync(path, fileContents);
            }
        }

        private static IEnumerable<TypeDef> GenerateTypeDefsInternal(IEnumerable<Type> types, TsGenSettings generatorSettings, int level = 0)
        {
            var typeDefs = types
                .Select(t => new { Type = t, TsGenProps = t.GetCustomAttribute<TsGenAttribute>() })
                .Select(t => ((t.TsGenProps?.HasCustomTypeBuilder ?? false) ? t.TsGenProps.TypeBuilder : generatorSettings.DefaultTypeBuilder).Build(t.Type, true));
            

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
