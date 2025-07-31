using CommandLine;
using FolderSyncTool.App.Common.Structs;

namespace FolderSyncTool.App.ArgumentParser.Service
{
    public class ArgumentParserService : IArgumentParserService
    {
        public Options Parse(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args);
            return result.Value;
        }
    }
}
