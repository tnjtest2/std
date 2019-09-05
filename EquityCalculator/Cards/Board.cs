namespace EquityCalculator.Cards
{
    using System.Linq;

    internal struct Board
    {
        public Card[] Cards { get; }

        public Board(params Card[] cards)
        {
            this.Cards = cards;
        }

        public Card[] Concat(Hand hand)
        {
            return this.Cards.Concat(hand.Cards).ToArray();
        }

        public override string ToString()
        {
            return string.Join(", ", this.Cards);
        }
    }
}