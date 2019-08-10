namespace ND4.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Entities;

    internal class Game
    {
        public EventHandler GameEnd;

        public void StartGame(int playersCount, int diceCount)
        {
            var players = this.GeneratePlayers(playersCount).ToList();

            this.Roll(players, diceCount);

            this.GameEnd?.Invoke(this, EventArgs.Empty);
        }

        private void DisplayScores(IEnumerable<Player> players)
        {
            foreach (var player in players)
            {
                Console.WriteLine(player.Name + ":");
                Thread.Sleep(300);

                foreach (var dice in player.Dices)
                {
                    Console.WriteLine("  " + dice.Name + ": " + dice.Value);
                    Thread.Sleep(300);
                }

                Console.WriteLine("   Score: " + player.Score);
                Thread.Sleep(300);
            }
        }

        private IEnumerable<Player> GeneratePlayers(int count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return new Player("Player" + (i + 1));
            }
        }

        private IEnumerable<Player> GetWinners(List<Player> players)
        {
            var maxScore = players.Max(x => x.Score);
            return players.Where(x => x.Score == maxScore);
        }

        private void RandomiseDices(IEnumerable<Player> players, int count)
        {
            var random = new Random();

            foreach (var player in players)
            {
                player.ClearScore();

                for (var i = 1; i <= count; i++)
                {
                    player.AddDice(new Dice("Dice" + i, random.Next(1, 7)));
                }
            }
        }

        private void Roll(List<Player> players, int diceCount)
        {
            do
            {
                this.RandomiseDices(players, diceCount);
                this.DisplayScores(players);

                players = this.GetWinners(players).ToList();

                if (players.Count > 1)
                {
                    Console.WriteLine();
                    Console.WriteLine("Rerolling for " + string.Join(", ", players.Select(x => x.Name)));
                    Console.WriteLine();
                }
            }
            while (players.Count > 1);

            Console.WriteLine();
            Console.WriteLine("Winner " + players[0].Name + "!");
        }
    }
}