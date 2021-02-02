using GitStats.Model;
using System.Linq;

namespace GitStats.ReportProviders
{
    internal static class HTMLReport
    {
        public static string CreateReport(Digest digest, string toolGitRepositoryUrl)
        {
            var html = @$"<!DOCTYPE html>
                          <html>
                              <head>
                                <title>Git Stats</title>
                                
                                <style>
                                    table, th, td {{
                                      border: 1px solid black;
                                      border-collapse: collapse;
                                      padding: 10px;
                                    }}
                                    
                                    th {{ background-color: #a7a37e; }}
                                </style>
                            </head>
                            <body>
                                <section>
                                    <p><strong>Git repository:</strong> {digest.GitRepositoryPath}</p>
                                    <p><strong>Total Authors:</strong> {digest.Authors.Count}</p>
                                    <p><strong>Total Commits With One Parent:</strong> {digest.TotalCommitsWithOneParent}</p>
                                    <p><strong>Total Lines Added:</strong> {digest.TotalLinesAdded}</p>
                                    <p><strong>Total Lines Deleted:</strong> {digest.TotalLinesDeleted}</p>
                                </section>
                                <section>
                                    <table>
                                        <tr>
                                            <th>Name</th>
                                            <th>Email</th>
                                            <th>Total Commits With One Parent</th>
                                            <th>Total Lines Added</th>
                                            <th>Total Lines Deleted</th>
                                        </tr>";
                                
                                foreach(var author in digest.Authors.OrderByDescending(a => a.PonderatedPercentage))
                                {
                                  html += @$"<tr>
                                                 <td>{author.Author.Name}</td>
                                                 <td>{author.Author.Email}</td>
                                                 <td>{author.Author.TotalCommitsWithOneParent} (<u>{author.PercentageTotalCommitsWithOneParent}%</u>)</td>
                                                 <td>{author.Author.LinesAdded} (<u>{author.PercentageLinesAdded}%</u>)</td>
                                                 <td>{author.Author.LinesDeleted} (<u>{author.PercentageLinesDeleted}%</u>)</td>
                                            </tr>";
                                }

                          html += @$"</table>
                                </section>
                                <section>
                                    <p>
                                            Report date: <i><small>{digest.DigestDateUTC:dd-MM-yy H:mm:ss}</small></i>
                                            This project is at <a href='{toolGitRepositoryUrl}'>GitHub</a>.
                                    </p>
                                </section >
                             </body>
                        </html>";

            return html;
        }
    }
}
