namespace TsGen.Enums
{
    /// <summary>
    /// The optionality of a type property.
    /// </summary>
    public enum Optionality
    {
        /// <summary>
        /// The default optionality (derived from config).
        /// </summary>
        Default = 0,

        /// <summary>
        /// Not optional
        /// </summary>
        Required = 1,

        /// <summary>
        /// Optional (uses Typescript ? for optionality).
        /// </summary>
        Optional = 2,

        /// <summary>
        /// Type or undefined (not optional but can be undefined).
        /// </summary>
        TypeOrUndefined = 3
    }
}
