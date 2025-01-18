namespace TsGen.Models
{
    public class TypeDef
    {
        public Type Type { get; set; }
        public string TypeText { get; set; } = string.Empty;
        public List<Type> DependentTypes { get; set; }

        public TypeDef(Type type) 
        { 
            Type = type; 
            DependentTypes = new List<Type>();
        }

        public TypeDef(Type type, string typeText, List<Type> dependentTypes)
        {
            Type = type;
            TypeText = typeText;
            DependentTypes = dependentTypes;
        }

    }
}
