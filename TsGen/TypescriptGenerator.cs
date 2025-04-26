using System.IO;
using System.Reflection;
using System.Text;
using TsGen.Attributes;
using TsGen.Builders.TypeBuilders;
using TsGen.Extensions;
using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen
{
    public static class TypescriptGenerator
    {
        private const int MaxRecursionIterations = 5;

        public static IEnumerable<TypeDef> GenerateTypeDefs(Assembly assembly, TsGenSettings generatorSettings)
            => GenerateTypeDefs(assembly.GetTypes().Where(t => Attribute.IsDefined(t, typeof(TsGenAttribute))), generatorSettings);

        public static IEnumerable<TypeDef> GenerateTypeDefs(IEnumerable<Type> types, TsGenSettings generatorSettings)
            => GenerateTypeDefsInternal(types, generatorSettings);

        public static IEnumerable<TypeFile> GenerateTypeFiles(Assembly assembly, TsGenSettings generatorSettings)
        {
            var typeDefs = GenerateTypeDefs(assembly, generatorSettings);
            return BuildTypeFiles(typeDefs, generatorSettings);
        }

        public static IEnumerable<TypeFile> GenerateTypeFiles(IEnumerable<Type> types, TsGenSettings generatorSettings)
        {
            var typeDefs = GenerateTypeDefs(types, generatorSettings);
            return BuildTypeFiles(typeDefs, generatorSettings);
        }

        private static List<TypeFile> BuildTypeFiles(IEnumerable<TypeDef> typeDefs, TsGenSettings generatorSettings)
        {
            var typeFiles = new List<TypeFile>();
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
                    //var depPath = depNs.Replace('.', '/');

                    //typeFile.Imports.Add($"import {{ { string.Join(", ", dep.Select(dep => dep.Name.Sanitize()).Distinct()) } }} from {Path.GetRelativePath(nsPath, depPath).Replace('\\', '/')};");
                    typeFile.Imports.Add(GetImports(ns, depNs, dep));
                }

                foreach (var type in typeGroup)
                {
                    typeFile.TypeMap[type.Type] = type.TypeText;
                }

                typeFiles.Add(typeFile);
            }
            return typeFiles;
        }

        private static string GetImports(string currentNamespace, string importNamespace, IEnumerable<Type> types)
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
            bldr.Append(Path.GetRelativePath(currentPath, importPath).Replace('\\', '/'));
            bldr.Append("\";");

            return bldr.ToString();
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
                .Select(t => ((t.TsGenProps?.HasCustomTypeBuilder ?? false) 
                    ? t.TsGenProps.TypeBuilder 
                    : t.Type.IsEnum
                        ? generatorSettings.DefaultEnumBuilder
                        : generatorSettings.DefaultTypeBuilder).Build(t.Type, true, generatorSettings));
            

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
