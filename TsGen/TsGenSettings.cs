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
        /// The directories to output to. It defaults to a single directory called "TsGen" in the users Documents folder (<see cref="Environment.SpecialFolder.Personal"/>).
        /// </summary>
        public virtual string[] OutputDirectories { get; set; } = new string[] { Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "TsGen") };

        /// <summary>
        /// Whether or not to clear the output directory before outputting the type files. Defaults to true.
        /// </summary>
        public virtual bool ClearTargetDirectory { get; set; } = true;

        /// <summary>
        /// The character(s) to use for an indentation. It defaults to 4 spaces.
        /// </summary>
        public virtual string Indentation { get; set; } = new string(' ', 4);

        /// <summary>
        /// The export structure (file structure of how the types are output). It defaults to <see cref="ExportStructure.DirectoryBased"/>
        /// </summary>
        public virtual ExportStructure ExportStructure { get; set; } = ExportStructure.DirectoryBased;

        /// <summary>
        /// Additional types that you want to manually include in the type files
        /// </summary>
        /// <remarks>
        /// This is useful if you want to include a type from a library that you don't have control over.
        /// </remarks>
        public virtual Type[] AdditionalTypes { get; } = Array.Empty<Type>();
    }
}
