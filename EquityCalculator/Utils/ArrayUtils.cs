namespace EquityCalculator.Utils
{
    using System;
    using System.Linq;

    using Cards;

    internal static class ArrayUtils
    {
        public static double GetRankHash(this Card[] cards)
        {
            return cards.Select(x => (double)x.Rank).Aggregate((hash, x) => hash * x);
        }

        public static void Print(this Card[] cards)
        {
            Console.WriteLine(string.Join(", ", cards));
        }
    }
}