namespace TsGen.Extensions
{
    public static class StringExtensions
    {
        public static string Sanitize(this string s)
            => s.Replace('`', '_');
    }
}
