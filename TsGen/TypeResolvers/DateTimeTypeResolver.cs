using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen.TypeResolvers
{
    public class DateTimeTypeResolver : ITypeResolver
    {
        /// <summary>
        /// Attempts to resovlve a Typescript type from the passed in type
        /// </summary>
        /// <param name="type">The type to attempt to resolve.</param>
        /// <param name="optional">Whether or not the resolved type should be optional.</param>
        /// <param name="recursiveResolver">The recursive resolver for resolving nested types.</param>
        /// <returns>A resolved type if the type can be handled by this resolver (see list in class description) otherwise null.</returns>
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
