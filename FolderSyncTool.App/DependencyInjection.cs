using FolderSyncTool.App.FileSync.Service;
using Microsoft.Extensions.DependencyInjection;

namespace FolderSyncTool.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IFileSyncService, FileSyncService>();

            return services;
        }
    }
}
