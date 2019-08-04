using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame.Gui
{
    sealed class GameWindow : Window
    {
        private TextBlock _titleTextBlock;

        private readonly List<Button> buttons = new List<Button>();

        public GameWindow() : base(0, 0, 120, 30, '%')
        {
            _titleTextBlock = new TextBlock(10, 5, 100, new List<string> { "Super duper zaidimas", "Vardas Pavardaitis kuryba!", "Made in Vilnius Coding School!" });

            this.buttons.Add(new Button(20, 13, 18, 5, "Start"));
            this.buttons.Add(new Button(50, 13, 18, 5, "Credits"));
            this.buttons.Add(new Button(80, 13, 18, 5, "Quit"));

            this.ActiveButtonIndex = 0;
        }

        private int activeButtonIndex;

        public void NextButton()
        {
            this.ActiveButtonIndex++;
        }

        public void PreviousButton()
        {
            this.ActiveButtonIndex--;
        }

        public int ActiveButtonIndex
        {
            get
            {
                return this.activeButtonIndex;
            }
            set
            {
                if (value < 0 || value >= this.buttons.Count)
                {
                    return;
                }

                this.ActiveButton.ChangeActive(false);
                this.activeButtonIndex = value;
                this.ActiveButton.ChangeActive(true);
            }
        }

        public Button ActiveButton
        {
            get
            {
                return this.buttons[this.activeButtonIndex];
            }
        }

        public override void Render()
        {
            base.Render();

            _titleTextBlock.Render();

            foreach (var button in this.buttons)
            {
               button.Render();
            }

            Console.SetCursorPosition(0, 0);
        }
    }
}
