using GitStats.Model;
using System.Text.Json;

namespace GitStats.ReportProviders
{
    internal static class JSONReport
    {
        public static string CreateReport(Digest digest) =>
            JsonSerializer.Serialize(digest, new JsonSerializerOptions { WriteIndented = true });
    }
}
