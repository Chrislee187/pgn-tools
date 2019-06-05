using System.Collections.Generic;
using System.Linq;

namespace PgnReader.Json
{
    public class PgnJson : Dictionary<string, string>
    {
        public string Moves { get; set; }

        public PgnJson(PgnGame game)
        {
            game.TagPairs.ToList().ForEach(tp => Add(tp.Name, tp.Value));

            Add("Moves", game.MoveText);

            Moves = game.MoveText;
        }
    }
}