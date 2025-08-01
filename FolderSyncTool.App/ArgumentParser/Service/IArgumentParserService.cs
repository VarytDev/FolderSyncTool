using FolderSyncTool.App.Common.Data;

namespace FolderSyncTool.App.ArgumentParser.Service
{
    public interface IArgumentParserService
    {
        public Config Parse(string[] args);
    }
}
