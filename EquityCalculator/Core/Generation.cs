namespace EquityCalculator.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Cards;

    using Data;

    using Enums;

    using Players;

    internal class Generation
    {
        private readonly List<Card> allCards = new List<Card>(GameData.TotalCardsCount);

        public Generation()
        {
            foreach (var rank in Enum.GetValues(typeof(CardRank)).Cast<CardRank>())
            {
                foreach (var suit in Enum.GetValues(typeof(CardSuit)).Cast<CardSuit>())
                {
                    this.allCards.Add(new Card(rank, suit));
                }
            }
        }

        public Board[] GeneratePossibleBoards(IEnumerable<Player> players)
        {
            var cards = this.allCards.Except(players.SelectMany(x => x.Cards)).ToArray();
            var cardsCount = cards.Length;
            var combinationsCount = GetCombinationsCount(cardsCount, GameData.BoardCardsCount);

            var boards = new List<Board>(combinationsCount);

            for (var i = 0; i < cardsCount; i++)
            {
                for (var j = i + 1; j < cardsCount; j++)
                {
                    for (var k = j + 1; k < cardsCount; k++)
                    {
                        for (var l = k + 1; l < cardsCount; l++)
                        {
                            for (var m = l + 1; m < cardsCount; m++)
                            {
                                boards.Add(new Board(cards[i], cards[j], cards[k], cards[l], cards[m]));
                            }
                        }
                    }
                }
            }

            return boards.ToArray();
        }

        private static int GetCombinationsCount(int n, int k)
        {
            var result = 1;
            for (var i = 1; i <= k; i++)
            {
                result *= n--;
                result /= i;
            }

            return result;
        }
    }
}