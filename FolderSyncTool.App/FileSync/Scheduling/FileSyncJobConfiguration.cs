using FolderSyncTool.App.Common.Data;
using Quartz;

namespace FolderSyncTool.App.FileSync.Scheduling
{
    public static class FileSyncJobConfiguration
    {
        public const string SourcePathKey = "SourcePath";
        public const string ReplicaPathKey = "ReplicaPath";

        public static async Task ScheduleFileSyncJob(this IScheduler scheduler, Config config)
        {
            var jobDetail = JobBuilder.Create<FileSyncJob>()
                .WithIdentity(nameof(FileSyncJob))
                .UsingJobData(SourcePathKey, config.SourcePath)
                .UsingJobData(ReplicaPathKey, config.ReplicaPath)
                .Build();

            var triggerBuilder = TriggerBuilder.Create()
                .ForJob(jobDetail)
                .StartNow();

            if (config.SyncInterval > 0)
            {
                triggerBuilder = triggerBuilder.WithSimpleSchedule(x => x.WithIntervalInSeconds(config.SyncInterval).RepeatForever());
            }

            var trigger = triggerBuilder.Build();

            await scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}
