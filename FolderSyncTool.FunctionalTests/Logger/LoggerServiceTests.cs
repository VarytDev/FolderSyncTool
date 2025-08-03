using FluentAssertions;
using FolderSyncTool.App.Logger.Service;

namespace FolderSyncTool.FunctionalTests.Logger
{
    public class LoggerServiceTests
    {
        private readonly LoggerService _loggerService = new LoggerService();


        [Fact]
        public void SaveLogFile_ShouldWriteLogToFile()
        {
            //Arrange
            string tempDir = CreateTempDirectory();
            string log = "test log message";
            _loggerService.StringBuilder.AppendLine(log);

            string expectedPath = Path.Combine(tempDir, LoggerService.LogFileName);

            try
            {
                //Act
                _loggerService.SaveLogFile(tempDir);

                //Assert
                File.Exists(expectedPath).Should().BeTrue();
                File.ReadAllText(expectedPath).Should().Contain(log);
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        [Fact]
        public void SaveLogFile_ShouldClearStringBuilderAfterWrite()
        {
            //Arrange
            string tempDir = CreateTempDirectory();
            _loggerService.StringBuilder.AppendLine("Cleared content");

            try
            {
                //Act
                _loggerService.SaveLogFile(tempDir);

                //Assert
                _loggerService.StringBuilder.Length.Should().Be(0);
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        [Fact]
        public void SaveLogFile_ShouldAppendToExistingFile()
        {
            //Arrange
            string tempDir = CreateTempDirectory();
            string logFilePath = Path.Combine(tempDir, LoggerService.LogFileName);
            string initialLine = "initial line\n";
            string appendedLine = "Appended line";

            File.WriteAllText(logFilePath, initialLine);

            _loggerService.StringBuilder.AppendLine(appendedLine);

            try
            {
                //Act
                _loggerService.SaveLogFile(tempDir);

                //Assert
                File.ReadAllText(logFilePath).Should().Contain(initialLine).And.Contain(appendedLine);
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        private static string CreateTempDirectory()
        {
            string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(path);
            return path;
        }
    }
}
