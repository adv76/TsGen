using System.Reflection;
using System.Text.Json.Serialization;
using TsGen.Attributes;
using TsGen.Enums;
using TsGen.Extensions;
using TsGen.Interfaces;

namespace TsGen.Models
{
    public class PropertyDef
    {
        public string Name { get; set; } = string.Empty;
        public Optionality Optionality { get; set; } = Optionality.Default;
        public string TypeName { get; set; } = string.Empty;
        public List<Type> DependentTypes { get; set; }

        public PropertyDef()
        {
            DependentTypes = new List<Type>();
        }

        public PropertyDef(string name, Optionality optionality, string typeName)
        {
            Name = name;
            Optionality = optionality;
            TypeName = typeName;
            DependentTypes = new List<Type>();
        }

        public PropertyDef(string name, Optionality optionality, string typeName, params Type[] dependentTypes)
        {
            Name = name;
            Optionality = optionality;
            TypeName = typeName;
            DependentTypes = new List<Type>();
            DependentTypes.AddRange(dependentTypes);
        }

        public PropertyDef(string name, Optionality optionality, string typeName, List<Type> dependentTypes)
        {
            Name = name;
            Optionality = optionality;
            TypeName = typeName;
            DependentTypes = dependentTypes;
        }

        public PropertyDef(string name, PropertyType type)
        {
            Name = name;
            Optionality = type.Optionality;
            TypeName = type.TypeName;
            DependentTypes = type.DependentTypes;
        }

        public static PropertyDef Build(PropertyInfo propertyInfo, IPropertyTypeResolver typeResolver, TsGenSettings generatorSettings)
        {
            string propName;

            var jsonPropNameAttr = propertyInfo.GetCustomAttribute<JsonPropertyNameAttribute>();
            if (jsonPropNameAttr is not null)
            {
                propName = jsonPropNameAttr.Name;
            }
            else
            {
                propName = generatorSettings.PropertyNamingPolicy.ConvertName(propertyInfo.Name);
            }

            var tsPropGenAttr = propertyInfo.GetCustomAttribute<TsPropGenAttribute>();

            var nullable = propertyInfo.IsNullable(out var propType);
            var optionality = tsPropGenAttr?.HasCustomOptionality ?? false
                ? tsPropGenAttr.Nullability
                : nullable
                    ? generatorSettings.DefaultNullablePropertyOptionality
                    : generatorSettings.DefaultPropertyOptionality;

            if (tsPropGenAttr is not null && tsPropGenAttr.HasCustomType)
            {
                return new PropertyDef(propName, new PropertyType(optionality, tsPropGenAttr.CustomType));
            }

            return new PropertyDef(propName, typeResolver.Resolve(propType, optionality, typeResolver) ?? new PropertyType(optionality, "unknown"));
        }

        public static PropertyDef Build(FieldInfo fieldInfo, IPropertyTypeResolver typeResolver, TsGenSettings generatorSettings)
        {
            string fieldName;

            var jsonPropNameAttr = fieldInfo.GetCustomAttribute<JsonPropertyNameAttribute>();
            if (jsonPropNameAttr is not null)
            {
                fieldName = jsonPropNameAttr.Name;
            }
            else
            {
                fieldName = generatorSettings.PropertyNamingPolicy.ConvertName(fieldInfo.Name);
            }

            var tsPropGenAttr = fieldInfo.GetCustomAttribute<TsPropGenAttribute>();

            var nullable = fieldInfo.IsNullable(out var fieldType);
            var optionality = tsPropGenAttr?.HasCustomOptionality ?? false
                ? tsPropGenAttr.Nullability
                : nullable
                    ? generatorSettings.DefaultNullablePropertyOptionality
                    : generatorSettings.DefaultPropertyOptionality;

            if (tsPropGenAttr is not null && tsPropGenAttr.HasCustomType)
            {
                return new PropertyDef(fieldName, new PropertyType(optionality, tsPropGenAttr.CustomType));
            }

            return new PropertyDef(fieldName, typeResolver.Resolve(fieldType, optionality, typeResolver) ?? new PropertyType(optionality, "unknown"));
        }
    }
}
