using System;
using System.IO;
using LasFinder.Configuration;
using LasFinder.Configuration.Impl;
using LasFinder.Impl;

namespace LasFinder
{
    class Program
    {
        static Program()
        {
            Directory.SetCurrentDirectory(AppContext.BaseDirectory); // https://github.com/dotnet/project-system/issues/589
        }

        static void Main(string[] args)
        {
            IConfiguration configuration = JsonConfiguration.FetchFromJsonFile();

            var lasFilesSourceFolder = configuration.LasFilesConfiguration.SourceFolder;

            if (string.IsNullOrWhiteSpace(lasFilesSourceFolder))
            {
                Console.WriteLine($"LAS file folder is not provided ('lasFilesFolder' property) in the settings file located at '{JsonConfiguration.SuggestJsonFilePath()}'");
                return;
            }

            var lasFilesDirectory = new DirectoryInfo(Path.GetFullPath(lasFilesSourceFolder));
            if (!lasFilesDirectory.Exists)
            {
                Console.WriteLine($"LAS file folder cannot be found at '{lasFilesDirectory.FullName}'");
                Console.WriteLine($"The folder location can be changed in the settings file located at '{JsonConfiguration.SuggestJsonFilePath()}'");
                return;
            }

            var fileStorage = new LasFileStorage();

            var currentDirectory = Directory.GetCurrentDirectory();
            var indexLocation = new DirectoryInfo(Path.Combine(currentDirectory, "Index", "Lucene"));

            using (var fileIndexedStorage = new LuceneLasFileStorage(indexLocation))
            {
                new ConsoleApplication(fileIndexedStorage, fileStorage, configuration).Run();
            }
        }
    }
}
