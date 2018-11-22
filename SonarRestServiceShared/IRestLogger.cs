namespace SonarRestService
{
    /// <summary>
    /// IRestLogger logger
    /// </summary>
    public interface IRestLogger
    {
        void ReportMessage(string message);
    }
}