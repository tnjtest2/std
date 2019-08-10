namespace ND4.Menu.Menus
{
    using System;

    internal class MainMenu
    {
        public EventHandler DisplayPlayersMenu;

        public EventHandler Exit;

        public void Display()
        {
            Console.WriteLine("(P)lay");
            Console.WriteLine("(Q)uit");

            this.HandleKey();
        }

        private void HandleKey()
        {
            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.P:
                        this.DisplayPlayersMenu?.Invoke(this, EventArgs.Empty);
                        return;
                    case ConsoleKey.Q:
                        this.Exit?.Invoke(this, EventArgs.Empty);
                        return;
                }
            }
        }
    }
}