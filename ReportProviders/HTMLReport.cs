using GitStats.Model;
using System.Linq;

namespace GitStats.ReportProviders
{
    internal static class HTMLReport
    {
        public static string CreateReport(Digest digest)
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
                                    <p><strong>Total Commits:</strong> {digest.TotalCommits}</p>
                                    <p><strong>Total Lines Added:</strong> {digest.TotalLinesAdded}</p>
                                    <p><strong>Total Lines Deleted:</strong> {digest.TotalLinesDeleted}</p>
                                </section>
                                <section>
                                    <table>
                                        <tr>
                                            <th>Nome</th>
                                            <th>Email</th>
                                            <th>Total Commits</th>
                                            <th>Total Lines Added</th>
                                            <th>Total Lines Deleted</th>
                                        </tr>";
                                
                                foreach(var author in digest.Authors.OrderByDescending(a => a.PonderatedPercentage))
                                {
                                  html += @$"<tr>
                                                 <td>{author.Author.Name}</td>
                                                 <td>{author.Author.Email}</td>
                                                 <td>{author.Author.LinesAdded} (<u>{author.PercentageLinesAdded}%</u>)</td>
                                                 <td>{author.Author.LinesDeleted} (<u>{author.PercentageLinesDeleted}%</u>)</td>
                                                 <td>{author.Author.TotalCommits} (<u>{author.PercentageTotalCommits}%</u>)</td>
                                            </tr>";
                                }

                          html += @$"</table>
                                </section>
                                <section>
                                    <p>Report date: <i><small>{digest.DigestDateUTC:dd-MM-yy H:mm:ss}</small></i></p>
                                </section >
                             </body>
                        </html>";

            return html;
        }
    }
}
