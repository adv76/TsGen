using System.Text.Json;
using TsGen.Builders.TypeBuilders;
using TsGen.Interfaces;

namespace TsGen
{
    public static class GeneratorSettings
    {
        public static JsonNamingPolicy PropertyNamingPolicy { get; internal set; } = JsonNamingPolicy.CamelCase;

        public static ITypeBuilder DefaultTypeBuilder { get; internal set; } = new TypeBuilder();

        public static string BaseDirectory { get; internal set; } = @"C:\Users\adv\Documents\test-ts";
    }
}
