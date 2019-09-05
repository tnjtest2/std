namespace EquityCalculator.Players
{
    using Cards;

    using Enums;

    internal struct PlayerCombination
    {
        public Card[] Cards;

        public HandCombination Combination;

        public bool IsSuccessful;

        public PlayerCombination(Card[] cards, HandCombination combination)
        {
            this.Cards = cards;
            this.Combination = combination;
            this.IsSuccessful = true;
        }

        public override string ToString()
        {
            return string.Join(", ", this.Cards);
        }

        public static PlayerCombination Failed
        {
            get
            {
                return new PlayerCombination();
            }
        }
    }
}