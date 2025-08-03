using FluentAssertions;
using FolderSyncTool.App.ArgumentParser.Service;
using FolderSyncTool.App.Common.Data;

namespace FolderSyncTool.UnitTests.ArgumentParser
{
    public class ArgumentParserServiceTests
    {
        private readonly IArgumentParserService _parserService = new ArgumentParserService();

        [Fact]
        public void Parse_ShouldReturnValidConfig()
        {
            //Arrange
            string sourcePath = "E:/SyncTest/Source";
            string replicaPath = "E:/SyncTest/Replica";
            int interval = 10;
            string logsPath = "E:/SyncTest/Logs";

            string commandLineArgs = $"-s {sourcePath} -r {replicaPath} -i {interval} -l {logsPath}";
            var args = commandLineArgs.Split(' ');
            var configToComapre = new Config
            {
                SourcePath = sourcePath,
                ReplicaPath = replicaPath,
                SyncInterval = interval,
                LogsPath = logsPath,
            };

            //Act
            var config = _parserService.Parse(args);

            //Assert
            config.Should().BeEquivalentTo(configToComapre);
        }

        [Fact]
        public void Parse_ShouldReturnValidConfig_WhenArgAliasesAreUsed()
        {
            //Arrange
            string sourcePath = "E:/SyncTest/Source";
            string replicaPath = "E:/SyncTest/Replica";
            int interval = 10;
            string logsPath = "E:/SyncTest/Logs";

            string commandLineArgs = $"--source {sourcePath} --replica {replicaPath} --interval {interval} --logs {logsPath}";
            var args = commandLineArgs.Split(' ');
            var configToComapre = new Config
            {
                SourcePath = sourcePath,
                ReplicaPath = replicaPath,
                SyncInterval = interval,
                LogsPath = logsPath,
            };

            //Act
            var config = _parserService.Parse(args);

            //Assert
            config.Should().BeEquivalentTo(configToComapre);
        }

        [Theory]
        [InlineData("E:/SyncTest/Source", "E:/SyncTest/Replica", 10, null)]
        [InlineData("E:/SyncTest/Source", "E:/SyncTest/Replica", null, null)]
        [InlineData("E:/SyncTest/Source", "E:/SyncTest/Replica", null, "E:/SyncTest/Logs")]
        public void Parse_ShouldReturnValidConfig_WhenOptionalArgumentsAreMissing(string? sourcePath, string? replicaPath, int? interval, string? logsPath)
        {
            //Arrange
            string commandLineArgs = "";
            if (sourcePath != null) commandLineArgs += $"-s {sourcePath} ";
            if (replicaPath != null) commandLineArgs += $"-r {replicaPath} ";
            if (interval != null) commandLineArgs += $"-i {interval} ";
            if (logsPath != null) commandLineArgs += $"-l {logsPath} ";

            var args = commandLineArgs.TrimEnd().Split(' ');
            var configToComapre = new Config
            {
                SourcePath = sourcePath,
                ReplicaPath = replicaPath,
                SyncInterval = interval ?? -1,
                LogsPath = logsPath,
            };

            //Act
            var config = _parserService.Parse(args);

            //Assert
            config.Should().BeEquivalentTo(configToComapre);
        }

        [Theory]
        [InlineData("E:/SyncTest/Source", null, null, "E:/SyncTest/Logs")]
        [InlineData(null, null, null, null)]
        public void Parse_ShouldReturnNull_WhenRequiredArgumentsAreMissing(string? sourcePath, string? replicaPath, int? interval, string? logsPath)
        {
            //Arrange
            string commandLineArgs = "";
            if (sourcePath != null) commandLineArgs += $"-s {sourcePath} ";
            if (replicaPath != null) commandLineArgs += $"-r {replicaPath} ";
            if (interval != null) commandLineArgs += $"-i {interval} ";
            if (logsPath != null) commandLineArgs += $"-l {logsPath} ";

            var args = commandLineArgs.TrimEnd().Split(' ');

            //Act
            var config = _parserService.Parse(args);

            //Assert
            config.Should().BeNull();
        }
    }
}
