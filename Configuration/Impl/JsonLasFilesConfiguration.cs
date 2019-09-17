using Microsoft.Extensions.Configuration;

namespace LasFinder.Configuration.Impl
{
    public class JsonLasFilesConfiguration : ILasFilesConfiguration
    {
        public string SourceFolder { get; set; } = string.Empty;

        public string LogTypeField { get; set; } = string.Empty;

        public void FillFromConfigurationSection(IConfigurationSection section)
        {
            this.SourceFolder = section["sourceFolder"].Trim();
            this.LogTypeField = section["logTypeField"].Trim();
        }
    }
}
