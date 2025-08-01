using CommandLine;
using FolderSyncTool.App.Common.Data;

namespace FolderSyncTool.App.ArgumentParser.Service
{
    public class ArgumentParserService : IArgumentParserService
    {
        public Config Parse(string[] args)
        {
            var result = Parser.Default.ParseArguments<Config>(args);
            return result.Value;
        }
    }
}
