using FolderSyncTool.App.FileSync.Service;
using Quartz;

namespace FolderSyncTool.App.FileSync.Scheduling
{
    public class FileSyncJob : IJob
    {
        private readonly IFileSyncService _fileSyncService;

        public FileSyncJob(IFileSyncService fileSyncService)
        {
            _fileSyncService = fileSyncService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.MergedJobDataMap;

            string? sourcePath = dataMap.GetString(FileSyncJobConfiguration.SourcePathKey);
            string? replicaPath = dataMap.GetString(FileSyncJobConfiguration.ReplicaPathKey);

            if(string.IsNullOrEmpty(sourcePath) || string.IsNullOrEmpty(replicaPath))
            {
                throw new Exception("Can't get required paths from data map!");
            }

            _fileSyncService.Sync(sourcePath, replicaPath);
            return Task.CompletedTask;
        }
    }
}
