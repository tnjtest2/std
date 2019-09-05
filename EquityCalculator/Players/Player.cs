namespace EquityCalculator.Players
{
    using System.Collections.Generic;
    using System.Threading;

    using Cards;

    internal class Player
    {
        private int gamesTied;

        private int gamesWon;

        public Player(string name, Hand hand)
        {
            this.Name = name;
            this.Hand = hand;
        }

        public IReadOnlyCollection<Card> Cards
        {
            get
            {
                return this.Hand.Cards;
            }
        }

        public Hand Hand { get; }

        public string Name { get; }

        public void GameTied(float playersCount)
        {
            var value = (int)(100 / playersCount);
            Interlocked.Add(ref this.gamesTied, value);
        }

        public void GameWon()
        {
            Interlocked.Increment(ref this.gamesWon);
        }

        public float GetEquity(float gamesPlayed)
        {
            var wins = (this.gamesWon / gamesPlayed) * 100;
            var ties = this.gamesTied / gamesPlayed;

            return wins + ties;
        }
    }
}