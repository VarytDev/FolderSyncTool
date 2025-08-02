using System.Security.Cryptography;

namespace FolderSyncTool.App.Common.Utilities
{
    public static class FileHashingUtility
    {
        public static string CheckMD5(string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            {
                return CheckMD5(stream);
            }
        }

        public static string CheckMD5(Stream stream)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
