using TsGen.Extensions;
using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen.TypeResolvers
{
    public class GenericTypeResolver : ITypeResolver
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
            if (!type.IsGenericType) return null;

            var genericParamTypes = type.GetGenericArguments();

            var genericTypeNames = new List<string>();
            var deptTypes = new List<Type>();

            foreach(var genericParamType in genericParamTypes)
            {
                var resolvedType = recursiveResolver.Resolve(genericParamType, false, recursiveResolver);
                if (resolvedType is not null)
                {
                    deptTypes.AddRange(resolvedType.DependentTypes);

                    genericTypeNames.Add(resolvedType.TypeName);
                }
                else
                {
                    genericTypeNames.Add("any");
                }
            }

            deptTypes.Add(type.GetGenericTypeDefinition());

            return new ResolvedType(optional, $"{type.Name.Sanitize()}<{string.Join(", ", genericTypeNames)}>", deptTypes);
        }
    }
}
