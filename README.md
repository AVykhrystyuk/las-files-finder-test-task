# LAS files finder


###  Prerequisites
-  Download and place [LAS (Log ASCII Standard)](https://en.wikipedia.org/wiki/Log_ASCII_Standard) files in different subfolders under running assembly


###  Requirements:

-   The solution should:
    -   find all LAS files in the subfolders (see Prerequisites section)
    -   find all logs in the files found
    -   index types of the logs found
    -   provide a simple UI or command line that allows to search by log type, and give a result as a list of LAS files that have this log type inside
-   (optional) Use Lucene.NET

## How to run

In project folder:
```bash
> dotnet run
```

## How to use

Once started, application will read and index all LAS files from the folder specified in `app-settings.json`.

To search LAS files by the type of the log, type a search term and press `enter`:
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
