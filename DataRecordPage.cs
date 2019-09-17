using System.Collections.Generic;

namespace LasFinder
{
    public class DataRecordPage
    {
        private static IReadOnlyList<DataRecord> EmptyRecords = new DataRecord[0];

        public int TotalCount { get; set; }

        public IReadOnlyList<DataRecord> Records { get; set; } = EmptyRecords;
    }
}
