using System.Text.Json;
using TsGen;
using TsGen.Builders.TypeBuilders;
using TsGen.Interfaces;

namespace TestTsGen
{
    internal class TestGenSettings : GeneratorSettingsBase
    {
        public override JsonNamingPolicy PropertyNamingPolicy { get; set; } = JsonNamingPolicy.CamelCase;

        public override ITypeBuilder DefaultTypeBuilder { get; set; } = new TypeBuilder();

        public override string OutputDirectory { get; set; } = @"C:\Users\adv\Documents\types";
    }
}
