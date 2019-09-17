using System;

namespace LasFinder
{
    public interface ILasFileIndexedStorage : IDisposable
    {
        void RebuildIndex();

        LasFileRecordPage SearchByLogType(string logType, int pageSize = 20);
    }
}
