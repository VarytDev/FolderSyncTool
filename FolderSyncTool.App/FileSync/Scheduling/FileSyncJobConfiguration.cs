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

            var trigger = TriggerBuilder.Create()
                .ForJob(jobDetail)
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(config.SyncInterval).RepeatForever())
                .Build();

            await scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}
