using GitStats.ReportProviders;
using Microsoft.Extensions.CommandLineUtils;
using System;
using System.IO;

namespace GitStats
{
    class Program
    {
        private static readonly string _htmlReportFileName = "gitstatsreport.html";
        private static readonly string _jsonReportFileName = "gitstatsreport.json";
        private static readonly string _toolGitRepositoryUrl = "https://github.com/davidvalenteam/gitstats";

        static void Main(string[] args)
        {
            var app = new CommandLineApplication
            {
                Name = "ConsoleArgs"
            };

            app.HelpOption("-?|-h|--help");

            var htmlOption = app.Option("-html",
                    "Genrates HTML report",
                    CommandOptionType.NoValue);

            var jsonOption = app.Option("-json",
                    "Genrates JSON report",
                    CommandOptionType.NoValue);

            var inputFolderOption = app.Option("-i",
                    "Input folder with the Git repository",
                    CommandOptionType.SingleValue);

            app.OnExecute(() =>
            {
                if (inputFolderOption.HasValue() && !Directory.Exists(inputFolderOption.Value()))
                {
                    Console.WriteLine($"-i - Folder does not exist: {inputFolderOption.Value()}");
                    return 0;
                }

                var gitFolder = inputFolderOption.HasValue() ? inputFolderOption.Value() : Directory.GetCurrentDirectory();

                if (!Directory.Exists(Path.Combine(gitFolder, ".git")))
                {
                    Console.WriteLine($"I couldn't find a .git folder. Please use -h for more options.");
                    return 0;
                }

                Console.WriteLine($"I found a git folder at '{gitFolder}'. I will do the job, please wait...");

                try
                {
                    var digest =  Task.Execute(gitFolder);

                    var jsonReport = JSONReport.CreateReport(digest);

                    if (htmlOption.HasValue())
                    {
                        var filePath = Path.Combine(gitFolder, _htmlReportFileName);
                        File.WriteAllText(filePath, HTMLReport.CreateReport(digest, _toolGitRepositoryUrl));
                        Console.WriteLine($"HTML report created at {filePath}");
                    }

                    if (jsonOption.HasValue())
                    {
                        var filePath = Path.Combine(gitFolder, _jsonReportFileName);
                        File.WriteAllText(filePath, jsonReport);
                        Console.WriteLine($"JSON report created at {filePath}");
                    }

                    if (!jsonOption.HasValue() && !htmlOption.HasValue())
                    {
                        Console.WriteLine(jsonReport);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"Fatal error, I am terribly sorry... Please visit my GitHub repository ({_toolGitRepositoryUrl}) and leave your feedback, thanks.");
                }

                return 0;
            });

            app.Execute(args);
        }
    }
}
