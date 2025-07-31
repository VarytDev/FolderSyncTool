namespace FolderSyncTool.App.FileSync.Service
{
    public interface IFileSyncService
    {
        public void Sync(string sourcePath, string targetPath);
    }
}
