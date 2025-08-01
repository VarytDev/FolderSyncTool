using FolderSyncTool.App.FileSync.Scheduling;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace FolderSyncTool.App
{
    public static class ScheduleConfiguration
    {
        public static IServiceCollection AddSchedulingServices(this IServiceCollection services)
        {
            services.AddQuartz();

            services.AddTransient<FileSyncJob>();

            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            return services;
        }
    }
}
