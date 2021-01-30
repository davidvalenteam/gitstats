using GitStats.Model;
using LibGit2Sharp;
using System.Collections.Generic;
using System.Linq;

namespace GitStats
{
    internal static class Task
    {
        public static Digest Execute(string gitFolder)
        {
            var authors = new Dictionary<string, Author>();

            using var repo = new Repository(gitFolder);

            var commits = repo.Commits.QueryBy(new CommitFilter()
            {
                SortBy = CommitSortStrategies.Reverse
            });

            foreach (var commit in commits)
            {
                if (!authors.ContainsKey(commit.Author.Name))
                {
                    authors.Add(commit.Author.Name, new Author(commit.Author.Name, commit.Author.Email));
                }

                if (commit.Parents.Count() == 1)  //a merge commit is not considered neither the first commit
                {
                    var patch = repo.Diff.Compare<Patch>(commit.Parents.First().Tree, commit.Tree);

                    foreach (var ptc in patch)
                    {
                        authors[commit.Author.Name].LinesAdded += ptc.LinesAdded;
                        authors[commit.Author.Name].LinesDeleted += ptc.LinesDeleted;
                    }

                    authors[commit.Author.Name].TotalCommits++;
                }
            }

            return Digest.DigestAuthors(authors.Values, gitFolder);
        }
    }
}
