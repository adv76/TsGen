using TsGen.Models;

namespace TsGen.Interfaces
{
    public interface IPropertyBuilder
    {
        public bool HandlesType(Type type);
        public PropertyDef? Build(string name, Type type, bool optional);
    }
}
