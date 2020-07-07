using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using CommandLine;
using CsvHelper;
using LibGit2Sharp;

namespace GitChangesSpreadsheetConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Arguments>(args).WithParsed<Arguments>(arguments =>
            {
                var repo = new Repository(arguments.InputDirectory);
                var repoStatus = repo.RetrieveStatus().OrderBy(i => i.FilePath);

                if (!Directory.Exists(arguments.OutputDirectory))
                {
                    Directory.CreateDirectory(arguments.OutputDirectory);
                }

                WriteToFile(repoStatus.Where(i => i.State == FileStatus.NewInWorkdir), Path.Join(arguments.OutputDirectory, "newitems.csv"));
                WriteToFile(repoStatus.Where(i => i.State == FileStatus.ModifiedInWorkdir), Path.Join(arguments.OutputDirectory,"updateditems.csv"));
                WriteToFile(repoStatus.Where(i => i.State == FileStatus.DeletedFromWorkdir), Path.Join(arguments.OutputDirectory,"deleteditems.csv"));
            });
        }

        static void WriteToFile(IEnumerable<StatusEntry> input, string fileName)
        {
            using (var file = File.Exists(fileName) ? File.Open(fileName, FileMode.Truncate) : File.Open(fileName, FileMode.OpenOrCreate))
            {
                using (var writer = new CsvWriter(new StreamWriter(file)))
                {
                    writer.WriteRecords(input.Select(i => new { item = i.FilePath, reason = "" }));
                }
            }
        }
    }
}
