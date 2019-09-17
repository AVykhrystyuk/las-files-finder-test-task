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

        public static IConfiguration FetchFromJsonFile()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("app-settings.json");

            var configuration = builder.Build();

            return new JsonConfiguration
            {
                PageSize = int.Parse(configuration["pageSize"]),
                LasFilesFolder = configuration["lasFilesFolder"],
                PrintMode = configuration["printMode"].Equals("Table", StringComparison.InvariantCultureIgnoreCase)
                    ? PrintMode.Table
                    : PrintMode.List,
            };
        }
    }
}
