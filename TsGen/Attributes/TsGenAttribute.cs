using TsGen.Interfaces;

namespace TsGen.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class TsGenAttribute : Attribute
    {
        private readonly Type? _typeBuilderType = null;

        public bool HasCustomTypeBuilder => _typeBuilderType is not null;
        public ITypeBuilder TypeBuilder
            => _typeBuilderType is not null && _typeBuilderType.IsAssignableTo(typeof(ITypeBuilder))
                ? (ITypeBuilder)Activator.CreateInstance(_typeBuilderType)!
                : throw new InvalidOperationException();
            
        public TsGenAttribute() { }

        public TsGenAttribute(Type? builderType = null) 
        { 
            _typeBuilderType = builderType;
        }
    }
}
