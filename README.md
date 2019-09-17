# LAS file finder

###  Requirements:

-   Implement the following using C# / .NET
-   Get attached [Log ASCII Standard (LAS)](https://en.wikipedia.org/wiki/Log_ASCII_Standard) files
-   Place LAS files in different subfolders under running assembly
-   The solution should
    -   find all LAS files in subfolders
    -   find all logs in LAS files
    -   index types of logs found inside LAS files
    -   provide simple UI or command line
-   Using UI or command line, it should be possible to search by log type, and give a result as a list of LAS files that have this log type inside
-   (optional) Use Lucene.NET

## How to run

In project folder:
```bash
> dotnet run
```

## How to use

Once started, application will read and index LAS files from the folder specified in `app-settings.json`.

Type a search term for log type and press `enter`:
```bash
log-finder> pa
Searching files for 'pa' term...
2 files found:
+------------------------------------------------+---------+
| Filename                                       | LogType |
+------------------------------------------------+---------+
| 7125_4-1__LOGS__VELOCITY_TZV_DEPTH_COMP__1.LAS | PAP.    |
+------------------------------------------------+---------+
| 7125-4-1__WLC_COMPOSITE__1.LAS                 | PAP.    |
+------------------------------------------------+---------+
```

## Help information

```bash
log-finder> :help
Well Log Finder (1.x.x)
Usage: Enter a command or a search term for log type.
Commands:
    :q|:quit     Terminates the current session.
    :h|:help     Shows help information (this one).
    :i|:index    Rebuilds the search index. Removes already existed search index and builds a new index.
```
