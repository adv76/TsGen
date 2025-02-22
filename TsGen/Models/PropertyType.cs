using TsGen.Enums;

namespace TsGen.Models
{
    public class PropertyType
    {
        public Optionality Optionality { get; set; } = Optionality.Default;
        public string TypeName { get; set; } = string.Empty;
        public List<Type> DependentTypes { get; set; }

        public PropertyType() 
        { 
            DependentTypes = new List<Type>();
        }

        public PropertyType(Optionality optionality, string typeName)
        {
            Optionality = optionality;
            TypeName = typeName;
            DependentTypes = new List<Type>();
        }

        public PropertyType(Optionality optionality, string typeName, params Type[] dependentType)
        {
            Optionality = optionality;
            TypeName = typeName;

            DependentTypes = new List<Type>();
            DependentTypes.AddRange(dependentType);
        }

        public PropertyType(Optionality optionality, string typeName, List<Type> dependentTypes)
        {
            Optionality = optionality;
            TypeName = typeName;
            DependentTypes = dependentTypes;
        }
    }
}
