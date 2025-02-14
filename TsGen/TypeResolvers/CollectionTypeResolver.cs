using System.Collections;
using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen.TypeResolvers
{
    public class CollectionTypeResolver : ITypeResolver
    {
        /// <summary>
        /// Attempts to resovlve a Typescript type from the passed in type
        /// </summary>
        /// <param name="type">The type to attempt to resolve.</param>
        /// <param name="optional">Whether or not the resolved type should be optional.</param>
        /// <param name="recursiveResolver">The recursive resolver for resolving nested types.</param>
        /// <returns>A resolved type if the type can be handled by this resolver (see list in class description) otherwise null.</returns>
        public ResolvedType? Resolve(Type type, bool optional, ITypeResolver recursiveResolver)
        {
            var interfaces = type.GetInterfaces();

            var genericDictionary = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>)
                ? type
                : interfaces.FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>));

            if (genericDictionary is not null)
            {
                var genericParamTypes = genericDictionary.GetGenericArguments();

                var deptTypes = new List<Type>();

                var genericType0 = recursiveResolver.Resolve(genericParamTypes[0], false, recursiveResolver);
                if (genericType0 is not null)
                {
                    deptTypes.AddRange(genericType0.DependentTypes);
                }

                var genericType1 = recursiveResolver.Resolve(genericParamTypes[1], false, recursiveResolver);
                if (genericType1 is not null)
                {
                    deptTypes.AddRange(genericType1.DependentTypes);
                }

                return new ResolvedType(optional, $"Record<{genericType0?.TypeName ?? "any"}, {genericType1?.TypeName ?? "any"}>", deptTypes);
            }

            var dictionary = type == typeof(IDictionary)
                ? type
                : interfaces.FirstOrDefault(i => i == typeof(IDictionary));

            if (dictionary is not null)
            {
                return new ResolvedType(optional, "Record<any, any>");
            }

            var genericEnumerable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                ? type 
                : interfaces.FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            
            if (genericEnumerable is not null)
            {
                var genericParamTypes = genericEnumerable.GetGenericArguments();

                var deptTypes = new List<Type>();

                var genericType0 = recursiveResolver.Resolve(genericParamTypes[0], false, recursiveResolver);
                if (genericType0 is not null)
                {
                    deptTypes.AddRange(genericType0.DependentTypes);
                }

                return new ResolvedType(optional, $"{genericType0?.TypeName ?? "any"}[]", deptTypes);
            }

            var enumerable = type == typeof(IEnumerable)
                ? type
                : interfaces.FirstOrDefault(i => i == typeof(IEnumerable));

            if (enumerable is not null)
            {
                return new ResolvedType(optional, "any[]");
            }

            return null;
        }
    }
}
