using System.Reflection;
using System.Text;
using TsGen.Attributes;
using TsGen.Builders.PropertyBuilders;
using TsGen.Extensions;
using TsGen.Interfaces;
using TsGen.Models;
using TsGen.TypeResolvers;

namespace TsGen.Builders.TypeBuilders
{
    public class TypeBuilder : ITypeBuilder
    {
        public TypeDef Build(Type type, bool export)
        {
            var properties = type.GetProperties()
                .Union(type.GetPropertiesWithAttribute<TsPropGenAttribute>());

            var stringBldr = new StringBuilder();

            if (export) stringBldr.Append("export ");

            stringBldr.Append("type ");
            stringBldr.Append(type.Name);
            stringBldr.AppendLine(" = {");

            var propDefs = OutputProperties(properties, stringBldr);

            stringBldr.AppendLine("};");

            var propertyBldr = new DefaultPropertyBuilder();

            var deptTypes = propDefs
                .Where(p => p.DependentTypes is not null)
                .SelectMany(p => p.DependentTypes!)
                .Distinct();

            return new TypeDef(type, stringBldr.ToString(), deptTypes.ToList());
        }

        private static List<PropertyDef> OutputProperties(IEnumerable<PropertyInfo> properties, StringBuilder stringBldr)
        {
            var propDefs = new List<PropertyDef>();

            //var propertyBldr = new DefaultPropertyBuilder();
            //var fallbackBldr = new FallbackPropertyBuilder();

            var resolver = new DefaultTypeResolver();

            foreach (var property in properties)
            {
                var name = GeneratorSettings.PropertyNamingPolicy.ConvertName(property.Name);

                var nullable = property.IsNullable(out var propType);

                //var propertyDef = propertyBldr.Build(name, propType, nullable) ?? fallbackBldr.Build(name, propType, nullable);
                var propertyDef = new PropertyDef(name, resolver.Resolve(propType, nullable, resolver) ?? new ResolvedType(nullable, "unknown"));
                propDefs.Add(propertyDef);

                stringBldr.Append("  ");
                stringBldr.Append(propertyDef.Name);

                if (propertyDef.Optional)
                {
                    stringBldr.Append('?');
                }

                stringBldr.Append(": ");
                stringBldr.Append(propertyDef.TypeName);
                stringBldr.AppendLine(";");
            }

            return propDefs;
        }
    }
}
