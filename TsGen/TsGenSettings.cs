using TsGen.Builders.TypeBuilders;
using TsGen.Enums;
using TsGen.Interfaces;
using TsGen.NamingPolicies;

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
        /// The default naming policy for types. It defaults to PascalCase.
        /// </summary>
        public virtual INamingPolicy TypeNamingPolicy { get; set; } = new PascalCaseNamingPolicy();

        /// <summary>
        /// The default naming policy for properties. It defaults to camelCase.
        /// </summary>
        public virtual INamingPolicy PropertyNamingPolicy { get; set; } = new CamelCaseNamingPolicy();

        /// <summary>
        /// The default enum builder. It defaults to <see cref="EnumBuilder">Enum Builder</see>.
        /// </summary>
        public virtual ITypeBuilder DefaultEnumBuilder { get; set; } = new EnumBuilder();

        /// <summary>
        /// The default type builder. It defaults to <see cref="TypeBuilder">Type Builder</see>.
        /// </summary>
        public virtual ITypeBuilder DefaultTypeBuilder { get; set; } = new TypeBuilder();

        /// <summary>
        /// The default optionality setting for properties. It defaults to <see cref="Optionality.Required"/>
        /// </summary>
        public virtual Optionality DefaultPropertyOptionality { get; set; } = Optionality.Required;

        /// <summary>
        /// The default optionality setting for nullable properties. It defaults to <see cref="Optionality.Optional"/>
        /// </summary>
        public virtual Optionality DefaultNullablePropertyOptionality { get; set; } = Optionality.Optional;

        /// <summary>
        /// The default output directory. It defaults to a directory called "TsGen" in the users Documents folder (<see cref="Environment.SpecialFolder.Personal"/>).
        /// </summary>
        public virtual string OutputDirectory { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "TsGen");

        /// <summary>
        /// The character(s) to use for an indentation. It defaults to 4 spaces.
        /// </summary>
        public virtual string Indentation { get; set; } = new string(' ', 4);
    }
}
