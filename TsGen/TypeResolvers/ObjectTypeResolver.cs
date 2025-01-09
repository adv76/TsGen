using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen.TypeResolvers
{
    public class ObjectTypeResolver : ITypeResolver
    {
        public ResolvedType Resolve(Type type, bool optional, ITypeResolver recursiveResolver)
        {
            return new ResolvedType(optional, type.Name, [type]);
        }
    }
}
