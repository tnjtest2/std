namespace ND4.EventArgs
{
    using System;

    internal class GameStartEventArgs : EventArgs
    {
        public GameStartEventArgs(int playersCount, int diceCount)
        {
            this.PlayersCount = playersCount;
            this.DiceCount = diceCount;
        }

        public int PlayersCount { get; }

        public int DiceCount { get; }
    }
}