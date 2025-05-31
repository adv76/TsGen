using TsGen.Enums;
using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen.TypeResolvers
{
    /// <summary>
    /// The default property type resolver
    /// </summary>
    /// <remarks>
    /// This type resolver combines some of the built-in resolvers to build a sensible default resolver that should handle most scenarios.
    /// 
    /// The order of resolution is:
    /// <list type="number">
    /// <item>
    /// <term><see cref="OverridesTypeResolver"/> which handles all overrides specified in <see cref="TsGenSettings.ManualOverrides"/></term>
    /// </item>
    /// <item>
    /// <term><see cref="BuiltInTypeResolver"/> which handles primitives and a few basic built-in types.</term>
    /// </item>
    /// <item>
    /// <term><see cref="CollectionTypeResolver"/> which handles most built-in collections (including generics) and any collection that implements IEnumerable.</term>
    /// </item>
    /// <item>
    /// <term><see cref="DateTimeTypeResolver"/> which handles the various date and time types.</term>
    /// </item>
    /// <item>
    /// <term><see cref="GenericTypeResolver"/> which handles any generic types not already handled in the collection resolver.</term>
    /// </item>
    /// <item>
    /// <term><see cref="ObjectTypeResolver"/> which resolves any remaining types as the name of the type and flags that type for generation also.</term>
    /// </item>
    /// </list>
    /// </remarks>
    public class DefaultTypeResolver : IPropertyTypeResolver
    {
        private readonly OverridesTypeResolver _overridesResolver = new();
        private readonly BuiltInTypeResolver _builtInResolver = new();
        private readonly CollectionTypeResolver _collectionResolver = new();
        private readonly GenericTypeResolver _genericResolver = new();
        private readonly DateTimeTypeResolver _dateTimeResolver = new();
        private readonly ObjectTypeResolver _objectResolver = new();

        /// <summary>
        /// Attempts to resovlve a Typescript type from the passed in type
        /// </summary>
        /// <param name="type">The type to attempt to resolve.</param>
        /// <param name="optionality">Whether or not the resolved type should be optional.</param>
        /// <param name="recursiveResolver">The recursive resolver for resolving nested types.</param>
        /// <param name="generatorSettings">The generator settings to use</param>
        /// <returns>A resolved type if the type can be handled by this resolver (see list in class description) otherwise null.</returns>
        public PropertyType? Resolve(Type type, Optionality optionality, IPropertyTypeResolver recursiveResolver, TsGenSettings generatorSettings)
            => _overridesResolver.Resolve(type, optionality, recursiveResolver, generatorSettings)
                ?? _builtInResolver.Resolve(type, optionality, recursiveResolver, generatorSettings)
                ?? _collectionResolver.Resolve(type, optionality, recursiveResolver, generatorSettings)
                ?? _dateTimeResolver.Resolve(type, optionality, recursiveResolver, generatorSettings)
                ?? _genericResolver.Resolve(type, optionality, recursiveResolver, generatorSettings)
                ?? _objectResolver.Resolve(type, optionality, recursiveResolver, generatorSettings);
    }
}
