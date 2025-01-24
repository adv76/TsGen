using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen.TypeResolvers
{
    public class BuiltInTypeResolver : ITypeResolver
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
            { typeof(Guid), "string" }
        };

        public ResolvedType? Resolve(Type type, bool optional, ITypeResolver recursiveResolver)
        {
            if (_defaults.TryGetValue(type, out var value))
            {
                return new ResolvedType(optional, value);
            }

            return null;
        }
    }
}
