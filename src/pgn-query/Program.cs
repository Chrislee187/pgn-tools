using System;
using System.Collections.Generic;
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
            var opts = CreateOptions(parser);


            if (parser.HasErrors)
            {
                OutputParserErrors(parser);
                return -1;
            }

            ConsoleHeaderForCountMode(parser);
  

            var worker = new PgnGameFinderService();

            worker.OnFileRead += (sender, filename, games) =>
            {
                OutputForCountMode(parser, $" {games.Count()} Games read from: {filename}\n");
            };

            worker.OnMatchesFound += (sender, matched) =>
            {
                var pgnGames = matched.ToList();
                if(!OutputForCountMode(parser, $" {pgnGames.Count()} games matched.\n"))
                {
                    OutputPgnFiles(pgnGames);
                }
            };

            var totalMatches = worker.Find(opts);

           OutputForCountMode(parser, $"Total Matches found: {totalMatches.Count()}\n");

           return 0;
        }

        private static void OutputPgnFiles(List<PgnGame> pgnGames)
        {
            Writer.WriteLine(pgnGames.Aggregate("", (s, game) => s + ("\n" + game.PgnText)));
        }

        private static bool OutputForCountMode(PgnQueryCommandLineParser parser, string value)
        {
            if (parser.CountMode)
            {
                Writer.Write(value);
            }

            return parser.CountMode;
        }

        private static void OutputParserErrors(PgnQueryCommandLineParser parser)
        {
            ErrorWriter.WriteLine("Command line error:");
            ErrorWriter.WriteLine($"{parser.Errors.Select(e => e.Message).Aggregate((s, e) => s + (e + "\n"))}");
        }

        private static PgnGameFinderService.FindOptions CreateOptions(PgnQueryCommandLineParser parser)
        {
            var opts = new PgnGameFinderService.FindOptions(
                parser.FileSources,
                parser.Debug,
                parser.CountMode,
                parser.Event,
                parser.Site,
                parser.Date,
                parser.Round,
                parser.White, parser.Black, parser.Result);
            return opts;
        }

        private static void ConsoleHeaderForCountMode(PgnQueryCommandLineParser parser)
        {
            OutputForCountMode(parser,$"{GetDisplayVersion<Program>()}\n");
            OutputForCountMode(parser,$"{GetDisplayVersion<PgnGame>()}\n");
        }

        private static string GetDisplayVersion<T>(string name = "")
        {
            var assemblyName =typeof(T).Assembly.GetName();
            var version = assemblyName.Version.ToString();
            name = string.IsNullOrEmpty(name) ? assemblyName.Name : name;
            return $"{name} V{version}";
        }
    }
}
