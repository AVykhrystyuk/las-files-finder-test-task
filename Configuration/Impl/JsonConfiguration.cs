using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace LasFinder.Configuration.Impl
{
    public class JsonConfiguration : IConfiguration
    {
        public int PageSize { get; set; }

        public PrintMode PrintMode { get; set; }

        public JsonLasFilesConfiguration LasFilesConfiguration { get; set; } = new JsonLasFilesConfiguration();

        ILasFilesConfiguration IConfiguration.LasFilesConfiguration => this.LasFilesConfiguration;

        private static string FileName = "app-settings.json";

        private static string FileDirectory => Directory.GetCurrentDirectory();

        public static string SuggestJsonFilePath() => Path.Combine(FileDirectory, FileName);

        public static IConfiguration FetchFromJsonFile()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(FileDirectory)
                .AddJsonFile(FileName);

            var configuration = builder.Build();

            var jsonConfiguration = new JsonConfiguration
            {
                PageSize = int.Parse(configuration["pageSize"].Trim()),
                PrintMode = configuration["printMode"].Trim().Equals("Table", StringComparison.InvariantCultureIgnoreCase)
                    ? PrintMode.Table
                    : PrintMode.List,
            };

            jsonConfiguration.LasFilesConfiguration.FillFromConfigurationSection(configuration.GetSection("lasFiles"));
            return jsonConfiguration;
        }
    }
}
