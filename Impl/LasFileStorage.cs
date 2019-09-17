using System.Collections.Generic;

namespace LasFinder.Impl
{
    public class LasFileStorage : ILasFileStorage
    {
        public IReadOnlyList<LasFileRecord> FetchFileRecords()
        {
            return LasFileRecordMother.Create();
        }
    }
}
