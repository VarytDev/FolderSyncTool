using FolderSyncTool.App.Logger.Service;
using System.Security.Cryptography;

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

        private void ReplicateDirectory(string sourcePath, string targetPath)
        {
            if(!Directory.Exists(sourcePath))
            {
                throw new Exception("There is no source to copy!");
            }

            Directory.CreateDirectory(targetPath);

            ClaerTargetFiles(sourcePath, targetPath);
            ReplicateSourceFiles(sourcePath, targetPath);

            foreach(var sourceSubPath in Directory.GetDirectories(sourcePath))
            {
                string targetSubPath = Path.Combine(targetPath, Path.GetFileName(sourceSubPath));
                ReplicateDirectory(sourceSubPath, targetSubPath);
            }
        }

        private void ClaerTargetFiles(string sourcePath, string targetPath)
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

        private void ReplicateSourceFiles(string sourcePath, string targetPath)
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

        private static bool IsFileChanged(string sourceFile, string targetFile)
        {
            return !string.Equals(CheckMD5(sourceFile), CheckMD5(targetFile));
        }

        private static string CheckMD5(string filePath)
        {
            using(var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLower();
                }
            }
        }
    }
}
