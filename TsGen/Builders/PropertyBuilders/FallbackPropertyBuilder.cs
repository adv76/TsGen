using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen.Builders.PropertyBuilders
{
    public class FallbackPropertyBuilder : IPropertyBuilder
    {
        public bool HandlesType(Type type) => false; // TODO should this be true?

        public PropertyDef Build(string name, Type type, bool optional)
        {
            return new PropertyDef(name, optional, "unknown");
        }
    }
}
