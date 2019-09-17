using System.Collections.Generic;

namespace LasFinder
{
    public static class DataRecordMother
    {
        public static IReadOnlyList<DataRecord> Create()
        {
            return new[] {
                new DataRecord
                {
                    Filename = "7125_4-1__LOGS__STH_09__6794-1__7125-4-1__VELOCITY_TZV_DEPTH_COMP__1.LAS",
                    LogType = "TheTheThe quick brown fox lazy dog",
                },
                new DataRecord
                {
                    Filename = "7125_4-1__LOGS__STH_09__6794-1__7125-4-1__VELOCITY_TZV_DEPTH_COMP__1.LAS",
                    LogType = "PAP. SUPER TYPE",
                },
                new DataRecord
                {
                    Filename = "7125-4-1__WLC_COMPOSITE__1.LAS",
                    LogType = "PAPA. 123 abcd", // LOG TYPE
                },
                new DataRecord
                {
                    Filename = "asdasd  dasd 7125-4-asdadsadad.LAS",
                    LogType = "test 123456 asdasd", // LOG TYPE
                },
            };
        }
    }
}
