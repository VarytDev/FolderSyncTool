using FolderSyncTool.App.Common.Utilities;
using FolderSyncTool.App.Logger.Service;

namespace FolderSyncTool.App.FileSync.Service
{
    public class FileSyncService : IFileSyncService
    {
        private readonly ILoggerService _loggerService;

        public FileSyncService(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        public void Sync(string sourcePath, string targetPath)
        {
            ReplicateDirectory(sourcePath, targetPath);
        }

        public void ReplicateDirectory(string sourcePath, string targetPath)
        {
            if(!Directory.Exists(sourcePath))
            {
                throw new Exception("There is no source to copy!");
            }

            Directory.CreateDirectory(targetPath);

            ClearTargetFiles(sourcePath, targetPath);
            ClearTargetDirectories(sourcePath, targetPath);

            ReplicateSourceFiles(sourcePath, targetPath);

            foreach(var sourceSubPath in Directory.GetDirectories(sourcePath))
            {
                string targetSubPath = Path.Combine(targetPath, Path.GetFileName(sourceSubPath));
                ReplicateDirectory(sourceSubPath, targetSubPath);
            }
        }

        public void ClearTargetDirectories(string sourcePath, string targetPath)
        {
            foreach(var targetSubPath in Directory.GetDirectories(targetPath))
            {
                var sourceSubPath = Path.Combine(sourcePath, Path.GetFileName(targetSubPath));

                if (Directory.Exists(sourceSubPath)) continue;

                ClearTargetFiles(sourceSubPath, targetSubPath);
                ClearTargetDirectories(sourceSubPath, targetSubPath);
                Directory.Delete(targetSubPath, false);
            }
        }

        public void ClearTargetFiles(string sourcePath, string targetPath)
        {
            foreach (var filePath in Directory.GetFiles(targetPath))
            {
                string fileName = Path.GetFileName(filePath);
                string sourceFilePath = Path.Combine(sourcePath, fileName);

                if (File.Exists(sourceFilePath)) continue;

                File.Delete(filePath);
                _loggerService.Log($"{fileName} removed from {targetPath}");
            }
        }

        public void ReplicateSourceFiles(string sourcePath, string targetPath)
        {
            foreach (var filePath in Directory.GetFiles(sourcePath))
            {
                string fileName = Path.GetFileName(filePath);
                string targetFilePath = Path.Combine(targetPath, fileName);

                if(!File.Exists(targetFilePath))
                {
                    File.Copy(filePath, targetFilePath);
                    _loggerService.Log($"{fileName} created in {targetPath}");
                    continue;
                }

                if (!IsFileChanged(filePath, targetFilePath)) continue;
                File.Copy(filePath, targetFilePath, true);
                _loggerService.Log($"{fileName} modified in {targetPath}");
            }
        }

        public static bool IsFileChanged(string sourceFile, string targetFile)
        {
            return !string.Equals(FileHashingUtility.CheckMD5(sourceFile), FileHashingUtility.CheckMD5(targetFile));
        }
    }
}
