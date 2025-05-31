using System.Reflection;
using System.Text.Json.Serialization;
using TsGen.Attributes;
using TsGen.Enums;
using TsGen.Extensions;
using TsGen.Interfaces;

namespace TsGen.Models
{
    /// <summary>
    /// Represents the definition of a Typescript property
    /// </summary>
    public class PropertyDef
    {
        /// <summary>
        /// The name of the property
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The optionality of the property
        /// </summary>
        public Optionality Optionality { get; set; } = Optionality.Default;

        /// <summary>
        /// The string resolved type of the property
        /// </summary>
        public string TypeName { get; set; } = string.Empty;

        /// <summary>
        /// Any types that this property may depend on (e.g. other classes, generics, etc.)
        /// </summary>
        public List<Type> DependentTypes { get; set; }

        /// <summary>
        /// Default constructor that ensures all properties are initialized
        /// </summary>
        public PropertyDef()
        {
            DependentTypes = new List<Type>();
        }

        /// <summary>
        /// Constructs a property definition with no dependent types
        /// </summary>
        /// <param name="name">The property name</param>
        /// <param name="optionality">The property optionality</param>
        /// <param name="typeName">The property resolved type</param>
        public PropertyDef(string name, Optionality optionality, string typeName)
        {
            Name = name;
            Optionality = optionality;
            TypeName = typeName;
            DependentTypes = new List<Type>();
        }

        /// <summary>
        /// Constructs a property definition with one or more dependent types
        /// </summary>
        /// <param name="name">The property name</param>
        /// <param name="optionality">The property optionality</param>
        /// <param name="typeName">the property resolved type</param>
        /// <param name="dependentTypes">The types this property depends on</param>
        public PropertyDef(string name, Optionality optionality, string typeName, params Type[] dependentTypes)
        {
            Name = name;
            Optionality = optionality;
            TypeName = typeName;
            DependentTypes = new List<Type>();
            DependentTypes.AddRange(dependentTypes);
        }

        /// <summary>
        /// Constructs a property definition with a list of dependent types
        /// </summary>
        /// <param name="name">The property name</param>
        /// <param name="optionality">The property optionality</param>
        /// <param name="typeName">the property resolved type</param>
        /// <param name="dependentTypes">The types this property depends on</param>
        public PropertyDef(string name, Optionality optionality, string typeName, List<Type> dependentTypes)
        {
            Name = name;
            Optionality = optionality;
            TypeName = typeName;
            DependentTypes = dependentTypes;
        }

        /// <summary>
        /// Constructs a property definition from a property type
        /// </summary>
        /// <param name="name">The property name</param>
        /// <param name="type">The property type</param>
        public PropertyDef(string name, PropertyType type)
        {
            Name = name;
            Optionality = type.Optionality;
            TypeName = type.TypeName;
            DependentTypes = type.DependentTypes;
        }

        /// <summary>
        /// Builds a property definition from the PropertyInfo reflection metadata
        /// </summary>
        /// <param name="propertyInfo">The property info metadata</param>
        /// <param name="typeResolver">The type resolver to use to resolve the type</param>
        /// <param name="generatorSettings">The generator settings to use</param>
        /// <returns>A property definition for the property info</returns>
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

            return new PropertyDef(propName, typeResolver.Resolve(propType, optionality, typeResolver, generatorSettings) ?? new PropertyType(optionality, "unknown"));
        }

        /// <summary>
        /// Builds a property definition from the FieldInfo reflection metadata
        /// </summary>
        /// <param name="fieldInfo">The field info metadata</param>
        /// <param name="typeResolver">The type resolver to use to resolve the type</param>
        /// <param name="generatorSettings">The generator settings to use</param>
        /// <returns>A property definition for the field info</returns>
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

            return new PropertyDef(fieldName, typeResolver.Resolve(fieldType, optionality, typeResolver, generatorSettings) ?? new PropertyType(optionality, "unknown"));
        }
    }
}
