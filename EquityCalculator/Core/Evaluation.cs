namespace EquityCalculator.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Cards;

    using Enums;

    using Players;

    using Utils;

    internal class Evaluation
    {
        private readonly Dictionary<CardSuit, double> flashHashes = new Dictionary<CardSuit, double>
        {
            // few hashes to speed up calcs
            { CardSuit.Diamond, Math.Pow((double)CardSuit.Diamond, 5) },
            { CardSuit.Spade, Math.Pow((double)CardSuit.Spade, 5) },
            { CardSuit.Club, Math.Pow((double)CardSuit.Club, 5) },
            { CardSuit.Hearth, Math.Pow((double)CardSuit.Hearth, 5) },
        };

        private readonly Dictionary<CardRank, double> straightHashes = new Dictionary<CardRank, double>
        {
            // few hashes to speed up calcs
            {
                CardRank.Ace,
                (double)CardRank.Ten * (double)CardRank.Jack * (double)CardRank.Queen * (double)CardRank.King * (double)CardRank.Ace
            },
            {
                CardRank.King,
                (double)CardRank.Nine * (double)CardRank.Ten * (double)CardRank.Jack * (double)CardRank.Queen * (double)CardRank.King
            },
            {
                CardRank.Queen,
                (double)CardRank.Eight * (double)CardRank.Nine * (double)CardRank.Ten * (double)CardRank.Jack * (double)CardRank.Queen
            },
            {
                CardRank.Jack,
                (double)CardRank.Seven * (double)CardRank.Eight * (double)CardRank.Nine * (double)CardRank.Ten * (double)CardRank.Jack
            },
            {
                CardRank.Ten,
                (double)CardRank.Six * (double)CardRank.Seven * (double)CardRank.Eight * (double)CardRank.Nine * (double)CardRank.Ten
            },
            {
                CardRank.Nine,
                (double)CardRank.Five * (double)CardRank.Six * (double)CardRank.Seven * (double)CardRank.Eight * (double)CardRank.Nine
            },
            {
                CardRank.Eight,
                (double)CardRank.Four * (double)CardRank.Five * (double)CardRank.Six * (double)CardRank.Seven * (double)CardRank.Eight
            },
            {
                CardRank.Seven,
                (double)CardRank.Three * (double)CardRank.Four * (double)CardRank.Five * (double)CardRank.Six * (double)CardRank.Seven
            },
            {
                CardRank.Six,
                (double)CardRank.Two * (double)CardRank.Three * (double)CardRank.Four * (double)CardRank.Five * (double)CardRank.Six
            },
            {
                CardRank.Five,
                (double)CardRank.Ace * (double)CardRank.Two * (double)CardRank.Three * (double)CardRank.Four * (double)CardRank.Five
            },
        };

        public void EvaluateHands(Board board, IReadOnlyCollection<Player> players)
        {
            var playerCombinations = players.ToDictionary(x => x, x => this.GetCombination(x.Hand, board));
            var bestCombinations = playerCombinations.GroupBy(x => x.Value.Combination).OrderByDescending(x => x.Key).First().ToArray();

            if (bestCombinations.Length == 1)
            {
                bestCombinations[0].Key.GameWon();
                return;
            }

            var playerHandStrengths = this.GetHandStrength(bestCombinations);
            var max = playerHandStrengths.Values.Max();
            var bestHands = playerHandStrengths.Where(x => x.Value == max).Select(x => x.Key).ToList();

            if (bestHands.Count == 1)
            {
                bestHands[0].GameWon();
            }
            else
            {
                foreach (var hand in bestHands)
                {
                    hand.GameTied(bestHands.Count);
                }
            }
        }

        private static bool HasHash(double item, double hash)
        {
            return (item / hash) % 1 == 0;
        }

        private PlayerCombination GetCombination(Hand hand, Board board)
        {
            var cached = new StructuredBoardCards(board.Concat(hand));
            PlayerCombination combination;

            // some checks can be merged, but too lazy

            combination = this.IsRoyalFlush(cached);
            if (combination.IsSuccessful)
            {
                return combination;
            }

            combination = this.IsStraightFlush(cached);
            if (combination.IsSuccessful)
            {
                return combination;
            }

            combination = this.IsFourOfaKind(cached);
            if (combination.IsSuccessful)
            {
                return combination;
            }

            combination = this.IsFullHouse(cached);
            if (combination.IsSuccessful)
            {
                return combination;
            }

            combination = this.IsFlush(cached);
            if (combination.IsSuccessful)
            {
                return combination;
            }

            combination = this.IsStraight(cached);
            if (combination.IsSuccessful)
            {
                return combination;
            }

            combination = this.IsThreeOfaKind(cached);
            if (combination.IsSuccessful)
            {
                return combination;
            }

            combination = this.IsTwoPair(cached);
            if (combination.IsSuccessful)
            {
                return combination;
            }

            combination = this.IsPair(cached);
            if (combination.IsSuccessful)
            {
                return combination;
            }

            return this.GetHighCards(cached);
        }

        private Dictionary<Player, int> GetHandStrength(IEnumerable<KeyValuePair<Player, PlayerCombination>> bestCombinations)
        {
            var strengthValues = new Dictionary<Player, int>();

            foreach (var (player, playerCombination) in bestCombinations)
            {
                int strength;

                switch (playerCombination.Combination)
                {
                    case HandCombination.Straight:
                    case HandCombination.Flush:
                    case HandCombination.FourOfAKind:
                    case HandCombination.StraightFlush:
                    case HandCombination.RoyalFlush:
                    {
                        // check high card only
                        strength = (int)playerCombination.Cards[0].Rank;
                        break;
                    }
                    case HandCombination.FullHouse:
                    {
                        // give 3kind more value
                        var threeRank = (int)playerCombination.Cards[0].Rank;
                        var pairRank = (int)playerCombination.Cards[4].Rank;

                        strength = (threeRank * 100) + pairRank;
                        break;
                    }
                    case HandCombination.ThreeOfaKind:
                    {
                        // give 3kind more value
                        var threeRank = (int)playerCombination.Cards[0].Rank;
                        var kicker1 = (int)playerCombination.Cards[3].Rank;
                        var kicker2 = (int)playerCombination.Cards[4].Rank;

                        strength = (threeRank * 100) + kicker1 + kicker2;
                        break;
                    }
                    case HandCombination.TwoPair:
                    {
                        // give pair more value
                        var pairRank1 = (int)playerCombination.Cards[0].Rank;
                        var pairRank2 = (int)playerCombination.Cards[2].Rank;
                        var kicker = (int)playerCombination.Cards[4].Rank;

                        strength = (pairRank1 * 100) + (pairRank2 * 100) + kicker;
                        break;
                    }
                    case HandCombination.OnePair:
                    {
                        // give pair more value
                        var pairRank = (int)playerCombination.Cards[0].Rank;
                        var kicker1 = (int)playerCombination.Cards[2].Rank;
                        var kicker2 = (int)playerCombination.Cards[3].Rank;
                        var kicker3 = (int)playerCombination.Cards[4].Rank;

                        strength = (pairRank * 100) + kicker1 + kicker2 + kicker3;
                        break;
                    }
                    default:
                    {
                        strength = playerCombination.Cards.Sum(x => (int)x.Rank);
                        break;
                    }
                }

                strengthValues.Add(player, strength);
            }

            return strengthValues;
        }

        private PlayerCombination GetHighCards(StructuredBoardCards cards)
        {
            return new PlayerCombination(cards.Cards.Take(5).ToArray(), HandCombination.HighCard);
        }

        private PlayerCombination IsFlush(StructuredBoardCards board)
        {
            foreach (var (suit, flashHash) in this.flashHashes)
            {
                if (!HasHash(board.SuitHash, flashHash))
                {
                    // not flash
                    continue;
                }

                // need only rank for combination strength evaluation
                var combination = new[] { new Card(board.SuitGroup[suit][0].Rank, CardSuit.Hearth) };

                return new PlayerCombination(combination, HandCombination.StraightFlush);
            }

            return PlayerCombination.Failed;
        }

        private PlayerCombination IsFourOfaKind(StructuredBoardCards board)
        {
            foreach (var (rank, cards) in board.RankGroup)
            {
                if (cards.Length != 4)
                {
                    // no 4 kind
                    continue;
                }

                // need only kicker for combination strength evaluation
                var combination = board.Cards.Where(x => x.Rank != rank).Take(1).ToArray();
                return new PlayerCombination(combination, HandCombination.FourOfAKind);
            }

            return PlayerCombination.Failed;
        }

        private PlayerCombination IsFullHouse(StructuredBoardCards board)
        {
            foreach (var (rank3, cards3) in board.RankGroup)
            {
                if (cards3.Length != 3)
                {
                    // no 3 kind
                    continue;
                }

                foreach (var (rank2, cards2) in board.RankGroup)
                {
                    if (rank3 == rank2 || cards2.Length < 2)
                    {
                        // no pair
                        continue;
                    }

                    var combination = cards3.Concat(cards2).Take(5).ToArray();
                    return new PlayerCombination(combination, HandCombination.FullHouse);
                }
            }

            return PlayerCombination.Failed;
        }

        private PlayerCombination IsPair(StructuredBoardCards board)
        {
            foreach (var (_, cards) in board.RankGroup)
            {
                if (cards.Length != 2)
                {
                    // no pair
                    continue;
                }

                var pairs = cards;
                var kickers = board.Cards.Where(x => !pairs.Contains(x)).Take(3);

                return new PlayerCombination(pairs.Concat(kickers).ToArray(), HandCombination.OnePair);
            }

            return PlayerCombination.Failed;
        }

        private PlayerCombination IsRoyalFlush(StructuredBoardCards cards)
        {
            if (!HasHash(cards.RankHash, this.straightHashes[CardRank.Ace]))
            {
                // not A high straight
                return PlayerCombination.Failed;
            }

            foreach (var (suit, hash) in this.flashHashes)
            {
                if (!HasHash(cards.SuitHash, hash))
                {
                    // not flash
                    continue;
                }

                var flashCards = cards.SuitGroup[suit].Take(5).ToArray();

                if (flashCards.GetRankHash() != this.straightHashes[CardRank.Ace])
                {
                    // not royal flash
                    continue;
                }

                return new PlayerCombination(flashCards, HandCombination.RoyalFlush);
            }

            return PlayerCombination.Failed;
        }

        private PlayerCombination IsStraight(StructuredBoardCards board)
        {
            foreach (var (rank, straightHash) in this.straightHashes)
            {
                if (!HasHash(board.RankHash, straightHash))
                {
                    // not straight
                    continue;
                }

                // need only rank for combination strength evaluation
                var combination = new[] { new Card(rank, CardSuit.Hearth) };

                return new PlayerCombination(combination, HandCombination.Straight);
            }

            return PlayerCombination.Failed;
        }

        private PlayerCombination IsStraightFlush(StructuredBoardCards board)
        {
            foreach (var (_, flashHash) in this.flashHashes)
            {
                if (!HasHash(board.SuitHash, flashHash))
                {
                    // not flash
                    continue;
                }

                foreach (var (rank, straightHash) in this.straightHashes)
                {
                    if (!HasHash(board.RankHash, straightHash))
                    {
                        // not straight
                        continue;
                    }

                    // need only rank for combination strength evaluation
                    var combination = new[] { new Card(rank, CardSuit.Hearth) };

                    return new PlayerCombination(combination, HandCombination.StraightFlush);
                }
            }

            return PlayerCombination.Failed;
        }

        private PlayerCombination IsThreeOfaKind(StructuredBoardCards board)
        {
            foreach (var (rank, cards) in board.RankGroup)
            {
                if (cards.Length != 3)
                {
                    // not three of a kind
                    continue;
                }

                var combination = cards.Concat(board.Cards.Where(x => x.Rank != rank).Take(2)).ToArray();
                return new PlayerCombination(combination, HandCombination.ThreeOfaKind);
            }

            return PlayerCombination.Failed;
        }

        private PlayerCombination IsTwoPair(StructuredBoardCards board)
        {
            foreach (var (pair1Rank, pair1Cards) in board.RankGroup)
            {
                if (pair1Cards.Length != 2)
                {
                    // no pair
                    continue;
                }

                foreach (var (pair2Rank, pair2Cards) in board.RankGroup)
                {
                    if (pair1Rank == pair2Rank || pair2Cards.Length != 2)
                    {
                        // no 2nd pair
                        continue;
                    }

                    var pairs = pair1Cards.Concat(pair2Cards);
                    var kicker = board.Cards.Where(x => !pairs.Contains(x)).Take(1);

                    return new PlayerCombination(pairs.Concat(kicker).ToArray(), HandCombination.TwoPair);
                }
            }

            return PlayerCombination.Failed;
        }
    }
}