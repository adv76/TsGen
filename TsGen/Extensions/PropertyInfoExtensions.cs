using System.Reflection;

namespace TsGen.Extensions
{
    internal static class PropertyInfoExtensions
    {
        private static readonly NullabilityInfoContext _nullabilityContext = new();

        public static bool IsNullable(this PropertyInfo propertyInfo, out Type type)
        {
            var nullability = _nullabilityContext.Create(propertyInfo);

            var nullable = false;
            type = propertyInfo.PropertyType;

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
