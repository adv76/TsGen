using TsGen.Models;

namespace TsGen.Interfaces
{
    /// <summary>
    /// The interface for type builders. All internal and custom type builders must implement it.
    /// </summary>
    public interface ITypeBuilder
    {
        /// <summary>
        /// Builds a type def from a .NET type
        /// </summary>
        /// <param name="type">The type to build a type def for.</param>
        /// <param name="export">Whether or not to export the type.</param>
        /// <param name="generatorSettings">The settings to use to build the type.</param>
        /// <returns></returns>
        public TypeDef Build(Type type, bool export, TsGenSettings generatorSettings);
    }
}
