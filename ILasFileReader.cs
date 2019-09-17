using System.IO;
using System.Threading.Tasks;

namespace LasFinder
{
    public class LasFileHeaderInfo
    {
        public string? LogType { get; set; }
    }

    public interface ILasFileReader
    {
        Task<LasFileHeaderInfo> ReadHeaderInfoAsync(FileInfo fileInfo);
    }
}
