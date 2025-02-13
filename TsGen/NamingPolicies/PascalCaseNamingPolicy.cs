using TsGen.Interfaces;

namespace TsGen.NamingPolicies
{
    /// <summary>
    /// Naming policy that outputs PascalCase names
    /// </summary>
    /// <remarks>
    /// The implementation is derived from System.Text.Json.JsonCamelCaseNamingPolicy
    /// </remarks>
    public class PascalCaseNamingPolicy : INamingPolicy
    {
        /// <summary>
        /// Converts a name to PascalCase
        /// </summary>
        /// <param name="name">The name to convert</param>
        /// <returns>The name in PascalCase</returns>
        public string ConvertName(string name)
        {
            if (string.IsNullOrEmpty(name) || !char.IsLower(name[0]))
            {
                return name;
            }

            return string.Create(name.Length, name, (chars, name) =>
            {
                name.CopyTo(chars);
                FixCasing(chars);
            });
        }

        private static void FixCasing(Span<char> chars)
        {
            chars[0] = char.ToUpperInvariant(chars[0]);
        }
    }
}
