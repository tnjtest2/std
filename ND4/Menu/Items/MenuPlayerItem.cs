namespace ND4.Menu.Items
{
    using System;

    internal class MenuPlayerItem
    {
        private readonly int x;

        private readonly int y;

        private bool isActive;

        public MenuPlayerItem(string name, int x, int y)
        {
            this.x = x;
            this.y = y;
            this.Name = name;
        }

        public bool IsActive
        {
            get
            {
                return this.isActive;
            }
            set
            {
                this.isActive = value;
                this.Render();
            }
        }

        public string Name { get; }

        public void Render()
        {
            Console.SetCursorPosition(this.x, this.y);
            Console.Write(this.IsActive ? ">" : " ");
            Console.Write(this.Name);
        }
    }
}