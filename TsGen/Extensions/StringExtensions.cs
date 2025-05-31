namespace TsGen.Extensions
{
    /// <summary>
    /// Helper methods for strings
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Sanitizes strings for unparsable characters
        /// </summary>
        /// <remarks>
        /// Currently the only thing this method does is removes any characters after a backtick (as well as the
        /// backtick). This is due to the fact that backticks are invalid identifier names but are found in the
        /// type names of generic types
        /// </remarks>
        /// <param name="s">The string to sanitize</param>
        /// <returns>A sanitized string</returns>
        public static string Sanitize(this string s)
        {
            var backtickIndex = s.IndexOf('`');
            if (backtickIndex != -1)
            {
                return s[0..backtickIndex];
            }
            else
            {
                return s;
            }
        }   
    }
}
