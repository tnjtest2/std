namespace ND4.Game
{
    using System;

    using EventArgs;

    using Menu;

    internal class GameController
    {
        private readonly Game game;

        private readonly MenuController menuController;

        public GameController()
        {
            this.game = new Game();
            this.menuController = new MenuController();

            this.menuController.GameStart += this.OnGameStart;
            this.game.GameEnd += this.OnGameEnd;

            this.menuController.Display();
        }

        private void OnGameEnd(object sender, EventArgs e)
        {
            this.menuController.DisplayReplayMenu();
        }

        private void OnGameStart(object sender, GameStartEventArgs e)
        {
            this.game.StartGame(e.PlayersCount + 1, e.DiceCount);
        }
    }
}