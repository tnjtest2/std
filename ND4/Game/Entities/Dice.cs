namespace ND4.Game.Entities
{
    internal struct Dice
    {
        public int Value { get; }

        public string Name { get; }

        public Dice(string name, int value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}