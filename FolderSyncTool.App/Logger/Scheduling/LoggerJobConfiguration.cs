using Quartz;

namespace FolderSyncTool.App.Logger.Scheduling
{
    public static class LoggerJobConfiguration
    {
        public const string LogsPathKey = "LogPath";

        public static async Task AddLoggerJob(this IScheduler scheduler, string logPath)
        {
            var jobDetail = JobBuilder.Create<LoggerJob>()
                .WithIdentity(nameof(LoggerJob))
                .StoreDurably()
                .UsingJobData(LogsPathKey, logPath)
                .Build();

            await scheduler.AddJob(jobDetail, true);
        }
    }
}
