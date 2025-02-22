using System.Reflection;

namespace TsGen.Extensions
{
    internal static class FieldInfoExtensions
    {
        private static readonly NullabilityInfoContext _nullabilityContext = new();

        public static bool IsNullable(this FieldInfo fieldInfo, out Type type)
        {
            var nullability = _nullabilityContext.Create(fieldInfo);

            var nullable = false;
            type = fieldInfo.FieldType;

            if (nullability.ReadState == NullabilityState.Nullable || nullability.WriteState == NullabilityState.Nullable)
            {
                nullable = true;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = type.GetGenericArguments()[0];
                }
                else
                {
                    type = nullability.Type;
                }
            }

            return nullable;
        }
    }
}
