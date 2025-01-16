using System.Text.Json;
using TsGen.Builders.TypeBuilders;
using TsGen.Interfaces;

namespace TsGen
{
    public abstract class GeneratorSettingsBase
    {
        public abstract JsonNamingPolicy PropertyNamingPolicy { get; set; }

        public abstract ITypeBuilder DefaultTypeBuilder { get; set; }

        public abstract string OutputDirectory { get; set; }
    }

    public sealed class DefaultGeneratorSettings : GeneratorSettingsBase
    {
        public override JsonNamingPolicy PropertyNamingPolicy { get; set; } = JsonNamingPolicy.CamelCase;

        public override ITypeBuilder DefaultTypeBuilder { get; set; } = new TypeBuilder();

        public override string OutputDirectory { get; set; } = @"C:\Users\adv\Documents\test-ts";
    }
}
