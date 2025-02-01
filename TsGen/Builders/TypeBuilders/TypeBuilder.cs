using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
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
                .Union(type.GetPropertiesWithAttribute<TsPropGenAttribute>())
                .Union(type.GetPropertiesWithAttribute<JsonIncludeAttribute>())
                .Except(type.GetPropertiesWithAttribute<JsonIgnoreAttribute>());

            var stringBldr = new StringBuilder();

            if (export) stringBldr.Append("export ");

            stringBldr.Append("type ");
            stringBldr.Append(type.Name.Sanitize());

            if (type.IsGenericTypeDefinition)
            {
                var args = type.GetGenericArguments().Select(t => t.Name);

                stringBldr.Append('<');
                stringBldr.Append(string.Join(", ", args));
                stringBldr.Append('>');
            }

            stringBldr.AppendLine(" = {");

            var propDefs = OutputProperties(properties, stringBldr);

            stringBldr.AppendLine("};");

            IEnumerable<Type> deptTypes;

            if (type.IsGenericTypeDefinition)
            {
                deptTypes = new List<Type>();
            }
            else
            {
                deptTypes = propDefs
                    .SelectMany(p => p.DependentTypes)
                    .Distinct();
            }

            return new TypeDef(type, stringBldr.ToString(), deptTypes.ToList());
        }

        private static List<PropertyDef> OutputProperties(IEnumerable<PropertyInfo> properties, StringBuilder stringBldr)
        {
            var propDefs = new List<PropertyDef>();

            var resolver = new DefaultTypeResolver();

            foreach (var property in properties)
            {
                string propName;

                var jsonPropNameAttr = property.GetCustomAttribute<JsonPropertyNameAttribute>();
                if (jsonPropNameAttr is not null)
                {
                    propName = jsonPropNameAttr.Name;
                }
                else
                {
                    propName = new TsGenSettings().PropertyNamingPolicy.ConvertName(property.Name);
                }

                var nullable = property.IsNullable(out var propType);

                PropertyDef propertyDef;

                var tsPropGenAttr = property.GetCustomAttribute<TsPropGenAttribute>();
                if (tsPropGenAttr is not null && tsPropGenAttr.HasCustomType)
                {
                    propertyDef = new PropertyDef(propName, new ResolvedType(nullable, tsPropGenAttr.CustomType));
                }
                else
                {
                    propertyDef = new PropertyDef(propName, resolver.Resolve(propType, nullable, resolver) ?? new ResolvedType(nullable, "unknown"));
                }
                
                propDefs.Add(propertyDef);

                stringBldr.Append("  ");
                stringBldr.Append(propertyDef.Name);

                if (propertyDef.Optional)
                {
                    stringBldr.Append('?');
                }

                stringBldr.Append(": ");
                stringBldr.Append(propertyDef.TypeName.Sanitize());
                stringBldr.AppendLine(";");
            }

            return propDefs;
        }
    }
}
