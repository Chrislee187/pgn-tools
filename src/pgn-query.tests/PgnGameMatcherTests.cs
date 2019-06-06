using System;
using Moq;
using NUnit.Framework;
using pgn;
using pgn_query;
using PgnReader;

namespace Tests
{
    [TestFixture]
    public class PgnGameMatcherTests
    {
        private Mock<IStringComparer> _stringComparerMock;
        private Mock<IPgnGameResultComparer> _resultComparerMock;
        private PgnGame _pgnGame;
        private PgnGameFinderService.FindOptions _findOptions;
        private PgnGameMatcher _matcher;

        [SetUp]
        public void SetUp()
        {
            _stringComparerMock = new Mock<IStringComparer>();
            SetupStringComparisonResult(true);
            
            _resultComparerMock = new Mock<IPgnGameResultComparer>();
            SetupPgnResultComparisonResult(true);

            _matcher = new PgnGameMatcher(_stringComparerMock.Object, _resultComparerMock.Object);

            _pgnGame = new PgnGameBuilder().Build();

            _findOptions = new PgnGameFinderService.FindOptions(null, false, false,
                "event compare",
                "site compare",
                "2019.0.01",
                "1",
                "white compare",
                "black compare",
                "1/2-1/2");
        }

        [Test]
        public void MatchGame_compares_the_standard_six_string_tags()
        {

            _matcher.MatchGame(_pgnGame, _findOptions);

            VerifyStringComparerWasCalledWith(s => s == _pgnGame.Event.ToLower(), c => c == _findOptions.Event.ToLower());
            VerifyStringComparerWasCalledWith(s => s == _pgnGame.Site.ToLower(), c => c == _findOptions.Site.ToLower());
            VerifyStringComparerWasCalledWith(s => s == _pgnGame.Date.ToString().ToLower(), c => c == _findOptions.Date.ToLower());
            VerifyStringComparerWasCalledWith(s => s == _pgnGame.Round.ToLower(), c => c == _findOptions.Round.ToLower());
            VerifyStringComparerWasCalledWith(s => s == _pgnGame.White.ToLower(), c => c == _findOptions.White.ToLower());
            VerifyStringComparerWasCalledWith(s => s == _pgnGame.Black.ToLower(), c => c == _findOptions.Black.ToLower());
        }

        [Test]
        public void MatchGame_compares_the_result_tag()
        {
            _matcher.MatchGame(_pgnGame, _findOptions);

            VerifyPgnResultComparerWasCalled();
        }

        [Test]
        public void MatchGame_returns_false_when_string_comparison_fails()
        {
            SetupStringComparisonResult(false);

            Assert.False(_matcher.MatchGame(_pgnGame, _findOptions));
        }

        [Test]
        public void MatchGame_returns_false_when_PgnGameResult_comparison_fails()
        {
            SetupPgnResultComparisonResult(false);

            Assert.False(_matcher.MatchGame(_pgnGame, _findOptions));
        }

        [Test]
        public void MatchGame_returns_true_when_all_comparisons_succeed()
        {
            Assert.True(_matcher.MatchGame(_pgnGame, _findOptions));
        }

        private void SetupPgnResultComparisonResult(bool value) => 
            _resultComparerMock.Setup(c => c.Compare(
                    It.IsAny<PgnGameResult>(), 
                    It.IsAny<string>()))
                .Returns(value);

        private void VerifyPgnResultComparerWasCalled() =>
            _resultComparerMock.Verify(m => m.Compare(
                It.IsAny<PgnGameResult>(),
                It.IsAny<string>()
            ), Times.Once);

        private void SetupStringComparisonResult(bool value) => 
            _stringComparerMock.Setup(c => c.Compare(
                    It.IsAny<string>(), 
                    It.IsAny<string>()))
                .Returns(value);

        private void VerifyStringComparerWasCalledWith(Func<string, bool> pgnGameField, Func<string, bool> optionsField) =>
            _stringComparerMock.Verify(m => m.Compare(
                It.Is<string>(s => pgnGameField(s)),
                It.Is<string>(s => optionsField(s))
            ), Times.Once);
    }
}