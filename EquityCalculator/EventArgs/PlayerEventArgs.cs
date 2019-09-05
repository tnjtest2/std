namespace EquityCalculator.EventArgs
{
    using System;
    using System.Collections.Generic;

    using Players;

    internal sealed class PlayerEventArgs : EventArgs
    {
        public PlayerEventArgs(IReadOnlyList<Player> players)
        {
            this.Players = players;
        }

        public IReadOnlyList<Player> Players { get; }
    }
}