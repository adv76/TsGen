namespace TsGen.Extensions
{
    public static class StringExtensions
    {
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
