namespace TsGen.Enums
{
    /// <summary>
    /// The optionality of a type property.
    /// </summary>
    public enum Optionality
    {
        /// <summary>
        /// The default optionality (not optional).
        /// </summary>
        Default = 0,

        /// <summary>
        /// Optional (uses Typescript ? for optionality).
        /// </summary>
        Optional = 1,

        /// <summary>
        /// Type or undefined (not optional but can be undefined).
        /// </summary>
        TypeOrUndefined = 2
    }
}
