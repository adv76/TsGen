using System.Text;

namespace TsGen.Models
{
    public class TypeFile
    {
        public string Namespace { get; set; } = string.Empty;
        public string RelativePath { get; set; } = string.Empty;

        public List<string> Imports { get; set; } = [];
        public Dictionary<Type, string> TypeMap { get; set; } = [];

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
                strBldr.AppendLine($"// {type.Key.FullName}");
                strBldr.AppendLine(type.Value);
            }

            fileContents = strBldr.ToString();

            return ReplacePathSeparators(Path.Combine(basePath, RelativePath, "index.ts"));
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
