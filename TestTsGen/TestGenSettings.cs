using System.Text.Json;
using TsGen;
using TsGen.Builders.TypeBuilders;
using TsGen.Interfaces;

namespace TestTsGen
{
    internal class TestGenSettings : GeneratorSettingsBase
    {
        public override JsonNamingPolicy PropertyNamingPolicy => JsonNamingPolicy.CamelCase;

        public override ITypeBuilder DefaultTypeBuilder => new TypeBuilder();

        public override string OutputDirectory => @"C:\Users\adv\Documents\types";
    }
}
