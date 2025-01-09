using TsGen.Models;

namespace TsGen.Interfaces
{
    public interface ITypeBuilder
    {
        public TypeDef Build(Type type, bool export);
    }
}
