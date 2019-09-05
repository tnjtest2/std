namespace EquityCalculator.Cards
{
    using System;
    using System.Collections.Generic;

    using Enums;

    internal struct Card : IEquatable<Card>
    {
        public CardRank Rank { get; }

        public CardSuit Suit { get; }

        public Card(CardRank rank, CardSuit suit)
        {
            this.Rank = rank;
            this.Suit = suit;
        }

        public static Dictionary<CardRank, char> RankAbbreviations = new Dictionary<CardRank, char>
        {
            { CardRank.Two, '2' },
            { CardRank.Three, '3' },
            { CardRank.Four, '4' },
            { CardRank.Five, '5' },
            { CardRank.Six, '6' },
            { CardRank.Seven, '7' },
            { CardRank.Eight, '8' },
            { CardRank.Nine, '9' },
            { CardRank.Ten, 'T' },
            { CardRank.Jack, 'J' },
            { CardRank.Queen, 'Q' },
            { CardRank.King, 'K' },
            { CardRank.Ace, 'A' },
        };

        public static Dictionary<CardSuit, char> SuitAbbreviations = new Dictionary<CardSuit, char>
        {
            { CardSuit.Diamond, '♦' },
            { CardSuit.Spade, '♠' },
            { CardSuit.Club, '♣' },
            { CardSuit.Hearth, '♥' },
        };

        public override string ToString()
        {
            return string.Concat(RankAbbreviations[this.Rank], SuitAbbreviations[this.Suit]);
        }

        public bool Equals(Card other)
        {
            return this.Rank == other.Rank && this.Suit == other.Suit;
        }

        public override int GetHashCode()
        {
            return (int)this.Suit * (int)this.Rank;
        }
    }
}