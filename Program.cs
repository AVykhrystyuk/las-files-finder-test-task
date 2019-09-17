using System;
using System.IO;
using LasFinder.Impl;

namespace LasFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppContext.BaseDirectory); // https://github.com/dotnet/project-system/issues/589

            IConfiguration configuration = JsonConfiguration.FetchFromJsonFile();

            var currentDirectory = Directory.GetCurrentDirectory();
            var indexLocation = new DirectoryInfo(Path.Combine(currentDirectory, "Index", "Lucene"));

            using (var logStorage = LuceneLogStorage.Build(indexLocation))
            {
                new ConsoleApplication(logStorage, configuration).Run();
            }
        }
    }
}
