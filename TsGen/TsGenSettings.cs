using System.Text.Json;
using TsGen.Builders.TypeBuilders;
using TsGen.Interfaces;

namespace TsGen
{
    /// <summary>
    /// Generator settings for the Typescript Generator
    /// </summary>
    /// <remarks>
    /// All of the properties are virtual and have default values, so you only need to override
    /// the ones that you want to change.
    /// </remarks>
    public class TsGenSettings
    {
        /// <summary>
        /// The default naming policy for properties. It defaults to CamelCase.
        /// </summary>
        public virtual JsonNamingPolicy PropertyNamingPolicy { get; set; } = JsonNamingPolicy.CamelCase;

        /// <summary>
        /// The default type builder. It defaults to <see cref="TypeBuilder">Type Builder</see>.
        /// </summary>
        public virtual ITypeBuilder DefaultTypeBuilder { get; set; } = new TypeBuilder();

        /// <summary>
        /// The default output directory. It defaults to a directory called "TsGen" in the users Documents folder (<see cref="Environment.SpecialFolder.Personal"/>).
        /// </summary>
        public virtual string OutputDirectory { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "TsGen");
    }
}
