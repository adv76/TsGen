using Microsoft.EntityFrameworkCore;
using Random.Records;
using System.Text.Json.Serialization;
using TsGen.Attributes;
using TsGen.Builders.TypeBuilders;

namespace TestTsGen
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
    }

    [TsGen(typeof(TypeBuilder))]
    internal record TestRecord2(int Id, string? Text, string? Info, long Index, bool IsValid);

    [TsGen]
    internal record TestRecord3(List<int> List, HashSet<int> Set, Dictionary<int, int> Dict);

    [TsGen]
    internal record TestRecord4(int Id, TestRecord5 Record5);

    

    
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
    internal record TestRecord6(int[] Ints);
}
