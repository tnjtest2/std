namespace EquityCalculator.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Cards;

    using Data;

    using Enums;

    using EventArgs;

    using Players;

    internal class HandParser
    {
        public EventHandler<PlayerEventArgs> HandParsed;

        private readonly List<Card> allStartingHands = new List<Card>(GameData.TotalCardsCount);

        private readonly Dictionary<ConsoleKey, CardRank> cardKeys = new Dictionary<ConsoleKey, CardRank>
        {
            { ConsoleKey.D2, CardRank.Two },
            { ConsoleKey.NumPad2, CardRank.Two },
            { ConsoleKey.D3, CardRank.Three },
            { ConsoleKey.NumPad3, CardRank.Three },
            { ConsoleKey.D4, CardRank.Four },
            { ConsoleKey.NumPad4, CardRank.Four },
            { ConsoleKey.D5, CardRank.Four },
            { ConsoleKey.NumPad5, CardRank.Five },
            { ConsoleKey.D6, CardRank.Six },
            { ConsoleKey.NumPad6, CardRank.Six },
            { ConsoleKey.D7, CardRank.Seven },
            { ConsoleKey.NumPad7, CardRank.Seven },
            { ConsoleKey.D8, CardRank.Eight },
            { ConsoleKey.NumPad8, CardRank.Eight },
            { ConsoleKey.D9, CardRank.Nine },
            { ConsoleKey.NumPad9, CardRank.Nine },
            { ConsoleKey.T, CardRank.Ten },
            { ConsoleKey.J, CardRank.Jack },
            { ConsoleKey.Q, CardRank.Queen },
            { ConsoleKey.K, CardRank.King },
            { ConsoleKey.A, CardRank.Ace },
        };

        private readonly Random rng = new Random();

        private readonly Dictionary<ConsoleKey, CardSuit> suitKeys = new Dictionary<ConsoleKey, CardSuit>
        {
            { ConsoleKey.C, CardSuit.Club },
            { ConsoleKey.D, CardSuit.Diamond },
            { ConsoleKey.H, CardSuit.Hearth },
            { ConsoleKey.S, CardSuit.Spade },
        };

        public HandParser()
        {
            foreach (var suit in Enum.GetValues(typeof(CardSuit)).Cast<CardSuit>())
            {
                foreach (var rank in Enum.GetValues(typeof(CardRank)).Cast<CardRank>())
                {
                    this.allStartingHands.Add(new Card(rank, suit));
                }
            }
        }

        public void ParseInput()
        {
            Console.Clear();
            Console.CursorVisible = true;

            var players = new List<Player>();
            var count = this.GetPlayersCount();
            var usedCards = new List<Card>();

            for (var i = 0; i < count; i++)
            {
                var playerName = "Player" + (i + 1);
                var cards = new Card[GameData.PlayerCardsCount];

                Console.Write(playerName + ": ");

                // get 2* cards for player
                for (var j = 0; j < GameData.PlayerCardsCount; j++)
                {
                    CardRank? rank = null;

                    // get correct card
                    while (true)
                    {
                        var key = Console.ReadKey(true).Key;

                        if (key == ConsoleKey.Enter)
                        {
                            // generate random
                            var random = this.GenerateRandomCard(usedCards);
                            cards[j] = random;
                            usedCards.Add(random);
                            Console.Write(random);
                            break;
                        }

                        if (rank == null)
                        {
                            if (this.cardKeys.TryGetValue(key, out var cardRank))
                            {
                                rank = cardRank;
                                Console.Write(Card.RankAbbreviations[rank.Value]);
                            }
                        }
                        else
                        {
                            if (this.suitKeys.TryGetValue(key, out var cardSuit))
                            {
                                var newCard = new Card(rank.Value, cardSuit);
                                if (usedCards.Contains(newCard))
                                {
                                    continue;
                                }

                                usedCards.Add(newCard);
                                cards[j] = newCard;
                                Console.Write(Card.SuitAbbreviations[cardSuit]);
                                break;
                            }
                        }
                    }
                }

                var player = new Player(playerName, new Hand(cards));
                players.Add(player);
                Console.WriteLine();
            }

            this.HandParsed?.Invoke(this, new PlayerEventArgs(players));
        }

        public void Replay()
        {
            Console.WriteLine();
            Console.WriteLine("Press P to play again...");

            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.P)
            {
                this.ParseInput();
            }
        }

        private Card GenerateRandomCard(IEnumerable<Card> usedCards)
        {
            var availableHands = this.allStartingHands.Except(usedCards).ToArray();
            return availableHands[this.rng.Next(0, availableHands.Length)];
        }

        private int GetPlayersCount()
        {
            const int MinPlayers = 2;
            const int MaxPlayers = 9;

            while (true)
            {
                Console.Write("Players count: ");

                if (int.TryParse(Console.ReadLine(), out var count) && count >= MinPlayers && count <= MaxPlayers)
                {
                    Console.WriteLine();
                    return count;
                }

                Console.Clear();
            }
        }
    }
}