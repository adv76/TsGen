using TsGen.Interfaces;

namespace TsGen.Attributes
{
    /// <summary>
    /// This attribute specifies that a Typescript type should be generated for this class or struct.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class TsGenAttribute : Attribute
    {
        private readonly Type? _typeBuilderType = null;

        /// <summary>
        /// Whether a custom type builder was supplied for this type
        /// </summary>
        public bool HasCustomTypeBuilder => _typeBuilderType is not null;

        /// <summary>
        /// An instance of the type builder that was passed in for this type
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// If a custom type builder was not supplied
        /// </exception>
        public ITypeBuilder TypeBuilder
            => _typeBuilderType is not null && _typeBuilderType.IsAssignableTo(typeof(ITypeBuilder))
                ? (ITypeBuilder)Activator.CreateInstance(_typeBuilderType)!
                : throw new InvalidOperationException();
            
        /// <summary>
        /// Default constructor for the attribute
        /// </summary>
        public TsGenAttribute() { }

        /// <summary>
        /// Alternate constructor that allows specifying the type builder
        /// </summary>
        /// <param name="builderType">The type builder to use to generate the TS Type.</param>
        public TsGenAttribute(Type? builderType = null) 
        { 
            _typeBuilderType = builderType;
        }
    }
}
