using FluentAssertions;
using FolderSyncTool.App.FileSync.Scheduling;
using FolderSyncTool.App.FileSync.Service;
using FolderSyncTool.App.Logger.Scheduling;
using FolderSyncTool.App.Logger.Service;
using NSubstitute;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderSyncTool.UnitTests.Logger
{
    public class LoggerJobTests
    {
        private readonly IJobExecutionContext _jobExecutionContext = Substitute.For<IJobExecutionContext>();
        private readonly ILoggerService _loggerService = Substitute.For<ILoggerService>();
        private readonly LoggerJob _loggerJob;

        public LoggerJobTests()
        {
            _loggerJob = new LoggerJob(_loggerService);
        }

        [Fact]
        public async Task Execute_ShouldCallFileSyncService()
        {
            //Arrange
            string logsPath = "G:/Test";

            var jobDataMap = new JobDataMap
            {
                { LoggerJobConfiguration.LogsPathKey, logsPath},
            };

            _jobExecutionContext.MergedJobDataMap.Returns(jobDataMap);

            //Act
            await _loggerJob.Execute(_jobExecutionContext);

            //Assert
            _loggerService.Received(1).SaveLogFile(logsPath);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("notapath")]
        public async Task Execute_ShouldThrowException_WhenInvalidPath(string? logsPath)
        {
            var jobDataMap = new JobDataMap
            {
                { LoggerJobConfiguration.LogsPathKey, logsPath},
            };

            _jobExecutionContext.MergedJobDataMap.Returns(jobDataMap);

            //Act
            var act = () => _loggerJob.Execute(_jobExecutionContext);

            //Assert
            await act.Should().ThrowAsync<Exception>();
        }
    }
}
