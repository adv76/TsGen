using TsGen.Enums;
using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen.TypeResolvers
{
    public class DateTimeTypeResolver : IPropertyTypeResolver
    {
        /// <summary>
        /// Attempts to resovlve a Typescript type from the passed in type
        /// </summary>
        /// <param name="type">The type to attempt to resolve.</param>
        /// <param name="optional">Whether or not the resolved type should be optional.</param>
        /// <param name="recursiveResolver">The recursive resolver for resolving nested types.</param>
        /// <returns>A resolved type if the type can be handled by this resolver (see list in class description) otherwise null.</returns>
        public PropertyType? Resolve(Type type, Optionality optionality, IPropertyTypeResolver recursiveResolver)
        {
            if (type == typeof(DateTime))
            {
                return new PropertyType(optionality, "string");
            }
            else if (type == typeof(DateTimeOffset))
            {
                return new PropertyType(optionality, "string");
            }
            else if (type == typeof(TimeSpan))
            {
                return new PropertyType(optionality, "string");
            }
            else if (type == typeof(DateOnly))
            {
                return new PropertyType(optionality, "string");
            }
            else if (type == typeof(TimeOnly))
            {
                return new PropertyType(optionality, "string");
            }

            return null;
        }
    }
}
