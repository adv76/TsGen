namespace TsGen.Interfaces
{
    /// <summary>
    /// Base interface for naming policies
    /// </summary>
    /// <remarks>
    /// The interface is inspired by System.Text.Json.JsonNamingPolicy
    /// </remarks>
    public interface INamingPolicy
    {
        /// <summary>
        /// Converts a name to meet the policy if it does not already
        /// </summary>
        /// <param name="name">The name to convert</param>
        /// <returns>Normalized name that meets the policy requirements</returns>
        public string ConvertName(string name);
    }
}
