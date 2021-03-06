﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using PgnReader.tests.TestData;

namespace PgnReader.tests
{
    public class PgnGameReadingTests
    {
        [Test]
        public void Wiki_games_FromString_works()
        {
            var games = PgnGame.ReadAllGamesFromString(
                WikiGame.PgnText + "\n" +
                WikiGame.PgnText).ToList();

            AssertWikiGameIsCorrect(games[0]);
            AssertWikiGameIsCorrect(games[1]);
        }
        
        [Test]
        public void Wiki_games_FromFile_works()
        {
            var reader = PgnGame.ReadAllGamesFromString(File.ReadAllText(@".\TestData\wikigame.pgn"));

            AssertWikiGame(reader);
        }


        private static void AssertWikiGame(IEnumerable<PgnGame> games)
        {
            Assert.True(games.All(AssertWikiGameIsCorrect));

            Assert.That(games.Count, Is.EqualTo(2), "Unexpected number of games read.");
        }
        private static bool AssertWikiGameIsCorrect(PgnGame pgnGame)
        {
            Assert.That(pgnGame.Event, Is.EqualTo($"F/S Return Match"));
            Assert.That(pgnGame.Site, Is.EqualTo("Belgrade, Serbia JUG"));
            Assert.That(pgnGame.Date.ToString(), Is.EqualTo("1992.11.04"));
            StringAssert.StartsWith("29", pgnGame.Round);
            Assert.That(pgnGame.White, Is.EqualTo("Fischer, Robert J."));
            Assert.That(pgnGame.Black, Is.EqualTo("Spassky, Boris V."));
            Assert.That(pgnGame.Result, Is.EqualTo(PgnGameResult.Draw));

            Assert.That(pgnGame.Turns.Count(), Is.EqualTo(43));
            Assert.That(pgnGame.Turns.First().Turn, Is.EqualTo(1));
            Assert.That(pgnGame.Turns.Last().Turn, Is.EqualTo(43));

            return true;
        }
    }
}