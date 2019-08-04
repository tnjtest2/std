using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame.Gui
{
    class Window : GuiObject
    {
        private Frame _border;

        public Window(int x, int y, int width, int height, char borderChar) : base(x, y, width, height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;

            _border = new Frame(x, y, width, height, borderChar);
        }

        public override void Render()
        {
            _border.Render();
        }
    }
}
