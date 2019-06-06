using System;
using System.Collections.Generic;
using System.Linq;
using PgnReader;

namespace pgn_query
{
    public class PgnGameMatcher
    {
        private readonly IStringComparer _stringComparer;
        private readonly IPgnGameResultComparer _pgnGameResultComparer;

        public PgnGameMatcher(IStringComparer stringComparer = null, IPgnGameResultComparer pgnGameResultComparer = null)
        {
            _stringComparer = stringComparer ?? new StringComparer();
            _pgnGameResultComparer = pgnGameResultComparer ?? new PgnGameResultComparer();
        }
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
                   && _pgnGameResultComparer.Compare(game.Result, options.Result.ToLower())
                ;
        }
    }
}