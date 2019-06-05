using System;
using System.Collections.Generic;
using System.Linq;
using PgnReader;

namespace pgn_query
{
    public class PgnGameMatcher
    {
        private readonly PgnGameResultComparer _resultComparer = new PgnGameResultComparer();
        private readonly StringComparer _stringComparer = new StringComparer();
        public bool MatchGame(PgnGame game, PgnGameFinderService.FindOptions options)
        {
            var stringComparisons = new List<Func<bool>>()
            {
                () => _stringComparer.Compare(game.Event.ToLower(), options.Event.ToLower()),
                () => _stringComparer.Compare(game.Site.ToLower(), options.Site.ToLower()),
                () => _stringComparer.Compare(game.Date.ToString().ToLower(), options.Date.ToLower()),
                () => _stringComparer.Compare(game.White.ToLower(), options.White.ToLower()),
                () => _stringComparer.Compare(game.Round.ToLower(), options.Round.ToLower()),
                () => _stringComparer.Compare(game.Black.ToLower(), options.Black.ToLower()),
            };

            return stringComparisons.All(c => c())
                   && _resultComparer.Compare(game.Result, options.Result.ToLower())
                ;
        }
    }
}