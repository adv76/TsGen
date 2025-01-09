using System.Reflection;

namespace TsGen.Extensions
{
    internal static class TypeExtensions
    {
        public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute<T>(this Type type) where T : Attribute
            => type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(p => Attribute.IsDefined(p, typeof(T)));
    }
}
