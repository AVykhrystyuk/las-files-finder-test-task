using System.Collections.Generic;
using System.IO;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Util;
using Lucene.Net.Search;
using Lucene.Net.Store;

namespace LasFinder.Impl
{
    public class LuceneLogStorage : ILogStorage
    {
        static LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_48;

        private readonly DirectoryInfo indexLocation;

        private FSDirectory? fsDirectory;
        private DirectoryReader? directoryReader;


        private LuceneLogStorage(DirectoryInfo indexLocation)
        {
            this.indexLocation = indexLocation;
        }

        private FSDirectory FSDirectory => fsDirectory ?? (fsDirectory = FSDirectory.Open(this.indexLocation));
        private DirectoryReader DirectoryReader => directoryReader ?? (directoryReader = DirectoryReader.Open(this.FSDirectory));

        public void RebuildIndex()
        {
            if (this.indexLocation.Exists)
            {
                this.indexLocation.Delete(true);

                this.directoryReader?.Dispose();
                this.directoryReader = null;

                this.fsDirectory?.Dispose();
                this.fsDirectory = null;
            }

            using (var analyzer = new StandardAnalyzer(AppLuceneVersion))
            using (var writer = new IndexWriter(this.FSDirectory, new IndexWriterConfig(AppLuceneVersion, analyzer)))
            {
                var sourceRecords = DataRecordMother.Create();

                foreach (var sourceRecord in sourceRecords)
                {
                    var document = BuildDocument(sourceRecord);
                    writer.AddDocument(document);
                }

                writer.Flush(triggerMerge: false, applyAllDeletes: false);
                writer.Commit();
            }
        }

        public DataRecordPage SearchByLogType(string logType, int pageSize)
        {
            var searcher = new IndexSearcher(this.DirectoryReader);

            var loweredLogType = logType.ToLowerInvariant();
            var term = new Term(nameof(DataRecord.LogType), loweredLogType);
            var query = new PrefixQuery(term);

            var searchResult = searcher.Search(query, n: pageSize);
            var hits = searchResult.ScoreDocs;

            var records = new List<DataRecord>(hits.Length);

            foreach (var hit in hits)
            {
                var document = searcher.Doc(hit.Doc);
                var record = BuildRecord(document);
                records.Add(record);
            }

            return new DataRecordPage
            {
                TotalCount = searchResult.TotalHits,
                Records = records,
            };
        }

        public static LuceneLogStorage Build(DirectoryInfo indexLocation)
        {
            var luceneLogStorage = new LuceneLogStorage(indexLocation);
            if (!indexLocation.Exists)
            {
                luceneLogStorage.RebuildIndex();
            }
            return luceneLogStorage;
        }

        public void Dispose()
        {
            this.directoryReader?.Dispose();
            this.fsDirectory?.Dispose();
        }

        private static DataRecord BuildRecord(Document document)
        {
            return new DataRecord
            {
                Filename = document.Get(nameof(DataRecord.Filename)),
                LogType = document.Get(nameof(DataRecord.LogType)),
            };
        }

        private static Document BuildDocument(DataRecord record)
        {
            return new Document
            {
                new TextField(nameof(record.LogType), record.LogType, Field.Store.YES),

                new StoredField(nameof(record.Filename), record.Filename),
            };
        }
    }
}
