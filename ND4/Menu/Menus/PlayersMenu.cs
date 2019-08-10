namespace ND4.Menu.Menus
{
    using System;
    using System.Collections.Generic;

    using EventArgs;

    using Items;

    internal class PlayersMenu
    {
        private const int MaxPlayers = 6;

        private const int PlayersPerLine = 3;

        public EventHandler<PlayersEventArgs> PlayersSelected;

        private readonly List<MenuPlayerItem> menuItems = new List<MenuPlayerItem>();

        private int selectedItemX;

        private int selectedItemY;

        public PlayersMenu()
        {
            for (int y = 0, nr = 2; y < MaxPlayers / PlayersPerLine; y++)
            {
                for (var x = 0; x < PlayersPerLine; x++, nr++)
                {
                    this.menuItems.Add(new MenuPlayerItem("P" + nr, 2 + (x * 5), y));
                }
            }
        }

        public int SelectedItemY
        {
            get
            {
                return this.selectedItemY;
            }
            private set
            {
                if (value < 0 || value >= MaxPlayers / PlayersPerLine)
                {
                    return;
                }

                this.SelectedItem.IsActive = false;
                this.selectedItemY = value;
                this.SelectedItem.IsActive = true;
            }
        }

        public int SelectedItemX
        {
            get
            {
                return this.selectedItemX;
            }
            private set
            {
                if (value < 0 || value >= PlayersPerLine)
                {
                    return;
                }

                this.SelectedItem.IsActive = false;
                this.selectedItemX = value;
                this.SelectedItem.IsActive = true;
            }
        }

        public MenuPlayerItem SelectedItem
        {
            get
            {
                var index = (this.SelectedItemY * PlayersPerLine) + this.SelectedItemX;
                return this.menuItems[index];
            }
        }

        public void Display()
        {
            Console.Clear();

            this.menuItems[0].IsActive = true;

            this.Render();
            this.HandleKey();
        }

        private void HandleKey()
        {
            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.LeftArrow:
                        this.SelectedItemX--;
                        break;
                    case ConsoleKey.RightArrow:
                        this.SelectedItemX++;
                        break;
                    case ConsoleKey.UpArrow:
                        this.SelectedItemY--;
                        break;
                    case ConsoleKey.DownArrow:
                        this.SelectedItemY++;
                        break;
                    case ConsoleKey.Enter:
                        var count = this.menuItems.IndexOf(this.SelectedItem) + 1;
                        this.PlayersSelected?.Invoke(this, new PlayersEventArgs(count));
                        return;
                }
            }
        }

        private void Render()
        {
            foreach (var menuItem in this.menuItems)
            {
                menuItem.Render();
            }
        }
    }
}