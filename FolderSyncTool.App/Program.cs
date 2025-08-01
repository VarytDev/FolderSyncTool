using FolderSyncTool.App.ArgumentParser.Service;
using FolderSyncTool.App.Common.Data;
using FolderSyncTool.App.FileSync.Scheduling;
using FolderSyncTool.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace FolderSyncTool.App
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args);
            builder.ConfigureServices(services =>
            {
                services.AddServices();
                services.AddSchedulingServices();
            });

            var host = builder.Build();

            var parseService = host.Services.GetRequiredService<IArgumentParserService>();
            Config config = parseService.Parse(args);

            var schedulerFactory = host.Services.GetRequiredService<ISchedulerFactory>();
            var scheduler = await schedulerFactory.GetScheduler();
            await scheduler.ScheduleFileSyncJob(config);

            await host.RunAsync();
        }
    }
}
