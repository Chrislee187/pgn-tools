using System.Collections.Generic;
using System.Linq;
using PgnReader;

namespace pgn_query
{
    public class PgnGameFinderService
    {
        public delegate void MatchesFound(object sender, IEnumerable<PgnGame> matched);
        public delegate void FileRead(object sender, string filename, IEnumerable<PgnGame> games);
        public event MatchesFound OnMatchesFound;
        public event FileRead OnFileRead;

        public IEnumerable<PgnGame> Find(FindOptions options)
        {
            var results = new List<PgnGame>();
            foreach (var fileSource in options.FileSources)
            {
                var games = PgnGame.ReadAllGamesFromFile(fileSource).ToList();
                OnFileRead?.Invoke(this, fileSource, games);

                var matchedGames = games.AsEnumerable().FindGames(options).ToList();
                OnMatchesFound?.Invoke(this, matchedGames);

                results.AddRange(matchedGames);
            }

            return results;
        }

        public class FindOptions
        {
            public IEnumerable<string> FileSources { get; }
            public bool Debug { get; }
            public bool CountMode { get; }
            public string Event { get; }
            public string Site { get; }
            public string Date { get; }
            public string Round { get; }
            public string White { get; }
            public string Black { get; }
            public string Result { get; }

            public FindOptions(string[] fileSources,
                string @event, string site, string date, string round, string white,
                string black, string result,
                bool countMode,
                bool debug)
            {
                FileSources = fileSources;
                Debug = debug;
                CountMode = countMode;
                Event = @event;
                Site = site;
                Date = date;
                Round = round;
                White = white;
                Black = black;
                Result = result;
            }
        }
    }
}