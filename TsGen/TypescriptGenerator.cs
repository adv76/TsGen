using System.Reflection;
using TsGen.Attributes;
using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen
{
    public static class TypescriptGenerator
    {
        private const int MaxRecursionIterations = 5;

        public static IEnumerable<TypeDef> GenerateTypeDefs(Assembly assembly)
        {
            var defaultBldr = GeneratorSettings.DefaultTypeBuilder;

            return GenerateTypeDefs(assembly.GetTypes().Where(t => Attribute.IsDefined(t, typeof(TsGenAttribute))), defaultBldr);
        }

        public static IEnumerable<TypeFile> GenerateTypeFiles(Assembly assembly)
        {
            var typeFiles = new List<TypeFile>();

            var typeDefs = GenerateTypeDefs(assembly);
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

                    typeFile.Imports.Add($"import {{ { string.Join(", ", dep.Select(dep => dep.Name)) } }} from {Path.GetRelativePath(ns, depPath).Replace('\\', '/')};");
                }
                
                foreach (var type in typeGroup)
                {
                    typeFile.TypeMap[type.Type] = type.TypeText;
                }

                typeFiles.Add(typeFile);
            }

            return typeFiles;
        }

        private static IEnumerable<TypeDef> GenerateTypeDefs(IEnumerable<Type> types, ITypeBuilder defaultBuilder, int level = 0)
        {
            var typeDefs = types
                .Select(t => new { Type = t, TsGenProps = t.GetCustomAttribute<TsGenAttribute>() })
                .Select(t => ((t.TsGenProps?.HasCustomTypeBuilder ?? false) ? t.TsGenProps.TypeBuilder : defaultBuilder).Build(t.Type, true));
            

            var dependentTypes = typeDefs
                .SelectMany(td => td.DependentTypes ?? []);

            if (dependentTypes.Any() && level < MaxRecursionIterations)
            {
                typeDefs = typeDefs.UnionBy(GenerateTypeDefs(dependentTypes, defaultBuilder, level + 1), td => td.Type);
            }

            return typeDefs;
        }
    }
}
