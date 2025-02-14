using TsGen.Attributes;

namespace TestTsGenDotnet9
{
    [TsGen]
    public class Class1
    {
        public string String1 { get; set; } = string.Empty;
        public object Object1 { get; set; } = new();
    }
}
