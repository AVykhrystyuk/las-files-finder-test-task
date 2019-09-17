using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace LasFinder.Impl
{
    public class JsonConfiguration : IConfiguration
    {
        public string LasFilesFolder { get; set; } = string.Empty;

        public int PageSize { get; set; }

        public PrintMode PrintMode { get; set; }

        private static string FileName = "app-settings.json";

        private static string FileDirectory => Directory.GetCurrentDirectory();

        public static string SuggestJsonFilePath() => Path.Combine(FileDirectory, FileName);

        public static IConfiguration FetchFromJsonFile()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(FileDirectory)
                .AddJsonFile(FileName);

            var configuration = builder.Build();

            return new JsonConfiguration
            {
                PageSize = int.Parse(configuration["pageSize"].Trim()),
                LasFilesFolder = configuration["lasFilesFolder"].Trim(),
                PrintMode = configuration["printMode"].Trim().Equals("Table", StringComparison.InvariantCultureIgnoreCase)
                    ? PrintMode.Table
                    : PrintMode.List,
            };
        }
    }
}
