using TsGen.Enums;
using TsGen.Extensions;
using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen.TypeResolvers
{
    /// <summary>
    /// Type resolver for objects with generic parameters
    /// </summary>
    public class GenericTypeResolver : IPropertyTypeResolver
    {
        /// <summary>
        /// Attempts to resovlve a Typescript type from the passed in type
        /// </summary>
        /// <param name="type">The type to attempt to resolve.</param>
        /// <param name="optionality">Whether or not the resolved type should be optional.</param>
        /// <param name="recursiveResolver">The recursive resolver for resolving nested types.</param>
        /// <param name="generatorSettings">The generator settings to use</param>
        /// <returns>A resolved type if the type can be handled by this resolver (see list in class description) otherwise null.</returns>
        public PropertyType? Resolve(Type type, Optionality optionality, IPropertyTypeResolver recursiveResolver, TsGenSettings generatorSettings)
        {
            if (!type.IsGenericType) return null;

            var genericParamTypes = type.GetGenericArguments();

            var genericTypeNames = new List<string>();
            var deptTypes = new List<Type>();

            foreach(var genericParamType in genericParamTypes)
            {
                var resolvedType = recursiveResolver.Resolve(genericParamType, Optionality.Required, recursiveResolver, generatorSettings);
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

            return new PropertyType(optionality, $"{type.Name.Sanitize()}<{string.Join(", ", genericTypeNames)}>", deptTypes);
        }
    }
}
