using System.Linq;
using pgn_tools.common;

namespace pgn_query
{
    public class PgnQueryCommandLineParser : CommonParser
    {
        public bool CountMode => SimpleParser.HasFlag("count");

        public string Event => SimpleParser.HasOption("event") ? SimpleParser.Option("event") : "";
        public string Site => SimpleParser.HasOption("site") ? SimpleParser.Option("site") : "";
        public string Date => SimpleParser.HasOption("date") ? SimpleParser.Option("date") : "";
        public string Round => SimpleParser.HasOption("round") ? SimpleParser.Option("round") : "";
        public string White => SimpleParser.HasOption("white") ? SimpleParser.Option("white") : "";
        public string Black => SimpleParser.HasOption("black") ? SimpleParser.Option("black") : "";
        public string Result => SimpleParser.HasOption("result") ? SimpleParser.Option("event") : "";

        public PgnQueryCommandLineParser(string[] args) : base(args)
        {
        }
    }
}