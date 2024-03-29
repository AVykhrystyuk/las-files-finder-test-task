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
    public class LuceneLasFileStorage : ILasFileIndexedStorage
    {
        static LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_48;

        private readonly DirectoryInfo indexLocation;

        private FSDirectory? fsDirectory;
        private DirectoryReader? directoryReader;


        public LuceneLasFileStorage(DirectoryInfo indexLocation)
        {
            this.indexLocation = indexLocation;
        }

        private FSDirectory FSDirectory => fsDirectory ?? (fsDirectory = FSDirectory.Open(this.indexLocation));
        private DirectoryReader DirectoryReader => directoryReader ?? (directoryReader = DirectoryReader.Open(this.FSDirectory));

        public bool HasIndex()
        {
            return this.indexLocation.Exists;
        }

        public void RebuildIndex(IReadOnlyList<LasFileRecord> records)
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
                foreach (var record in records)
                {
                    var document = BuildDocument(record);
                    writer.AddDocument(document);
                }

                writer.Flush(triggerMerge: false, applyAllDeletes: false);
                writer.Commit();
            }
        }

        public LasFileRecordPage SearchByLogType(string logType, int pageSize)
        {
            var searcher = new IndexSearcher(this.DirectoryReader);

            var loweredLogType = logType.ToLowerInvariant();
            var term = new Term(nameof(LasFileRecord.LogType), loweredLogType);
            var query = new PrefixQuery(term);

            var searchResult = searcher.Search(query, n: pageSize);
            var hits = searchResult.ScoreDocs;

            var records = new List<LasFileRecord>(hits.Length);

            foreach (var hit in hits)
            {
                var document = searcher.Doc(hit.Doc);
                var record = BuildRecord(document);
                records.Add(record);
            }

            return new LasFileRecordPage
            {
                TotalCount = searchResult.TotalHits,
                Records = records,
            };
        }

        public void Dispose()
        {
            this.directoryReader?.Dispose();
            this.fsDirectory?.Dispose();
        }

        private static LasFileRecord BuildRecord(Document document)
        {
            return new LasFileRecord
            {
                Filename = document.Get(nameof(LasFileRecord.Filename)),
                LogType = document.Get(nameof(LasFileRecord.LogType)),
            };
        }

        private static Document BuildDocument(LasFileRecord record)
        {
            var logType = string.IsNullOrWhiteSpace(record.LogType) ? "EMPTY" : record.LogType;

            return new Document
            {
                new TextField(nameof(record.LogType), logType, Field.Store.YES),

                new StoredField(nameof(record.Filename), record.Filename),
            };
        }
    }
}
