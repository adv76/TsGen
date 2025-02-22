using System.Text;
using TsGen.Extensions;
using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen.Builders.TypeBuilders
{
    /// <summary>
    /// Typescript type builder for enums
    /// </summary>
    /// <remarks>
    /// If the supplied type is a .NET enum, it will output a Typescript enum with the fields and underlying numeric values.
    /// </remarks>
    public class EnumBuilder : ITypeBuilder
    {
        /// <summary>
        /// Builds a TypeDef for an enum
        /// </summary>
        /// <param name="type">The type to generate Typescript for</param>
        /// <param name="export">Whether or not the enum should be exported from the module</param>
        /// <param name="settings">The settings to use to build the type.</param>
        /// <returns>A TypeScript enum if the type is an enum otherwise an empty type def.</returns>
        public TypeDef Build(Type type, bool export, TsGenSettings settings)
        {
            if (type.IsEnum)
            {
                var stringBldr = new StringBuilder();

                if (export) stringBldr.Append("export ");

                stringBldr.Append("enum ");
                stringBldr.Append(type.Name.Sanitize());
                stringBldr.AppendLine(" {");

                var enumFields = type.GetFields();
                foreach (var field in enumFields)
                {
                    if (field.Name == "value__") continue;

                    stringBldr.Append(settings.Indentation);
                    stringBldr.Append(field.Name.Sanitize());
                    stringBldr.Append(" = ");
                    stringBldr.Append(field.GetRawConstantValue());
                    stringBldr.AppendLine(",");
                }

                stringBldr.AppendLine("};");

                return new TypeDef(type, stringBldr.ToString());
            }
            else
            {
                return new TypeDef(type);
            }
        }
    }
}
