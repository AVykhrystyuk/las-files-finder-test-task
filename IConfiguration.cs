namespace LasFinder
{
    public interface IConfiguration
    {
        string LasFilesFolder { get; }

        PrintMode PrintMode { get; }

        int PageSize { get; }
    }

    public enum PrintMode
    {
        Table,
        List,
    }
}
