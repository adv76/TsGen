using TsGen.Enums;
using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen.TypeResolvers
{
    /// <summary>
    /// Resolves the Typescript type of various built-in property types
    /// </summary>
    /// <remarks>
    /// The types this resolver handles are as follows:
    /// 
    /// <list type="bullet">
    /// <item>
    /// <term><see cref="bool"/> maps to TS boolean.</term>     
    /// </item>
    /// <item>
    /// <term><see cref="byte"/>, <see cref="sbyte"/>, <see cref="decimal"/>, <see cref="double"/>, <see cref="float"/>,
    /// <see cref="int"/>, <see cref="uint"/>, <see cref="nint"/>, <see cref="nuint"/>, <see cref="long"/>,
    /// <see cref="ulong"/>, <see cref="short"/>, and <see cref="ushort"/> map to TS number.</term>
    /// </item>
    /// <item>
    /// <term><see cref="char"/>, <see cref="string"/>, and <see cref="Guid"/> map to TS string.</term>>
    /// </item>
    /// <item>
    /// <term><see cref="object"/> maps to TS any.</term>
    /// </item>
    /// </list>
    /// </remarks>
    public class BuiltInTypeResolver : IPropertyTypeResolver
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
            { typeof(string), "string" },
            { typeof(Guid), "string" },
            { typeof(object), "any" }
        };

        /// <summary>
        /// Attempts to resovlve a Typescript type from the passed in type
        /// </summary>
        /// <param name="type">The type to attempt to resolve.</param>
        /// <param name="optional">Whether or not the resolved type should be optional.</param>
        /// <param name="recursiveResolver">The recursive resolver for resolving nested types.</param>
        /// <returns>A resolved type if the type can be handled by this resolver (see list in class description) otherwise null.</returns>
        public PropertyType? Resolve(Type type, Optionality optionality, IPropertyTypeResolver recursiveResolver)
        {
            if (_defaults.TryGetValue(type, out var value))
            {
                return new PropertyType(optionality, value);
            }

            return null;
        }
    }
}
