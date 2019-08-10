namespace ND4.EventArgs
{
    using System;

    internal class DicesEventArgs : EventArgs
    {
        public DicesEventArgs(int count)
        {
            this.DiceCount = count;
        }

        public int DiceCount { get; }
    }
}