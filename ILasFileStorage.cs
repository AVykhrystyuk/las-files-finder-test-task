using System.Collections.Generic;
using System.Threading.Tasks;

namespace LasFinder
{
    public interface ILasFileStorage
    {
        Task<IReadOnlyList<LasFileRecord>> FetchFileRecords();
    }
}
