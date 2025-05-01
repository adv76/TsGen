using TsGen;
using TsGen.Enums;

namespace TestTsGen
{
    internal class TestGenSettings : TsGenSettings
    {
        public override string OutputDirectory { get; set; } = @"C:\Users\adv\Documents\types";
        public override ExportStructure ExportStructure { get; set; } = ExportStructure.FileBased;
    }
}
