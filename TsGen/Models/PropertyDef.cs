namespace TsGen.Models
{
    public class PropertyDef
    {
        public string Name { get; set; } = string.Empty;
        public bool Optional { get; set; } = false;
        public string TypeName { get; set; } = string.Empty;
        public List<Type>? DependentTypes { get; set; } = null;

        public PropertyDef() { }

        public PropertyDef(string name, bool optional, string typeName, List<Type>? dependentTypes = null)
        {
            Name = name;
            Optional = optional;
            TypeName = typeName;
            DependentTypes = dependentTypes;
        }

        public PropertyDef(string name, ResolvedType type)
        {
            Name = name;
            Optional = type.Optional;
            TypeName = type.TypeName;
            DependentTypes = type.DependentTypes;
        }
    }
}
