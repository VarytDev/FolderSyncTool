using CommandLine;

namespace FolderSyncTool.App.Common.Structs
{
    public class Options
    {
        [Option('s', "source", Default = null, Required = true, HelpText = "Source folder path.")]
        public string? SourcePath { get; set; }

        [Option('r', "replica", Default = null, Required = true, HelpText = "Source folder path.")]
        public string? ReplicaPath { get; set; }

        [Option('l', "logs", Default = null, Required = false, HelpText = "Source folder path.")]
        public string? LogsPath { get; set; }

        [Option('i', "interval", Default = -1, Required = false, HelpText = "Source folder path.")]
        public int SyncInterval { get; set; }
    }
}
