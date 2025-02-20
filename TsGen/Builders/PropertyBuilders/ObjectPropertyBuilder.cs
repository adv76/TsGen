using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen.Builders.PropertyBuilders
{
    public class ObjectPropertyBuilder : IPropertyBuilder
    {
        public PropertyDef Build(string name, Type type, bool optional)
        {
            return new PropertyDef(name, optional, type.Name, type);
        }
    }
}
