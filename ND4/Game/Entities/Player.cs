namespace ND4.Game.Entities
{
    using System.Collections.Generic;
    using System.Linq;

    internal class Player
    {
        private readonly List<Dice> dices = new List<Dice>();

        public Player(string Name)
        {
            this.Name = Name;
        }

        public string Name { get; }

        public int Score
        {
            get
            {
                return this.dices.Sum(x => x.Value);
            }
        }

        public IEnumerable<Dice> Dices
        {
            get
            {
                return this.dices;
            }
        }

        public void AddDice(Dice dice)
        {
            this.dices.Add(dice);
        }

        public void ClearScore()
        {
            this.dices.Clear();
        }
    }
}