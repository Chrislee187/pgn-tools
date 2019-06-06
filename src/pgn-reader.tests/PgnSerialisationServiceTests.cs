using System.Linq;
using NUnit.Framework;
using PgnReader.tests.TestData;

namespace PgnReader.tests
{
    [TestFixture]
    public class PgnSerialisationServiceTests
    {
        [Test]
        public void METHOD()
        {
            var svc = new PgnSerialisationService();

            var serializeAllGames = svc.SerializeAllGames(WikiGame.PgnText, true);


        }
    }
}