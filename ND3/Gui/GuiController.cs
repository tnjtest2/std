namespace ConsoleGame.Gui
{
    using System;

    using Game;

    internal class GuiController
    {
        private readonly CreditWindow creditWindow;

        private readonly GameWindow gameWindow;

        private bool inGame;

        public GuiController()
        {
            Console.CursorVisible = false;

            this.gameWindow = new GameWindow();
            this.creditWindow = new CreditWindow();
        }

        public void ShowMenu()
        {
            this.gameWindow.Render();

            while (true)
            {
                this.HandleKey();
            }
        }

        private void HandleKey()
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.LeftArrow:
                    this.gameWindow.PreviousButton();
                    break;

                case ConsoleKey.RightArrow:
                    this.gameWindow.NextButton();
                    break;

                case ConsoleKey.Enter when this.creditWindow.IsActive:
                case ConsoleKey.Escape when this.creditWindow.IsActive:
                    this.creditWindow.IsActive = false;
                    Console.Clear();
                    this.gameWindow.Render();
                    break;

                case ConsoleKey.Enter:
                    switch (this.gameWindow.ActiveButton.Label)
                    {
                        case "Start":
                            new GameController().StartGame();
                            this.inGame = true;
                            break;
                        case "Credits":
                            this.creditWindow.IsActive = true;
                            this.creditWindow.Render();
                            break;
                        case "Quit":
                            Environment.Exit(0);
                            break;
                    }

                    break;

                case ConsoleKey.Escape:
                    if (this.inGame)
                    {
                        this.inGame = false;
                        Console.Clear();
                        this.gameWindow.Render();
                    }
                    else
                    {
                        Environment.Exit(0);
                    }

                    break;
            }
        }
    }
}