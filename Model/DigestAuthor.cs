using System;

namespace GitStats.Model
{
    internal class DigestAuthor 
    {
        public Author Author { get; private set; }
        public int PercentageLinesAdded { get; private set; }
        public int PercentageLinesDeleted { get; private set; }
        public int PercentageTotalCommitsWithOneParent { get; private set; }
        public int PonderatedPercentage { get; private set; }

        public DigestAuthor(Author author, int percentageLinesAdded, int percentageLinesDeleted, int percentageTotalCommitsWithOneParent)
        {
            Author = author;
            PercentageLinesAdded = percentageLinesAdded;
            PercentageLinesDeleted = percentageLinesDeleted;
            PercentageTotalCommitsWithOneParent = percentageTotalCommitsWithOneParent;

            PonderatedPercentage = Convert.ToInt32(Decimal.Divide(PercentageLinesAdded + PercentageLinesDeleted + PercentageTotalCommitsWithOneParent, 3));
        }
    }
}
