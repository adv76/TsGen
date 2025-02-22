using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen.Zod
{
    public class ZodSchemaBuilder : ITypeBuilder
    {
        public TypeDef Build(Type type, bool export)
        {
            var generatorSettings = new TsGenSettings();



            return new TypeDef(type);
        }


    }
}
