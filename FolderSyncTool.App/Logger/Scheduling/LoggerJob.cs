using FolderSyncTool.App.Logger.Service;
using Quartz;

namespace FolderSyncTool.App.Logger.Scheduling
{
    public class LoggerJob : IJob
    {
        private readonly ILoggerService _loggerService;

        public LoggerJob(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.MergedJobDataMap;

            string? logPath = dataMap.GetString(LoggerJobConfiguration.LogsPathKey);

            if (string.IsNullOrEmpty(logPath))
            {
                throw new Exception("Can't get required paths from data map!");
            }

            if (!Path.IsPathRooted(logPath))
            {
                throw new Exception("Invalid path!");
            }

            _loggerService.SaveLogFile(logPath);
            return Task.CompletedTask;
        }
    }
}
