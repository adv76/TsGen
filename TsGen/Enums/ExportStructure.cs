namespace TsGen.Enums
{
    /// <summary>
    /// The file structure of the type exports
    /// </summary>
    public enum ExportStructure
    {
        /// <summary>
        /// Outputs the files in a directory hierarchy based on namespaces
        /// </summary>
        DirectoryBased = 0,

        /// <summary>
        /// Outputs the files with a file per namespace
        /// </summary>
        FileBased = 1
    }
}
