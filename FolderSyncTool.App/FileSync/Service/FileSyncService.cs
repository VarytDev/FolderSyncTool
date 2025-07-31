using System;
using System.IO;

namespace FolderSyncTool.App.FileSync.Service
{
    public class FileSyncService : IFileSyncService
    {
        private const string SourceFolderPath = "E:\\SyncTest\\Source";
        private const string ReplicaFolderPath = "E:\\SyncTest\\Replica";

        public void Sync()
        {
            CopyDirectory(SourceFolderPath, ReplicaFolderPath);
        }

        public static void CopyDirectory(string sourcePath, string targetPath, bool recursive = true)
        {
            if(string.IsNullOrEmpty(sourcePath) || string.IsNullOrEmpty(targetPath))
            {
                throw new Exception("Path can't be empty!");
            }

            if(!Directory.Exists(sourcePath))
            {
                throw new Exception("There is no source to copy!");
            }

            Directory.CreateDirectory(targetPath);

            foreach(var filePath in Directory.GetFiles(sourcePath))
            {
                string targetFilePath = Path.Combine(targetPath, Path.GetFileName(filePath));
                File.Copy(filePath, targetFilePath, true);
            }

            if (recursive == false) return;

            foreach(var sourceSubPath in Directory.GetDirectories(sourcePath))
            {
                string targetSubPath = Path.Combine(targetPath, Path.GetFileName(sourceSubPath));
                CopyDirectory(sourceSubPath, targetSubPath);
            }
        }
    }
}
