namespace LasFinder.Configuration
{
    public interface IConfiguration
    {
        PrintMode PrintMode { get; }

        int PageSize { get; }

        ILasFilesConfiguration LasFilesConfiguration { get; }
    }

    public enum PrintMode
    {
        Table,
        List,
    }
}
