namespace SonarRestService
{
    /// <summary>
    /// IRestLogger logger
    /// </summary>
    public interface IRestLogger
    {
        /// <summary>
        /// Report message
        /// </summary>
        /// <param name="message">message</param>
        void ReportMessage(string message);
    }
}