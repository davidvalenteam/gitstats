using System;

namespace GitStats.Model
{
    internal class DigestAuthor 
    {
        public Author Author { get; private set; }
        public int PercentageLinesAdded { get; private set; }
        public int PercentageLinesDeleted { get; private set; }
        public int PercentageTotalCommits { get; private set; }
        public int PonderatedPercentage { get; private set; }

        public DigestAuthor(Author author, int percentageLinesAdded, int percentageLinesDeleted, int percentageTotalCommits)
        {
            Author = author;
            PercentageLinesAdded = percentageLinesAdded;
            PercentageLinesDeleted = percentageLinesDeleted;
            PercentageTotalCommits = percentageTotalCommits;
            PonderatedPercentage = Convert.ToInt32(Decimal.Divide(PercentageLinesAdded + PercentageLinesDeleted + PercentageTotalCommits, 3));
        }
    }
}
