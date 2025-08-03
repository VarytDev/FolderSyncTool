using System.Text;

namespace FolderSyncTool.App.Logger.Service
{
    public class LoggerService : ILoggerService
    {
        public const string LogFileName = "log.txt";

        public StringBuilder StringBuilder { get; private set; } = new StringBuilder();

        public void Log(string message)
        {
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            string formattedMessage = $"{currentTime} - {message}";

            StringBuilder.AppendLine(formattedMessage);
            Console.WriteLine(formattedMessage);
        }

        public void SaveLogFile(string logFilePath)
        {
            string filePath = Path.Combine(logFilePath, LogFileName);

            Directory.CreateDirectory(logFilePath);

            using (StreamWriter streamWriter = new StreamWriter(filePath, true))
            {
                streamWriter.WriteLine(StringBuilder.ToString());
            }

            StringBuilder.Clear();
        }
    }
}
