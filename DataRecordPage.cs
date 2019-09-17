using System.Collections.Generic;

namespace LasFinder
{
    public class LasFileRecordPage
    {
        private static IReadOnlyList<LasFileRecord> EmptyRecords = new LasFileRecord[0];

        public int TotalCount { get; set; }

        public IReadOnlyList<LasFileRecord> Records { get; set; } = EmptyRecords;
    }
}
