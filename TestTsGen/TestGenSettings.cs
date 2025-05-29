using Random;
using TsGen;
using TsGen.Enums;

namespace TestTsGen
{
    internal class TestGenSettings : TsGenSettings
    {
        public override string[] OutputDirectories { get; set; } = [@"C:\Users\adv\Documents\types"];
        public override ExportStructure ExportStructure { get; set; } = ExportStructure.FileBased;
        public override Type[] AdditionalTypes { get; } = { typeof(NotAttributedOrReferencedRecord) };
    }
}
