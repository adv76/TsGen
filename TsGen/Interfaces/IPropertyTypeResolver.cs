using TsGen.Enums;
using TsGen.Models;

namespace TsGen.Interfaces
{
    public interface IPropertyTypeResolver
    {
        public PropertyType? Resolve(Type type, Optionality optionality, IPropertyTypeResolver recursiveResolver, TsGenSettings generatorSettings);
    }
}
