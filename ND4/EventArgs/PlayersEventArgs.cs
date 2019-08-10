namespace ND4.EventArgs
{
    using System;

    internal class PlayersEventArgs : EventArgs
    {
        public PlayersEventArgs(int count)
        {
            this.PlayersCount = count;
        }

        public int PlayersCount { get; }
    }
}