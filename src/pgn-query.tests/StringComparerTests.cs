using NUnit.Framework;
using pgn_query;

namespace Tests
{
    [TestFixture]
    public class StringComparerTests
    {
        [TestCase("text", "text")]
        public void Matches_exact_strings(string text, string compare)
        {
            var comparer = new StringComparer();

            Assert.True(comparer.Compare(text, compare));

        }

        [TestCase("text", "te")]
        public void Matches_strings_starting_with(string text, string startsWith)
        {
            var comparer = new StringComparer();

            Assert.True(comparer.Compare(text, $"^{startsWith}"));

        }

        [TestCase("text", "xt")]
        public void Matches_strings_ending_with(string text, string endsWith)
        {
            var comparer = new StringComparer();

            Assert.True(comparer.Compare(text, $"{endsWith}$"));

        }

        [TestCase("text", "ex")]
        public void Matches_strings_containing(string text, string contains)
        {
            var comparer = new StringComparer();

            Assert.True(comparer.Compare(text, $"*{contains}"));

        }

    }
}