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
            ConsoleHeader(parser);

            if (parser.HasErrors)
            {
                OutputParserErrors(parser);
                return -1;
            }

            var gamesRead = 0;
            var filesRead = 0;
            var totalMatches = 0;
            var parserFileSources = parser.FileSources.ToList();
            var totalFiles = parserFileSources.Count;
            

            parserFileSources.ForEach(parserFileSource =>
            {
                if (parser.Debug) Info(parser, $"Parsing: {filesRead + 1}/{totalFiles}\n");

                var games = PgnGame.ReadAllGamesFromFile(parserFileSource).ToList();

                Info(parser, parser.Debug 
                    ? $" Games read: {games.Count()} | {parserFileSource}\n" 
                    : ".");

                gamesRead += games.Count();
                filesRead++;

                var matchedGames = games.AsEnumerable().FindGames(parser).ToList();
                totalMatches += matchedGames.Count();
                if (!parser.CountMode)
                {
                    Writer.WriteLine(matchedGames.Aggregate("", (s, game) => s + ("\n" + game.PgnText)));
                }
            });

           Info(parser, "\n");
           Info(parser, $"Files read: {filesRead}\n");
           Info(parser, $"Games read: {gamesRead}\n");
           Info(parser, $"Matches found: {totalMatches}\n");
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
