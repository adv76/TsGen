using System.Text;

namespace TsGen.Models
{
    /// <summary>
    /// Represents a complete TypeScript file that is ready to be output.
    /// </summary>
    public class TypeFile
    {
        /// <summary>
        /// The namespace that all of the types are in (types are organized one namespace per file)
        /// </summary>
        public string Namespace { get; set; } = string.Empty;

        /// <summary>
        /// The relative path to this type file (relative to the base directory)
        /// </summary>
        public string RelativeFilePath { get; set; } = string.Empty;

        /// <summary>
        /// All of the imports to this file. Imports must be complete e.g. "import { item } from './my-module';"
        /// </summary>
        public List<string> Imports { get; set; } = new();

        /// <summary>
        /// Dictionary of the types in this namespace/file and the generated output text for the type
        /// </summary>
        public Dictionary<Type, string> TypeMap { get; set; } = new();

        /// <summary>
        /// Builds a the text of a type file from the class properties
        /// </summary>
        /// <param name="basePath">The base output directory to output all types to</param>
        /// <param name="fileContents">(Out) the contents of the type file</param>
        /// <returns>The complete file path of where to save the type file</returns>
        public string ToFile(string basePath, out string fileContents)
        {
            var strBldr = new StringBuilder();

            strBldr.AppendLine("/*");
            strBldr.AppendLine(" * TsGen Auto-Generated Type File");
            strBldr.AppendLine(" *");
            strBldr.Append(" * Namespace: ");
            strBldr.AppendLine(string.IsNullOrEmpty(Namespace) ? "<EMPTY>" : Namespace);
            strBldr.AppendLine(" *");
            strBldr.AppendLine(" * DO NOT MODIFY THIS FILE. ALL CHANGES WILL BE OVERWRITTEN ON REGEN.");
            strBldr.AppendLine(" */");
            strBldr.AppendLine();

            foreach (var import in Imports)
            {
                strBldr.AppendLine(import);
            }

            if (Imports.Count > 0)
            {
                strBldr.AppendLine();
                strBldr.AppendLine();
            }

            foreach (var type in TypeMap)
            {
                strBldr.AppendLine($"// {type.Key.ToString()}");
                strBldr.AppendLine(type.Value);
            }

            fileContents = strBldr.ToString();

            return ReplacePathSeparators(Path.Combine(basePath, RelativeFilePath));
        }

        private static string ReplacePathSeparators(string path) 
            => Path.DirectorySeparatorChar switch
                {
                    '\\' => path.Replace('/', '\\'),
                    '/' => path.Replace('\\', '/'),
                    _ => path
                };
    }
}
