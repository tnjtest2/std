using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame.Units
{
    class Enemy : Unit
    {
        private int _id;

        public Enemy(int id, int x, int y, string name) : base(x, y, name)
        {
            this._id = id;
        }

        public void MoveDown()
        {
            Y++;
        }

        public int GetId()
        {
            return _id;
        }
    }
}
