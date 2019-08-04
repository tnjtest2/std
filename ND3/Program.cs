using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleGame.Game;
using ConsoleGame.Gui;

namespace ConsoleGame
{
    class Program
    {
        static void Main()
        {
            var guiController = new GuiController();
            guiController.ShowMenu();
        }
    }
}
