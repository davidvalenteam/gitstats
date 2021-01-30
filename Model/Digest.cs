﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace GitStats.Model
{
    internal class Digest
    {
        public IReadOnlyCollection<DigestAuthor> Authors { get; private set; }
        public long TotalLinesAdded { get; private set; }
        public long TotalLinesDeleted { get; private set; }
        public long TotalCommits { get; private set; }
        public string GitRepositoryPath { get; private set; }
        public DateTime DigestDateUTC { get; private set; }

        protected Digest() { }

        public static Digest DigestAuthors(IReadOnlyCollection<Author> authors, string gitRepositoryPath)
        {
            var digest = new Digest
            {
                DigestDateUTC = DateTime.UtcNow,
                GitRepositoryPath = gitRepositoryPath,
                TotalCommits = authors.Sum(s => s.TotalCommits),
                TotalLinesAdded = authors.Sum(s => s.LinesAdded),
                TotalLinesDeleted = authors.Sum(s => s.LinesDeleted)
            };

            digest.Authors = authors.Select(a => new DigestAuthor(a,
                    percentageLinesAdded: GetPercentage(a.LinesAdded, digest.TotalLinesAdded),
                    percentageLinesDeleted: GetPercentage(a.LinesDeleted, digest.TotalLinesDeleted),
                    percentageTotalCommits: GetPercentage(a.TotalCommits, digest.TotalCommits))).ToList();

            return digest;
        }

        private static int GetPercentage(long a, long b)
        {
            if (b == 0)
                return 0;

            return Convert.ToInt32(Decimal.Divide(a, b) * 100);
        }
    }
}
