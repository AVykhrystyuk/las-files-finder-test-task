using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LasFinder.Impl
{
    public class LasFileStorage : ILasFileStorage
    {
        private readonly DirectoryInfo directory;
        private readonly ILasFileReader fileReader;

        public LasFileStorage(DirectoryInfo directory, ILasFileReader fileReader)
        {
            this.directory = directory;
            this.fileReader = fileReader;
        }

        public async Task<IReadOnlyList<LasFileRecord>> FetchFileRecords()
        {
            var fileInfos = directory.GetFiles("*.LAS", SearchOption.AllDirectories);

            var recordsTasks = fileInfos
                .Select(async fileInfo =>
                {
                    var headerInfo = await this.fileReader.ReadHeaderInfoAsync(fileInfo);
                    return new LasFileRecord
                    {
                        Filename = fileInfo.Name,
                        LogType = headerInfo.LogType ?? string.Empty,
                    };
                })
                .ToList();

            var records = await Task.WhenAll(recordsTasks);
            return records;
        }
    }
}
