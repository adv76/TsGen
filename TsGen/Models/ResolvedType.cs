namespace TsGen.Models
{
    public class ResolvedType
    {
        public bool Optional { get; set; } = false;
        public string TypeName { get; set; } = string.Empty;
        public List<Type> DependentTypes { get; set; } = [];

        public ResolvedType() { }

        public ResolvedType(bool optional, string typeName, List<Type>? dependentTypes = null)
        {
            Optional = optional;
            TypeName = typeName;

            if (dependentTypes is not null)
            {
                DependentTypes = dependentTypes;
            }
        }
    }
}
