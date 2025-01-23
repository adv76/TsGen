using System.Text.Json;
using TsGen.Builders.TypeBuilders;
using TsGen.Interfaces;

namespace TsGen
{
    public class TsGenSettings
    {
        public virtual JsonNamingPolicy PropertyNamingPolicy { get; set; } = JsonNamingPolicy.CamelCase;

        public virtual ITypeBuilder DefaultTypeBuilder { get; set; } = new TypeBuilder();

        public virtual string OutputDirectory { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "TsGen");
    }
}
