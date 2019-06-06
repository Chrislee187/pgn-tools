using System.Linq;
using PgnReader;

namespace pgn_query
{
    public interface IPgnGameResultComparer
    {
        bool Compare(PgnGameResult result, string comparison);
    }

    public class PgnGameResultComparer : IPgnGameResultComparer
    {
        public bool Compare(PgnGameResult result, string comparison)
        {
            if (string.IsNullOrEmpty(comparison)) return true;
            comparison = comparison.ToLower();
            return result == PgnGameResult.BlackWins && new[] { "0-1", "b", "black" }.Any(c => c.Equals(comparison))
                   || result == PgnGameResult.WhiteWins && new[] { "1-0", "w", "white" }.Any(c => c.Equals(comparison))
                   || result == PgnGameResult.Draw && new[] { "1/2-1/2", "d", "draw" }.Any(c => c.Equals(comparison))
                ;
        }
    }
}