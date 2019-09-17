using System;
using System.Collections.Generic;
using System.Reflection;
using ConsoleTables;
using LasFinder.Configuration;

namespace LasFinder
{
    public class ConsoleApplication
    {
        private string assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);

        private bool isRunning = false;
        private readonly ILasFileIndexedStorage fileIndexedStorage;
        private readonly ILasFileStorage fileStorage;
        private readonly IConfiguration configuration;

        public ConsoleApplication(ILasFileIndexedStorage fileIndexedStorage, ILasFileStorage fileStorage, IConfiguration configuration)
        {
            this.fileIndexedStorage = fileIndexedStorage;
            this.fileStorage = fileStorage;
            this.configuration = configuration;
        }

        public void Run()
        {
            this.BuildIndexIfMissing();

            this.PrintHelpInfo();

            this.isRunning = true;
            while (this.isRunning)
            {
                this.RunLoopIteration();
            }
        }

        private void BuildIndexIfMissing()
        {
            if (this.fileIndexedStorage.HasIndex()) {
                return;
            }

            Console.WriteLine();
            Console.WriteLine("[Initializing]: Reading LAS files...");
            var fileRecords = this.fileStorage.FetchFileRecords();
            Console.WriteLine("[Initializing]: Building index...");
            this.fileIndexedStorage.RebuildIndex(fileRecords);
            Console.WriteLine("[Initializing]: Index successfully built!");
        }

        private void RunLoopIteration()
        {
            Console.Write("log-finder>");
            var commandOrSearchTerm = Console.ReadLine().ToLowerInvariant().Trim();
            switch (commandOrSearchTerm)
            {
                case ":index":
                case ":i":
                    Console.WriteLine("Reading files...");
                    var fileRecords = this.fileStorage.FetchFileRecords();
                    Console.WriteLine("Re-building index...");
                    this.fileIndexedStorage.RebuildIndex(fileRecords);
                    Console.WriteLine("Index successfully re-built!");
                    Console.WriteLine();
                    break;

                case ":help":
                case ":h":
                    this.PrintHelpInfo();
                    Console.WriteLine();
                    break;

                case ":quit":
                case ":q":
                    this.isRunning = false;
                    Console.WriteLine("Good bye!");
                    Console.WriteLine();
                    break;

                default:
                    Console.WriteLine($"Searching files for '{commandOrSearchTerm}' term...");
                    var records = this.fileIndexedStorage.SearchByLogType(commandOrSearchTerm, this.configuration.PageSize);
                    this.PrintRecords(records);
                    break;
            }
        }

        private void PrintHelpInfo()
        {
            Console.WriteLine();
            Console.WriteLine($"Well Log Finder ({this.assemblyVersion})");
            Console.WriteLine("Usage: Enter a command or a search term for log type.");

            Console.WriteLine("Commands:");
            var commandInfos = new[]
            {
                Tuple.Create(":q|:quit", "Terminates the current session."),
                Tuple.Create(":h|:help", "Shows help information (this one)."),
                Tuple.Create(":i|:index", "Rebuilds the search index. Removes already existed search index and builds a new index."),
            };

            foreach (var (command, description) in commandInfos)
            {
                Console.WriteLine($"{string.Empty,4} {command,-12} {description,-90}");
            }
        }

        private void PrintRecords(LasFileRecordPage recordPage)
        {
            var records = recordPage.Records;

            if (recordPage.TotalCount == 0)
            {
                Console.WriteLine($"No files found.");
                return;
            }

            var postfix = records.Count > 0 ? "s" : string.Empty;
            var showing = recordPage.TotalCount != records.Count ? $" (showing {records.Count})" : string.Empty;
            Console.WriteLine($"{recordPage.TotalCount} file{postfix} found{showing}:");

            switch (this.configuration.PrintMode)
            {
                case PrintMode.Table:
                    this.PrintTable(records);
                    break;

                case PrintMode.List:
                    this.PrintList(records);
                    break;
                default:
                    throw new NotSupportedException("PrintMode is not supported");
            }
        }

        private void PrintTable(IReadOnlyList<LasFileRecord> records)
        {
            ConsoleTable.From<LasFileRecord>(records)
                .Configure(o => o.NumberAlignment = Alignment.Right)
                .Write(Format.Alternative);
        }

        private void PrintList(IReadOnlyList<LasFileRecord> records)
        {
            Console.WriteLine();

            foreach (var record in records)
            {
                Console.WriteLine($"[Filename]:  {record.Filename}");
                Console.WriteLine($"[LogType]:   {record.LogType}");
                Console.WriteLine();
            }
        }
    }
}
