using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LasFinder.Impl
{
    public class LasFileReader : ILasFileReader
    {
        private static readonly Regex ValueRegex = new Regex(@".*?\.\S*\s+(?<value>.*?):", RegexOptions.Compiled);

        private readonly string logTypeField;

        public LasFileReader(string logTypeField)
        {
            this.logTypeField = logTypeField;
        }

        public async Task<LasFileHeaderInfo> ReadHeaderInfoAsync(FileInfo fileInfo)
        {
            var lasFileHeaderInfo = new LasFileHeaderInfo
            {
                LogType = null,
            };

            using (var fileStream = fileInfo.OpenRead())
            using (var reader = new StreamReader(fileStream))
            {
                var insideParametersSection = false;
                string line = string.Empty;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var sectionLine = line.StartsWith("~", StringComparison.InvariantCultureIgnoreCase);
                    if (sectionLine)
                    {
                        string upperLine = line.ToUpperInvariant();
                        if (isAscillSectionLine(upperLine))
                        {
                            // it is section with values, we've gone too far
                            break;
                        }

                        insideParametersSection = isParametersSection(upperLine);
                    }
                    else if (insideParametersSection && line.StartsWith(this.logTypeField, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var match = ValueRegex.Match(line);
                        if (match.Success)
                        {
                            lasFileHeaderInfo.LogType = match.Groups["value"].Value;
                            break;
                        }
                    }
                }

                return lasFileHeaderInfo;
            }
        }

        private bool isAscillSectionLine(string line)
        {
            // ~ASCII LOG DATA
            return line[1] == 'A';
        }

        private bool isParametersSection(string line)
        {
            // ~PARAMETER INFORMATION SECTION
            return line[1] == 'P';
        }
    }
}
