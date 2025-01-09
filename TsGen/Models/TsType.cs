namespace TsGen.Models
{
    public class TsType(string typeName)
    {
        public string TypeName { get; set; } = typeName;
        public List<Type> DependantTypes { get; } = [];
    }
}
