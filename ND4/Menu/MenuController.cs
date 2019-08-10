namespace ND4.Menu
{
    using System;

    using EventArgs;

    using Menus;

    internal class MenuController
    {
        public EventHandler<GameStartEventArgs> GameStart;

        private readonly DiceMenu diceMenu;

        private readonly MainMenu mainMenu;

        private readonly PlayersMenu playersMenu;

        private readonly ReplayMenu replayMenu;

        private int diceCount;

        private int playersCount;

        public MenuController()
        {
            Console.CursorVisible = false;

            this.mainMenu = new MainMenu();
            this.playersMenu = new PlayersMenu();
            this.diceMenu = new DiceMenu();
            this.replayMenu = new ReplayMenu();

            this.mainMenu.Exit += this.OnExit;
            this.mainMenu.DisplayPlayersMenu += this.OnDisplayPlayersMenu;
            this.playersMenu.PlayersSelected += this.OnPlayersSelected;
            this.diceMenu.DiceSelected += this.OnDiceSelected;
            this.replayMenu.Exit += this.OnExit;
            this.replayMenu.DisplayMainMenu += this.OnDisplayMainMenu;
            this.replayMenu.Replay += this.OnReplay;
        }

        public void Display()
        {
            this.mainMenu.Display();
        }

        public void DisplayReplayMenu()
        {
            this.replayMenu.Display();
        }

        private void OnDiceSelected(object sender, DicesEventArgs e)
        {
            Console.Clear();
            this.diceCount = e.DiceCount;
            this.GameStart?.Invoke(this, new GameStartEventArgs(this.playersCount, this.diceCount));
        }

        private void OnDisplayMainMenu(object sender, EventArgs e)
        {
            Console.Clear();
            this.Display();
        }

        private void OnDisplayPlayersMenu(object sender, EventArgs e)
        {
            Console.Clear();
            this.playersMenu.Display();
        }

        private void OnExit(object sender, EventArgs e)
        {
            Console.WriteLine("Exiting...");
            Environment.Exit(0);
        }

        private void OnPlayersSelected(object sender, PlayersEventArgs e)
        {
            Console.Clear();
            this.playersCount = e.PlayersCount;
            this.diceMenu.Display();
        }

        private void OnReplay(object sender, EventArgs e)
        {
            Console.Clear();
            this.GameStart?.Invoke(this, new GameStartEventArgs(this.playersCount, this.diceCount));
        }
    }
}