using System.Collections;
using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen.TypeResolvers
{
    internal class CollectionTypeResolver : ITypeResolver
    {
        public ResolvedType? Resolve(Type type, bool optional, ITypeResolver recursiveResolver)
        {
            var interfaces = type.GetInterfaces();
            if (interfaces.Length == 0) return null;

            var genericDictionary = interfaces.FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>));
            if (genericDictionary is not null)
            {
                var genericParamTypes = genericDictionary.GetGenericArguments();

                var genericType0 = recursiveResolver.Resolve(genericParamTypes[0], false, recursiveResolver);
                var genericType1 = recursiveResolver.Resolve(genericParamTypes[1], false, recursiveResolver);

                return new ResolvedType(optional, $"Record<{genericType0?.TypeName ?? "any"}, {genericType1?.TypeName ?? "any"}>", [..(genericType0?.DependentTypes ?? []), ..(genericType1?.DependentTypes ?? [])]);
            }

            var dictionary = interfaces.FirstOrDefault(i => i == typeof(IDictionary));
            if (dictionary is not null)
            {
                return new ResolvedType(optional, "Record<any, any>");
            }

            var genericEnumerable = interfaces.FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            if (genericEnumerable is not null)
            {
                var genericParamTypes = genericEnumerable.GetGenericArguments();

                var genericType0 = recursiveResolver.Resolve(genericParamTypes[0], false, recursiveResolver);

                return new ResolvedType(optional, $"{genericType0?.TypeName ?? "any"}[]", genericType0?.DependentTypes ?? []);
            }

            var enumerable = interfaces.FirstOrDefault(i => i == typeof(IEnumerable));
            if (enumerable is not null)
            {
                return new ResolvedType(optional, "any[]");
            }

            return null;
        }
    }
}
