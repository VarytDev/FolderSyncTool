using System.Security.Cryptography;

namespace FolderSyncTool.App.FileSync.Service
{
    public class FileSyncService : IFileSyncService
    {
        public void Sync(string sourcePath, string targetPath)
        {
            ReplicateDirectory(sourcePath, targetPath);
        }

        private static void ReplicateDirectory(string sourcePath, string targetPath)
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

        private static void ClaerTargetFiles(string sourcePath, string targetPath)
        {
            foreach (var filePath in Directory.GetFiles(targetPath))
            {
                string sourceFilePath = Path.Combine(sourcePath, Path.GetFileName(filePath));

                if (File.Exists(sourceFilePath)) continue;

                File.Delete(filePath);
            }
        }

        private static void ReplicateSourceFiles(string sourcePath, string targetPath)
        {
            foreach (var filePath in Directory.GetFiles(sourcePath))
            {
                string targetFilePath = Path.Combine(targetPath, Path.GetFileName(filePath));

                if (File.Exists(targetFilePath) && !IsFileChanged(filePath, targetFilePath)) continue;

                File.Copy(filePath, targetFilePath, true);
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
