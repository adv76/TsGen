namespace TsGen.Models
{
    public class TsType
    {
        public string TypeName { get; set; }
        public List<Type> DependantTypes { get; } = new();

        public TsType(string typeName)
        {
            TypeName = typeName;
        }
    }
}
