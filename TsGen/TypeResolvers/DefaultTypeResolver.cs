using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen.TypeResolvers
{
    public class DefaultTypeResolver : ITypeResolver
    {
        private readonly BuiltInTypeResolver _builtInResolver = new();
        private readonly CollectionTypeResolver _collectionResolver = new();
        private readonly DateTimeTypeResolver _dateTimeResolver = new();
        private readonly ObjectTypeResolver _objectResolver = new();

        public ResolvedType? Resolve(Type type, bool optional, ITypeResolver recursiveResolver)
            => _builtInResolver.Resolve(type, optional, recursiveResolver)
                ?? _collectionResolver.Resolve(type, optional, recursiveResolver)
                ?? _dateTimeResolver.Resolve(type, optional, recursiveResolver)
                ?? _objectResolver.Resolve(type, optional, recursiveResolver);
            
    }
}
