using TsGen.Enums;
using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen.TypeResolvers
{
    public class ObjectTypeResolver : IPropertyTypeResolver
    {
        /// <summary>
        /// Attempts to resovlve a Typescript type from the passed in type
        /// </summary>
        /// <param name="type">The type to attempt to resolve.</param>
        /// <param name="optionality">Whether or not the resolved type should be optional.</param>
        /// <param name="recursiveResolver">The recursive resolver for resolving nested types.</param>
        /// <returns>A resolved type if the type can be handled by this resolver (see list in class description) otherwise null.</returns>
        public PropertyType Resolve(Type type, Optionality optionality, IPropertyTypeResolver recursiveResolver)
        {
            return new PropertyType(optionality, type.Name, type);
        }
    }
}
