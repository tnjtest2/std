namespace EquityCalculator.Cards
{
    using System.Collections.Generic;
    using System.Linq;

    using Enums;

    internal struct StructuredBoardCards
    {
        public StructuredBoardCards(Card[] cards)
        {
            this.Cards = cards.OrderByDescending(x => x.Rank).ToArray();

            this.lazySuitGroup = null;

            //lazy rank group not worth it
            this.RankGroup = cards.GroupBy(x => x.Rank).OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.ToArray());

            this.RankHash = cards.Select(x => (double)x.Rank).Aggregate((sum, x) => sum * x);
            this.SuitHash = cards.Select(x => (double)x.Suit).Aggregate((sum, x) => sum * x);
        }

        private Dictionary<CardSuit, Card[]> lazySuitGroup;

        public Dictionary<CardSuit, Card[]> SuitGroup
        {
            get
            {
                if (this.lazySuitGroup == null)
                {
                    this.lazySuitGroup = this.Cards.GroupBy(x => x.Suit)
                        .ToDictionary(x => x.Key, x => x.OrderByDescending(z => z.Rank).ToArray());
                }

                return this.lazySuitGroup;
            }
        }

        public override string ToString()
        {
            return string.Join(", ", this.Cards);
        }

        public Dictionary<CardRank, Card[]> RankGroup { get; }

        public double RankHash { get; }

        public double SuitHash { get; }

        public Card[] Cards { get; }
    }
}