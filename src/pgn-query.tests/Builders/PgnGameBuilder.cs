using System.Linq;
using PgnReader;

namespace pgn
{
    public class PgnGameBuilder
    {
        private string _event = "Game Event";
        private string _site = "Game Site";
        private string _date = "2019.01.01";
        private string _round = "1";
        private string _white = "White player";
        private string _black = "Black Player";
        private string _result = "1/2-1/2";
        private const string PgnText = "[Event \"{Event}\"]\n" +
                                       "[Site \"{Site}\"]\n" +
                                       "[Date \"{Date}\"]\n" +
                                       "[Round \"{Round}\"]\n" +
                                       "[White \"{White}\"]\n" +
                                       "[Black \"{Black}\"]\n" +
                                       "[Result \"{Result}\"]\n\n" +
                                       "1. e4 e5 2. Nf3 Nc6 3. Bb5 a6 { This opening is called the Ruy Lopez.}\n" +
                                       "4. Ba4 Nf6 5. O-O Be7 6. Re1 b5 7. Bb3 d6 8. c3 O-O 9. h3 Nb8 10. d4 Nbd7\n" +
                                       "11. c4 c6 12. cxb5 axb5 13. Nc3 Bb7 14. Bg5 b4 15. Nb1 h6 16. Bh4 c5 17. dxe5\n" +
                                       "Nxe4 18. Bxe7 Qxe7 19. exd6 Qf6 20. Nbd2 Nxd6 21. Nc4 Nxc4 22. Bxc4 Nb6\n" +
                                       "23. Ne5 Rae8 24. Bxf7+ Rxf7 25. Nxf7 Rxe1+ 26. Qxe1 Kxf7 27. Qe3 Qg5 28. Qxg5\n" +
                                       "hxg5 29. b3 Ke6 30. a3 Kd6 31. axb4 cxb4 32. Ra5 Nd5 33. f3 Bc8 34. Kf2 Bf5\n" +
                                       "35. Ra7 g6 36. Ra6+ Kc5 37. Ke1 Nf4 38. g3 Nxh3 39. Kd2 Kb5 40. Rd6 Kc5 41. Ra6\n" +
                                       "Nf2 42. g4 Bd3 43. Re6 1/2-1/2\n";

        public PgnGame Build()
        {
            return PgnGame.ReadAllGamesFromString(
                PgnText
                    .Replace("{Event}", _event)
                    .Replace("{Site}", _site)
                    .Replace("{Date}", _date)
                    .Replace("{Round}", _round)
                    .Replace("{White}", _white)
                    .Replace("{Black}", _black)
                    .Replace("{Result}", _result)
            ).Single();
        }
    }
}