using System;
using System.Collections.Generic;
using System.Linq;
using PgnReader;

namespace pgn_query
{
    public static class PgnGameFinder {
        public static IEnumerable<PgnGame> FindGames(this IEnumerable<PgnGame> games, PgnQueryCommandLineParser parser)
        {
            return games.Where(g => MatchGame(g, parser));
        }

        private static bool MatchGame(PgnGame game, PgnQueryCommandLineParser parser)
        {
            var stringComparisons = new List<Func<bool>>()
            {
                () => StringComparer(game.Event.ToLower(), parser.Event.ToLower()),
                () => StringComparer(game.Site.ToLower(), parser.Site.ToLower()),
                () => StringComparer(game.Date.ToString().ToLower(), parser.Date.ToString().ToLower()),
                () => StringComparer(game.White.ToLower(), parser.White.ToLower()),
                () => StringComparer(game.Black.ToLower(), parser.Black.ToLower()),
            };

            return stringComparisons.All(c => c())
                   && ResultComparer(game.Result, parser.Result.ToLower())
                ;
        }

        private static bool ResultComparer(PgnGameResult g, string p)
        {
            if (string.IsNullOrEmpty(p)) return true;

            return g == PgnGameResult.BlackWins && (p == "0-1" || p == "b") || g == PgnGameResult.WhiteWins && (p == "1-0" || p == "w") || g == PgnGameResult.Draw && (p == "1/2-1/2" || p == "d");
        }

        private static bool StringComparer(string g, string p)
        {
            if (string.IsNullOrEmpty(p)) return true;
            switch (p)
            {
                case string t when t.StartsWith("^"):
                    return g.StartsWith(p.Substring(1));
                case string t when t.StartsWith("*"):
                    return g.Contains(p.Substring(1));
                case string t when t.EndsWith("$"):
                    return g.EndsWith(p.Substring(0, p.Length - 1));
                default:
                    return !string.IsNullOrEmpty(p) && g.Equals(p);
            }
        }

    }
}