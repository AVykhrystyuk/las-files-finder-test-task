using System;
using System.Collections.Generic;

namespace LasFinder
{
    public interface ILasFileIndexedStorage : IDisposable
    {
        bool HasIndex();

        void RebuildIndex(IReadOnlyList<LasFileRecord> records);

        LasFileRecordPage SearchByLogType(string logType, int pageSize = 20);
    }
}
