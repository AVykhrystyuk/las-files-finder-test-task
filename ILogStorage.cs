using System;

namespace LasFinder
{
    public interface ILogStorage : IDisposable
    {
        void RebuildIndex();

        DataRecordPage SearchByLogType(string logType, int pageSize = 20);
    }
}
