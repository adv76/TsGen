using Microsoft.EntityFrameworkCore;
using Random.Records;
using System.Collections;
using System.Text.Json.Serialization;
using TsGen.Attributes;
using TsGen.Builders.TypeBuilders;

namespace TestTsGen.Level2
{
    [Index(nameof(Id))]
    [TsGen]
    internal class TestRecord1
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("DescriptionCustomName")]
        public string? Description { get; set; }
        public DateTime? Date { get; set; }
        [TsPropGen("any")]
        private int PrivateField { get; set; }
        [JsonIgnore]
        public int IgnoreThisProperty { get; set; }
    }

    [TsGen(typeof(TypeBuilder))]
    internal record TestRecord2(int Id, string? Text, string? Info, long Index, bool IsValid);

    [TsGen]
    internal record TestRecord3(List<int> List, HashSet<int> Set, Dictionary<int, int> Dict, IList<int> IList, IEnumerable<int> IEnumerable, IDictionary<int, int> IDict, IEnumerable IEnumerableNonGen);

    [TsGen]
    internal record TestRecord4(int Id, TestRecord5 Record5, Enum1 enum1);

    [TsGen]
    internal class TestClass1
    {
        public Guid Id { get; set; }
        public Guid Id2 { get; set; }
        public KeyValuePair<string, int> Kvp1 { get; set; }
        public KeyValuePair<string, double> Kvp2 { get; set; }
        public KeyValuePair<string, string> Kvp3 { get; set; }
    }

    internal enum Enum1
    {
        None,
        One
    }
}

namespace Random.Records 
{
    internal record TestRecord5(int Int1, string String1, float Float1);

    [TsGen]
    internal struct TestStruct1
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [TsGen]
    internal record TestRecord6(int[] Ints, TestRecord9 Record9);
}

namespace Random
{
    internal record TestRecord9(int Int1);
}
