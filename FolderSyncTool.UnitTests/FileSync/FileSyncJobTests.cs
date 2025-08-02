using FluentAssertions;
using FolderSyncTool.App.FileSync.Scheduling;
using FolderSyncTool.App.FileSync.Service;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Quartz;

namespace FolderSyncTool.UnitTests.FileSync
{
    public class FileSyncJobTests
    {
        private readonly IJobExecutionContext _jobExecutionContext = Substitute.For<IJobExecutionContext>();
        private readonly IFileSyncService _fileSyncService = Substitute.For<IFileSyncService>();
        private readonly FileSyncJob _fileSyncJob;

        public FileSyncJobTests()
        {
            _fileSyncJob = new FileSyncJob(_fileSyncService);
        }

        [Fact]
        public async Task Execute_ShouldCallFileSyncService()
        {
            //Arrange
            string sourcePath = "G:/Test";
            string replicaPath = "G:/Test2";

            var jobDataMap = new JobDataMap
            {
                { FileSyncJobConfiguration.SourcePathKey, sourcePath},
                { FileSyncJobConfiguration.ReplicaPathKey, replicaPath}
            };

            _jobExecutionContext.MergedJobDataMap.Returns(jobDataMap);

            //Act
            await _fileSyncJob.Execute(_jobExecutionContext);

            //Assert
            _fileSyncService.Received(1).Sync(sourcePath, replicaPath);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(null, null)]
        [InlineData("notapath", "notapath")]
        [InlineData("G:/Test", null)]
        [InlineData(null, "G:/Test")]
        public async Task Execute_ShouldThrowException_WhenInvalidPath(string? sourcePath, string? replicaPath)
        {
            var jobDataMap = new JobDataMap
            {
                { FileSyncJobConfiguration.SourcePathKey, sourcePath},
                { FileSyncJobConfiguration.ReplicaPathKey, replicaPath}
            };

            _jobExecutionContext.MergedJobDataMap.Returns(jobDataMap);

            //Act
            var act = () => _fileSyncJob.Execute(_jobExecutionContext);

            //Assert
            await act.Should().ThrowAsync<Exception>();
        }
    }
}
