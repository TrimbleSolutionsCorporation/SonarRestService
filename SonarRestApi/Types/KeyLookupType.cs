namespace SonarRestService.Types
{
    /// <summary>
    /// Key lookup type
    /// </summary>
    public enum KeyLookupType
    {
        /// <summary>
        /// flat
        /// </summary>
        Flat = 0,
        /// <summary>
        /// project guid
        /// </summary>
        ProjectGuid = 1,
        /// <summary>
        /// module
        /// </summary>
        Module = 2,
        /// <summary>
        /// old vs boot strapper
        /// </summary>
        VSBootStrapper = 3,
        /// <summary>
        /// invalid
        /// </summary>
        Invalid = 4
    }
}