namespace TsGen.Models
{
    /// <summary>
    /// Represents a fully functional Typescript type
    /// </summary>
    public class TypeDefinition
    {
        /// <summary>
        /// The type that this type definition is for
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// The text of the type file
        /// </summary>
        public string TypeText { get; set; } = string.Empty;

        /// <summary>
        /// All of the types that this type is dependant on (and thus is importing)
        /// </summary>
        public List<Type> DependentTypes { get; set; }

        /// <summary>
        /// Creates a new type definition for the supplied type
        /// </summary>
        /// <param name="type">The type to define</param>
        public TypeDefinition(Type type) 
        { 
            Type = type; 
            DependentTypes = new List<Type>();
        }

        /// <summary>
        /// Creates a new type definition for the supplied type
        /// </summary>
        /// <param name="type">The type to define</param>
        /// <param name="typeText">The text of the type file</param>
        public TypeDefinition(Type type, string typeText)
        {
            Type = type;
            TypeText = typeText;
            DependentTypes = new List<Type>();
        }

        /// <summary>
        /// Creates a new type definition for the supplied type
        /// </summary>
        /// <param name="type">The type to define</param>
        /// <param name="typeText">The text of the type file</param>
        /// <param name="dependentTypes">The types this type depends on</param>
        public TypeDefinition(Type type, string typeText, List<Type> dependentTypes)
        {
            Type = type;
            TypeText = typeText;
            DependentTypes = dependentTypes;
        }

    }
}
