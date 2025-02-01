using TsGen.Extensions;
using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen.TypeResolvers
{
    public class GenericTypeResolver : ITypeResolver
    {
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
