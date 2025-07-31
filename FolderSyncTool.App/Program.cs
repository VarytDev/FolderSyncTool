using FolderSyncTool.App.ArgumentParser.Service;
using FolderSyncTool.App.Common.Structs;
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

            var parseService = host.Services.GetRequiredService<IArgumentParserService>();
            Options options = parseService.Parse(args);

            if (string.IsNullOrEmpty(options.SourcePath))
            {
                throw new NullReferenceException(nameof(options.SourcePath));
            }

            if(string.IsNullOrEmpty(options.ReplicaPath))
            {
                throw new NullReferenceException(nameof(options.ReplicaPath));
            }

            var syncService = host.Services.GetRequiredService<IFileSyncService>();
            syncService.Sync(options.SourcePath, options.ReplicaPath);
        }
    }
}
