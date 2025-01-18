using System.Collections;
using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen.Builders.PropertyBuilders
{
    public class CollectionPropertyBuilder : IPropertyBuilder
    {
        public bool HandlesType(Type type) => false; // TODO this needs fixing for nested collections

        public PropertyDef? Build(string name, Type type, bool optional)
        {
            var interfaces = type.GetInterfaces();
            if (interfaces.Length == 0) return null;

            var genericDictionary = interfaces.FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>));
            if (genericDictionary is not null)
            {
                var genericParamTypes = genericDictionary.GetGenericArguments();

                return new PropertyDef(name, optional, $"Record<{genericParamTypes[0].Name}, {genericParamTypes[1].Name}>", genericParamTypes[0], genericParamTypes[1]);
            }

            var dictionary = interfaces.FirstOrDefault(i => i == typeof(IDictionary));
            if (dictionary is not null)
            {
                return new PropertyDef(name, optional, "Record<any, any>");
            }

            var genericEnumerable = interfaces.FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            if (genericEnumerable is not null)
            {
                var genericParamTypes = genericEnumerable.GetGenericArguments();

                return new PropertyDef(name, optional, genericParamTypes[0].Name, genericParamTypes[0]);
            }

            var enumerable = interfaces.FirstOrDefault(i => i == typeof(IEnumerable));
            if (enumerable is not null)
            {
                return new PropertyDef(name, optional, "any[]");
            }

            return null;
        }
    }
}
