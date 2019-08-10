namespace ND4.Menu.Menus
{
    using System;

    internal class ReplayMenu
    {
        public EventHandler DisplayMainMenu;

        public EventHandler Exit;

        public EventHandler Replay;

        public void Display()
        {
            Console.WriteLine();
            Console.WriteLine("(R)eplay");
            Console.WriteLine("(M)enu");
            Console.WriteLine("(Q)uit");

            this.HandleKey();
        }

        private void HandleKey()
        {
            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.R:
                        this.Replay?.Invoke(this, EventArgs.Empty);
                        return;
                    case ConsoleKey.Q:
                        this.Exit?.Invoke(this, EventArgs.Empty);
                        return;
                    case ConsoleKey.M:
                        this.DisplayMainMenu?.Invoke(this, EventArgs.Empty);
                        return;
                }
            }
        }
    }
}