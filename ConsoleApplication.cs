using System;
using System.Collections.Generic;
using System.Reflection;
using ConsoleTables;

namespace LasFinder
{
    public class ConsoleApplication
    {
        private string assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);

        private bool isRunning = false;
        private readonly ILogStorage logStorage;
        private readonly IConfiguration configuration;

        public ConsoleApplication(ILogStorage logStorage, IConfiguration configuration)
        {
            this.logStorage = logStorage;
            this.configuration = configuration;
        }

        public void Run()
        {
            this.PrintHelpInfo();

            this.isRunning = true;
            while (this.isRunning)
            {
                this.RunLoopIteration();
            }
        }

        private void RunLoopIteration()
        {
            Console.Write("log-finder>");
            var commandOrSearchTerm = Console.ReadLine().ToLowerInvariant().Trim();
            switch (commandOrSearchTerm)
            {
                case ":index":
                case ":i":
                    Console.WriteLine("Rebuilding index...");
                    this.logStorage.RebuildIndex();
                    Console.WriteLine("Index successfully rebuilt!");
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
                    var records = this.logStorage.SearchByLogType(commandOrSearchTerm, this.configuration.PageSize);
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

        private void PrintRecords(DataRecordPage recordPage)
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

        private void PrintTable(IReadOnlyList<DataRecord> records)
        {
            ConsoleTable.From<DataRecord>(records)
                .Configure(o => o.NumberAlignment = Alignment.Right)
                .Write(Format.Alternative);
        }

        private void PrintList(IReadOnlyList<DataRecord> records)
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
