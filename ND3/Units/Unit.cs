using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame.Units
{
    class Unit
    {
        protected int X;
        protected int Y;
        private string Name;

        public Unit(int x, int y, string name)
        {
            X = x;
            Y = y;
            Name = name;
        }

        public void PrintInfo()
        {
            Console.WriteLine($" Unit {Name} is at {X}x{Y}");
        }
    }
}
