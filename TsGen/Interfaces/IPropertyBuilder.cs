using TsGen.Enums;
using TsGen.Models;

namespace TsGen.Interfaces
{
    /// <summary>
    /// Interface for property builders.
    /// </summary>
    /// <remarks>
    /// A property builders resolve metadata and types for each property.
    /// </remarks>
    public interface IPropertyBuilder
    {
        /// <summary>
        /// Builds a property builder from a property name, property type, and optionality.
        /// </summary>
        /// <param name="name">The name of the property</param>
        /// <param name="type">The type of the property</param>
        /// <param name="optionality">The optionality of the property</param>
        /// <returns>A PropertyDef if the builder can handle the supplied type, otherwise null.</returns>
        public PropertyDef? Build(string name, Type type, Optionality optionality);
    }
}
