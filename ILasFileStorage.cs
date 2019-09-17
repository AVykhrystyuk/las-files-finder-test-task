using System.Collections.Generic;

namespace LasFinder
{
    public interface ILasFileStorage
    {
        IReadOnlyList<LasFileRecord> FetchFileRecords();
    }
}
