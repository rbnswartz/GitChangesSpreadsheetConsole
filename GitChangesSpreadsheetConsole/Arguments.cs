using System;
using CommandLine.Text;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace GitChangesSpreadsheetConsole
{
    public class Arguments
    {
        [Option("input", Required = true, HelpText = "Source git repository directory")]
        public string InputDirectory { get; set; }
        [Option("output", Default = "output", HelpText = "Output directory for the csv files")]
        public string OutputDirectory { get; set; }
    }
}
