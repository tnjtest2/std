namespace EquityCalculator.Cards
{
    internal struct Hand
    {
        public Card[] Cards { get; }

        public Hand(Card[] cards)
        {
            this.Cards = cards;
        }

        public override string ToString()
        {
            return string.Join(", ", this.Cards);
        }
    }
}