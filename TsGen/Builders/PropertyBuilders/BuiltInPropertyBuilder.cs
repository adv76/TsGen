using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen.Builders.PropertyBuilders
{
    public class BuiltInPropertyBuilder : IPropertyBuilder
    {
        private static readonly Dictionary<Type, string> _defaults = new()
        {
            { typeof(bool), "boolean" },
            { typeof(byte), "number" },
            { typeof(sbyte), "number" },
            { typeof(char), "string" },
            { typeof(decimal), "number" },
            { typeof(double), "number" },
            { typeof(float), "number" },
            { typeof(int), "number" },
            { typeof(uint), "number" },
            { typeof(nint), "number" },
            { typeof(nuint), "number" },
            { typeof(long), "number" },
            { typeof(ulong), "number" },
            { typeof(short), "number" },
            { typeof(ushort), "number" },
            { typeof(string), "string" }
        };

        public bool HandlesType(Type type) => _defaults.ContainsKey(type);

        public PropertyDef? Build(string name, Type type, bool optional)
        {
            if (_defaults.TryGetValue(type, out var value))
            {
                return new PropertyDef(name, optional, value);
            }

            return null;
        }
    }
}
