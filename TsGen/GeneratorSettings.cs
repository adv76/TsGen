using System.Text.Json;
using TsGen.Builders.TypeBuilders;
using TsGen.Interfaces;

namespace TsGen
{
    public abstract class GeneratorSettingsBase
    {
        public abstract JsonNamingPolicy PropertyNamingPolicy { get; }

        public abstract ITypeBuilder DefaultTypeBuilder { get; }

        public abstract string OutputDirectory { get; }
    }

    public sealed class DefaultGeneratorSettings : GeneratorSettingsBase
    {
        public override JsonNamingPolicy PropertyNamingPolicy { get; } = JsonNamingPolicy.CamelCase;

        public override ITypeBuilder DefaultTypeBuilder { get; } = new TypeBuilder();

        public override string OutputDirectory { get; } = @"C:\Users\adv\Documents\test-ts";
    }
}
