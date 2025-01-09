using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen.TypeResolvers
{
    public class DateTimeTypeResolver : ITypeResolver
    {
        public ResolvedType? Resolve(Type type, bool optional, ITypeResolver recursiveResolver)
        {
            if (type == typeof(DateTime))
            {
                return new ResolvedType(optional, "string");
            }
            else if (type == typeof(DateTimeOffset))
            {
                return new ResolvedType(optional, "string");
            }
            else if (type == typeof(TimeSpan))
            {
                return new ResolvedType(optional, "string");
            }
            else if (type == typeof(DateOnly))
            {
                return new ResolvedType(optional, "string");
            }
            else if (type == typeof(TimeOnly))
            {
                return new ResolvedType(optional, "string");
            }

            return null;
        }
    }
}
