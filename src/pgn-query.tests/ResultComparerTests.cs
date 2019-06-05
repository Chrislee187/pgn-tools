using NUnit.Framework;
using pgn_query;
using PgnReader;

namespace Tests
{
    [TestFixture]
    public class ResultComparerTests
    {
        [TestCase("0-1")]
        [TestCase("b")]
        [TestCase("black")]
        [TestCase("B")]
        [TestCase("BLACK")]
        public void Matches_black_wins(string arg)
        {
            var comparer = new PgnGameResultComparer();

            Assert.True(comparer.Compare(PgnGameResult.BlackWins, arg));

        }
        [TestCase("1-0")]
        [TestCase("w")]
        [TestCase("white")]
        [TestCase("W")]
        [TestCase("WHITE")]
        public void Matches_white_wins(string arg)
        {
            var comparer = new PgnGameResultComparer();

            Assert.True(comparer.Compare(PgnGameResult.WhiteWins, arg));

        }

        [TestCase("1/2-1/2")]
        [TestCase("d")]
        [TestCase("draw")]
        [TestCase("D")]
        [TestCase("draw")]
        public void Matches_draws(string arg)
        {
            var comparer = new PgnGameResultComparer();

            Assert.True(comparer.Compare(PgnGameResult.Draw, arg));
        }
    }
}