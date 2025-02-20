using TsGen.Models;

namespace TsGen.Interfaces
{
    public interface IPropertyBuilder
    {
        public PropertyDef? Build(string name, Type type, bool optional);
    }
}
