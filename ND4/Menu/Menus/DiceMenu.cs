namespace ND4.Menu.Menus
{
    using System;

    using EventArgs;

    internal class DiceMenu
    {
        public EventHandler<DicesEventArgs> DiceSelected;

        private int count = 3;

        public int Count
        {
            get
            {
                return this.count;
            }
            set
            {
                if (value < 1)
                {
                    return;
                }

                this.count = value;
            }
        }

        public void Display()
        {
            this.ShowCount();
            this.HandleKey();
        }

        private void HandleKey()
        {
            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.OemPlus:
                    case ConsoleKey.Add:
                        this.Count++;
                        this.ShowCount();
                        break;
                    case ConsoleKey.OemMinus:
                    case ConsoleKey.Subtract:
                        this.Count--;
                        this.ShowCount();
                        break;
                    case ConsoleKey.Enter:
                        this.DiceSelected?.Invoke(this, new DicesEventArgs(this.count));
                        return;
                }
            }
        }

        private void ShowCount()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Players will have " + this.Count + " dice ");
        }
    }
}