namespace EquityCalculator.Data
{
    using System;

    using Enums;

    internal static class GameData
    {
        static GameData()
        {
            SuitsCount = Enum.GetValues(typeof(CardSuit)).Length;
            CardsCount = Enum.GetValues(typeof(CardRank)).Length;
            TotalCardsCount = CardsCount * SuitsCount;
            BoardCardsCount = 5;
            PlayerCardsCount = 2;
        }

        public static int BoardCardsCount { get; }

        public static int CardsCount { get; }

        public static int PlayerCardsCount { get; }

        public static int SuitsCount { get; }

        public static int TotalCardsCount { get; }
    }
}