using NUnit.Framework;
using pgn_query;

namespace Tests
{
    public class PgnQueryCommandLineParserTests
    {
        private const string ExpectedValue = "aValue";

        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void Defaults_are_as_expected()
        {
            var args = new string[0];

            var parser = new PgnQueryCommandLineParser(args);

            Assert.False(parser.CountMode);
            Assert.False(parser.Debug);
            Assert.False(parser.HasErrors);
            Assert.That(parser.Event, Is.EqualTo(string.Empty));
            Assert.That(parser.Site, Is.EqualTo(string.Empty));
            Assert.That(parser.Date, Is.EqualTo(string.Empty));
            Assert.That(parser.Round, Is.EqualTo(string.Empty));
            Assert.That(parser.White, Is.EqualTo(string.Empty));
            Assert.That(parser.Black, Is.EqualTo(string.Empty));
            Assert.That(parser.Result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Count_flag_is_supported()
        {
            var args = new[] {"--count"};
        
            var parser = new PgnQueryCommandLineParser(args);

            Assert.True(parser.CountMode);
        }

        [Test]
        public void Debug_flag_is_supported()
        {
            var args = new[] { "--debug" };

            var parser = new PgnQueryCommandLineParser(args);

            Assert.True(parser.Debug);
        }

        [Test]
        public void Event_option_is_supported()
        {
            var args = new[] { "--event", ExpectedValue };

            var parser = new PgnQueryCommandLineParser(args);

            Assert.That(parser.Event, Is.EqualTo(ExpectedValue));
        }

        [Test]
        public void Site_option_is_supported()
        {
            var args = new[] { "--site", ExpectedValue };

            var parser = new PgnQueryCommandLineParser(args);

            Assert.That(parser.Site, Is.EqualTo(ExpectedValue));
        }

        [Test]
        public void Date_option_is_supported()
        {
            var args = new[] { "--date", ExpectedValue };

            var parser = new PgnQueryCommandLineParser(args);

            Assert.That(parser.Date, Is.EqualTo(ExpectedValue));
        }

        [Test]
        public void Round_option_is_supported()
        {
            var args = new[] { "--round", ExpectedValue };

            var parser = new PgnQueryCommandLineParser(args);

            Assert.That(parser.Round, Is.EqualTo(ExpectedValue));
        }

        [Test]
        public void White_option_is_supported()
        {
            var args = new[] { "--white", ExpectedValue };

            var parser = new PgnQueryCommandLineParser(args);

            Assert.That(parser.White, Is.EqualTo(ExpectedValue));
        }

        [Test]
        public void Black_option_is_supported()
        {
            var args = new[] { "--black", ExpectedValue };

            var parser = new PgnQueryCommandLineParser(args);

            Assert.That(parser.Black, Is.EqualTo(ExpectedValue));
        }

        [Test]
        public void Result_option_is_supported()
        {
            var args = new[] { "--result", ExpectedValue };

            var parser = new PgnQueryCommandLineParser(args);

            Assert.That(parser.Result, Is.EqualTo(ExpectedValue));
        }
        [Test]
        public void Json_flag_is_supported()
        {
            var args = new[] { "--json" };

            var parser = new PgnQueryCommandLineParser(args);

            Assert.True(parser.Json);
        }
    }
}