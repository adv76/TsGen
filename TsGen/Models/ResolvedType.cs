namespace TsGen.Models
{
    public class ResolvedType
    {
        public bool Optional { get; set; } = false;
        public string TypeName { get; set; } = string.Empty;
        public List<Type> DependentTypes { get; set; }

        public ResolvedType() 
        { 
            DependentTypes = new List<Type>();
        }

        public ResolvedType(bool optional, string typeName)
        {
            Optional = optional;
            TypeName = typeName;
            DependentTypes = new List<Type>();
        }

        public ResolvedType(bool optional, string typeName, params Type[] dependentType)
        {
            Optional = optional;
            TypeName = typeName;

            DependentTypes = new List<Type>();
            DependentTypes.AddRange(dependentType);
        }

        public ResolvedType(bool optional, string typeName, List<Type> dependentTypes)
        {
            Optional = optional;
            TypeName = typeName;
            DependentTypes = dependentTypes;
        }
    }
}
