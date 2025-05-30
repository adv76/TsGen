﻿using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using TsGen.Attributes;
using TsGen.Enums;
using TsGen.Extensions;
using TsGen.Interfaces;
using TsGen.Models;
using TsGen.TypeResolvers;

namespace TsGen.Builders.TypeBuilders
{
    /// <summary>
    /// A type builder that outputs typescript types
    /// </summary>
    public class TypeBuilder : ITypeBuilder
    {
        /// <summary>
        /// Builds a type definition for a type
        /// </summary>
        /// <param name="type">The type to build</param>
        /// <param name="export">Whether it should be exported</param>
        /// <param name="generatorSettings">The generator settings to use</param>
        /// <returns>A type definition for the supplied type</returns>
        public TypeDefinition Build(Type type, bool export, TsGenSettings generatorSettings)
        {
            var properties = type.GetProperties()
                .Union(type.GetPropertiesWithAttribute<TsPropGenAttribute>())
                .Union(type.GetPropertiesWithAttribute<JsonIncludeAttribute>())
                .Except(type.GetPropertiesWithAttribute<JsonIgnoreAttribute>());

            var stringBldr = new StringBuilder();

            if (export) stringBldr.Append("export ");

            stringBldr.Append("type ");
            stringBldr.Append(generatorSettings.TypeNamingPolicy.ConvertName(type.Name.Sanitize()));

            if (type.IsGenericTypeDefinition)
            {
                var args = type.GetGenericArguments().Select(t => t.Name);

                stringBldr.Append('<');
                stringBldr.Append(string.Join(", ", args));
                stringBldr.Append('>');
            }

            stringBldr.AppendLine(" = {");

            var propDefs = OutputProperties(properties, stringBldr, generatorSettings);

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

            return new TypeDefinition(type, stringBldr.ToString(), deptTypes.ToList());
        }

        private static List<PropertyDef> OutputProperties(IEnumerable<PropertyInfo> properties, StringBuilder stringBldr, TsGenSettings generatorSettings)
        {
            var propDefs = new List<PropertyDef>();

            var resolver = new DefaultTypeResolver();

            foreach (var property in properties)
            {
                var propertyDef = PropertyDef.Build(property, resolver, generatorSettings);
                
                propDefs.Add(propertyDef);

                stringBldr.Append(generatorSettings.Indentation);
                stringBldr.Append(propertyDef.Name);

                if (propertyDef.Optionality == Optionality.Optional)
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
