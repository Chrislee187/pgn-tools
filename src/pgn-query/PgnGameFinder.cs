using System.Collections.Generic;
using System.Linq;
using PgnReader;

namespace pgn_query
{
    public static class PgnGameFinderExtensions {
        public static IEnumerable<PgnGame> FindGames(this IEnumerable<PgnGame> games, PgnGameFinderService.FindOptions options)
        {
            return games.Where(g => new PgnGameMatcher().MatchGame(g, options));
        }
    }
}