using FluentAssertions;
using FolderSyncTool.App.FileSync.Service;
using FolderSyncTool.App.Logger.Service;
using NSubstitute;

namespace FolderSyncTool.FunctionalTests.FileSync
{
    public class FileSyncServiceTests
    {
        private readonly ILoggerService _loggerService = Substitute.For<ILoggerService>();
        private readonly FileSyncService _fileSyncService;

        public FileSyncServiceTests()
        {
            _fileSyncService = new FileSyncService(_loggerService);
        }

        [Fact]
        public void ReplicateDirectory_ShouldCopyNewFiles()
        {
            //Arrange
            var (sourcePath, targetPath) = CreateTempDirs();
            string fileName = "test.txt";
            string content = "test123";
            string copiedFile = Path.Combine(targetPath, fileName);

            File.WriteAllText(Path.Combine(sourcePath, fileName), content);

            try
            {
                //Act
                _fileSyncService.ReplicateDirectory(sourcePath, targetPath);

                //Assert
                File.Exists(copiedFile).Should().BeTrue();
                File.ReadAllText(copiedFile).Should().Be(content);
            }
            finally
            {
                CleanUp(sourcePath, targetPath);
            }
        }

        [Fact]
        public void ReplicateDirectory_ShouldRemoveDeletedFiles()
        {
            //Arrange
            var (sourcePath, targetPath) = CreateTempDirs();
            string fileName = "stale.txt";
            File.WriteAllText(Path.Combine(targetPath, fileName), "Old content");

            try
            {
                //Act
                _fileSyncService.ReplicateDirectory(sourcePath, targetPath);

                //Assert
                File.Exists(Path.Combine(targetPath, fileName)).Should().BeFalse();
            }
            finally
            {
                CleanUp(sourcePath, targetPath);
            }
        }

        [Fact]
        public void ReplicateDirectory_ShouldUpdateModifiedFiles()
        {
            //Arrange
            var (sourcePath, targetPath) = CreateTempDirs();
            string fileName = "update.txt";
            string newContent = "new";
            string updatedFile = Path.Combine(targetPath, fileName);

            File.WriteAllText(Path.Combine(sourcePath, fileName), newContent);
            File.WriteAllText(Path.Combine(targetPath, fileName), "old");

            try
            {
                //Act
                _fileSyncService.ReplicateDirectory(sourcePath, targetPath);

                //Assert
                File.ReadAllText(updatedFile).Should().Be(newContent);
            }
            finally
            {
                CleanUp(sourcePath, targetPath);
            }
        }

        [Fact]
        public void ReplicateDirectory_ShouldHandleNestedDirectories()
        {
            //Arrange
            var (sourcePath, targetPath) = CreateTempDirs();
            string subFolderName = "subFolder";
            string subFileName = "subFile";
            string fileContent = "test";

            string subFolder = Path.Combine(sourcePath, subFolderName);
            string replicatedFilePath = Path.Combine(targetPath, subFolderName, subFileName);

            Directory.CreateDirectory(subFolder);
            File.WriteAllText(Path.Combine(subFolder, subFileName), fileContent);

            try
            {
                //Act
                _fileSyncService.ReplicateDirectory(sourcePath, targetPath);

                //Assert
                File.Exists(replicatedFilePath).Should().BeTrue();
                File.ReadAllText(replicatedFilePath).Should().Be(fileContent);
            }
            finally
            {
                CleanUp(sourcePath, targetPath);
            }
        }

        private static (string sourcePath, string targetPath) CreateTempDirs()
        {
            string sourcePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            string targetPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            Directory.CreateDirectory(sourcePath);
            Directory.CreateDirectory(targetPath);

            return (sourcePath, targetPath);
        }

        private static void CleanUp(params string[] paths)
        {
            foreach (var path in paths)
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
        }
    }
}
