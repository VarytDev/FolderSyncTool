namespace FolderSyncTool.App.Logger.Service
{
    public interface ILoggerService
    {
        public void Log(string message);
        public void SaveLogFile(string logFilePath);
    }
}
