using FolderSyncTool.App.ArgumentParser.Service;
using FolderSyncTool.App.FileSync.Service;
using FolderSyncTool.App.Logger.Service;
using Microsoft.Extensions.DependencyInjection;

namespace FolderSyncTool.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IArgumentParserService, ArgumentParserService>();
            services.AddTransient<IFileSyncService, FileSyncService>();
            services.AddSingleton<ILoggerService, LoggerService>();

            return services;
        }
    }
}
