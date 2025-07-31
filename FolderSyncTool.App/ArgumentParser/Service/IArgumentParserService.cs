using FolderSyncTool.App.Common.Structs;

namespace FolderSyncTool.App.ArgumentParser.Service
{
    public interface IArgumentParserService
    {
        public Options Parse(string[] args);
    }
}
