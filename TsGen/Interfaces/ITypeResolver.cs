using TsGen.Models;

namespace TsGen.Interfaces
{
    public interface ITypeResolver
    {
        public ResolvedType? Resolve(Type type, bool optional, ITypeResolver recursiveResolver);
    }
}
