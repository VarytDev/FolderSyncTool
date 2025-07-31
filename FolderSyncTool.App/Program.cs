using FolderSyncTool.App.FileSync.Service;
using FolderSyncTool.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FolderSyncTool.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args);
            builder.ConfigureServices(services =>
            {
                services.AddServices();
            });

            var host = builder.Build();

            var syncService = host.Services.GetRequiredService<IFileSyncService>();
            syncService.Sync("E:\\SyncTest\\Source", "E:\\SyncTest\\Replica");
        }
    }
}
