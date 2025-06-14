using TsGen.Enums;
using TsGen.Models;

namespace TsGen.Interfaces
{
    /// <summary>
    /// Resolves property types
    /// </summary>
    public interface IPropertyTypeResolver
    {
        /// <summary>
        /// Resolves a property type if it can
        /// </summary>
        /// <param name="type">The type to resolve</param>
        /// <param name="optionality">The optionality setting for the type</param>
        /// <param name="recursiveResolver">The resolver to use if necessary for nested types</param>
        /// <param name="generatorSettings">The generator settings to use</param>
        /// <returns>A property type (or null if no type could be resolved)</returns>
        public PropertyType? Resolve(Type type, Optionality optionality, IPropertyTypeResolver recursiveResolver, TsGenSettings generatorSettings);
    }
}
