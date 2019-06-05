using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using pgn_tools.common;
using PgnReader;
using YACLAP;

namespace pgn_query
{
    class Program
    {
        static int Main(string[] args)
        {
            ConsoleHeader();

            var parser = new PgnQueryCLAP(args);

            if (parser.HasErrors)
            {
                OutputParserErrors(parser);
                return -1;
            }

            if (parser.Debug)
            {
                DebugHeader(parser);
            }

            if (parser.UseStdIn)
            {
                throw new NotImplementedException();
            }

            var gamesRead = 0;
            var filesRead = 0;
            var totalMatches = 0;
            var parserFileSources = parser.FileSources.ToList();
            var totalFiles = parserFileSources.Count;
            foreach (var parserFileSource in parserFileSources)
            {
                if(parser.Debug) Console.Write($"Parsing: {filesRead+1}/{totalFiles}");
                var games = PgnGame.ReadAllGamesFromFile(parserFileSource);

                if (parser.Debug) Console.WriteLine($" Games read: {games.Count()} | {parserFileSource}");
                else Console.Write(".");
                gamesRead += games.Count();
                filesRead++;

                var matchedGames = FindGames(games, parser);
                totalMatches += matchedGames.Count();
            }

            Console.WriteLine();
            Console.WriteLine($"Files read: {filesRead}");
            Console.WriteLine($"Games read: {gamesRead}");
            Console.WriteLine($"Matches found: {totalMatches}");
            return 0;
        }

        private static IEnumerable<PgnGame> FindGames(IEnumerable<PgnGame> games, PgnQueryCLAP parser)
        {
            return games.Where(g => MatchGame(g, parser));
        }

        private static bool MatchGame(PgnGame game, PgnQueryCLAP parser)
        {
            bool StringComparer(string g, string p)
            {
                switch (p)
                {
                    case string t when t.StartsWith("^"):
                        return g.StartsWith(p.Substring(1));
                    case string t when t.StartsWith("*"):
                        return g.Contains(p.Substring(1));
                    case string t when t.EndsWith("$"):
                        return g.EndsWith(p.Substring(0, p.Length -1));
                    default:
                        return !string.IsNullOrEmpty(p) && g.Equals(p);
                }
            }

            return StringComparer(game.Event.ToLower(), parser.Event.ToLower());
        }
        private static void DebugHeader(PgnQueryCLAP parser)
        {
            if (parser.UseStdIn)
            {
                Console.WriteLine($"Filesource: STDIN");
            }
        }

        private static void OutputParserErrors(PgnQueryCLAP parser)
        {
            Console.WriteLine("Command line error:");
            Console.WriteLine($"{parser.Errors.Select(e => e.Message).Aggregate((s, e) => s += e + "\n")}");
        }

        private static void ConsoleHeader()
        {
            Console.WriteLine(GetDisplayVersion());
        }

        private static string GetDisplayVersion()
        {
            var assemblyName =typeof(Program).Assembly.GetName();
            var version = assemblyName.Version.ToString();
            var name = assemblyName.Name;
            return $"{name} V{version}";
        }
    }

    public class PgnQueryCLAP : CommonParser
    {
        public string Event => SimpleParser.HasOption("event") ? SimpleParser.Option("event") : "";
        public PgnQueryCLAP(string[] args) : base(args)
        {
        }
    }
}
