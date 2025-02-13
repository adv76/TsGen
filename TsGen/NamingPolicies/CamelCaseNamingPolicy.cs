﻿using TsGen.Interfaces;

namespace TsGen.NamingPolicies
{
    /// <summary>
    /// Naming policy that outputs camelCase names
    /// </summary>
    /// <remarks>
    /// The implementation is derived from System.Text.Json.JsonCamelCaseNamingPolicy
    /// </remarks>
    public class CamelCaseNamingPolicy : INamingPolicy
    {
        /// <summary>
        /// Converts a name to camelCase
        /// </summary>
        /// <param name="name">The name to convert</param>
        /// <returns>The name in camelCase</returns>
        public string ConvertName(string name)
        {
            if (string.IsNullOrEmpty(name) || !char.IsUpper(name[0]))
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
            for (int i = 0; i < chars.Length; i++)
            {
                if (i == 1 && !char.IsUpper(chars[i]))
                {
                    break;
                }

                bool hasNext = (i + 1 < chars.Length);

                // Stop when next char is already lowercase.
                if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
                {
                    // If the next char is a space, lowercase current char before exiting.
                    if (chars[i + 1] == ' ')
                    {
                        chars[i] = char.ToLowerInvariant(chars[i]);
                    }

                    break;
                }

                chars[i] = char.ToLowerInvariant(chars[i]);
            }
        }
    }
}
