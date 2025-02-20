using System.Text.Json;
using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen.Builders.PropertyBuilders
{
    public class DefaultPropertyBuilder : IPropertyBuilder
    {
        private readonly BuiltInPropertyBuilder _builtInPropertyBuilder = new();
        private readonly CollectionPropertyBuilder _collectionPropertyBuilder = new();
        private readonly ObjectPropertyBuilder _objectPropertyBuilder = new();

        public PropertyDef? Build(string name, Type type, bool optional)
        {
            return _builtInPropertyBuilder.Build(name, type, optional)
                ?? _collectionPropertyBuilder.Build(name, type, optional)
                ?? _objectPropertyBuilder.Build(name, type, optional);
        }
    }
}
