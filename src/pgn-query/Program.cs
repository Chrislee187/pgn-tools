using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using PgnReader;

namespace pgn_query
{
    class Program
    {
        public static TextWriter Writer = Console.Out;
        public static TextWriter ErrorWriter = Console.Error;
        static int Main(string[] args)
        {
            var parser = new PgnQueryCommandLineParser(args);
            ConsoleHeader(parser);

            if (parser.HasErrors)
            {
                OutputParserErrors(parser);
                return -1;
            }

            var gamesRead = 0;
            var filesRead = 0;
            var parserFileSources = parser.FileSources.ToList();
            var totalFiles = parserFileSources.Count;
            
            var opts = new PgnGameFinderService.FindOptions(
                parserFileSources.ToArray(),
                parser.Debug,
                parser.CountMode,
                parser.Event,
                parser.Site,
                parser.Date,
                parser.Round,
                parser.White, parser.Black, parser.Result);

            var worker = new PgnGameFinderService();

            worker.OnFileRead += (sender, filename, games) =>
            {
                Info(parser, $" {games.Count()} Games read from: {filename}\n");
            };

            worker.OnMatchesFound += (sender, matched) =>
            {
                Info(parser, $" {matched.Count()} games matched.\n");

                if (!parser.CountMode)
                {
                    // NOTE: Won't be able to use this approach when sorting is implemented, will need the whole data-set before outputting
                    Writer.WriteLine(matched.Aggregate("", (s, game) => s + ("\n" + game.PgnText)));
                }
            };

            var totalMatches = worker.Find(opts);

           Info(parser, $"Total Matches found: {totalMatches.Count()}\n");

           return 0;
        }

        private static void Info(PgnQueryCommandLineParser parser, string value)
        {
            if (parser.CountMode) Writer.Write(value);
        }

        private static void OutputParserErrors(PgnQueryCommandLineParser parser)
        {
            ErrorWriter.WriteLine("Command line error:");
            ErrorWriter.WriteLine($"{parser.Errors.Select(e => e.Message).Aggregate((s, e) => s + (e + "\n"))}");
        }

        private static void ConsoleHeader(PgnQueryCommandLineParser parser)
        {
            Info(parser,$"{GetDisplayVersion()}\n");
        }

        private static string GetDisplayVersion()
        {
            var assemblyName =typeof(Program).Assembly.GetName();
            var version = assemblyName.Version.ToString();
            var name = assemblyName.Name;
            return $"{name} V{version}";
        }
    }
}
