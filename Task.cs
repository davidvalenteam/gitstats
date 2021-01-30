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
            var commitsSha = new HashSet<string>();

            using var repo = new Repository(gitFolder);

            foreach (var branch in repo.Branches.Where(b => b.IsRemote))
            {
                foreach (var commit in branch.Commits.OrderBy(c => c.Author.When))
                {
                    if(commitsSha.Contains(commit.Sha))
                        continue;

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

                    commitsSha.Add(commit.Sha);
                }
            }

            return Digest.DigestAuthors(authors.Values, gitFolder);
        }
    }
}
