using System.Text;
using TsGen.Extensions;
using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen.Builders.TypeBuilders
{
    public class EnumBuilder : ITypeBuilder
    {
        public TypeDef Build(Type type, bool export)
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

                    stringBldr.Append("  ");
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
