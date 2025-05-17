using System.Reflection;
using TsGen.Attributes;
using TsGen.Interfaces;

namespace TsGen.Extensions
{
    internal static class TypeExtensions
    {
        public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute<T>(this Type type) where T : Attribute
            => type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(p => Attribute.IsDefined(p, typeof(T)));

        public static ITypeBuilder GetTypeBuilder(this Type type, TsGenSettings generatorSettings)
        {
            var attribute = type.GetCustomAttribute<TsGenAttribute>();
            if (attribute is not null && attribute.HasCustomTypeBuilder)
            {
                return attribute.TypeBuilder;
            }
            else if (type.IsEnum)
            {
                return generatorSettings.DefaultEnumBuilder;
            }
            else
            {
                return generatorSettings.DefaultTypeBuilder;
            }
        }
    }
}
