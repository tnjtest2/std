namespace EquityCalculator.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using EventArgs;

    using Players;

    internal class EquityCalculator
    {
        private readonly Evaluation evaluation = new Evaluation();

        private readonly Generation generation = new Generation();

        private readonly HandParser handParser = new HandParser();

        private readonly List<Task> helperTasks = new List<Task>();

        private int gamesPlayed;

        private Stopwatch stopwatch;

        private CancellationToken taskToken;

        private CancellationTokenSource taskTokenSource;

        public EquityCalculator()
        {
            Console.OutputEncoding = Encoding.UTF8;

            this.handParser.HandParsed += this.OnHandParsed;
            this.handParser.ParseInput();
        }

        private void GenerateTaskToken()
        {
            this.taskTokenSource = new CancellationTokenSource();
            this.taskToken = this.taskTokenSource.Token;
        }

        private void OnHandParsed(object sender, PlayerEventArgs e)
        {
            this.stopwatch = Stopwatch.StartNew();
            this.gamesPlayed = 0;
            Console.CursorVisible = false;

            this.GenerateTaskToken();
            this.StartHelperTasks(e.Players);

            var boards = this.generation.GeneratePossibleBoards(e.Players);

            try
            {
                Parallel.ForEach(
                    boards,
                    new ParallelOptions()
                    {
                        CancellationToken = this.taskToken
                    },
                    board =>
                        {
                            this.evaluation.EvaluateHands(board, e.Players);
                            Interlocked.Increment(ref this.gamesPlayed);
                        });
            }
            catch (OperationCanceledException)
            {
                // evaluation canceled by user
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            this.StopHelperTasks();
            this.WaitAllTasks();

            this.handParser.Replay();
        }

        private void StartHelperTasks(IReadOnlyList<Player> players)
        {
            // evaluation cancel
            // blocks additional thread, not worth it?
            var cancelTask = Task.Run(
                () =>
                    {
                        while (!this.taskToken.IsCancellationRequested)
                        {
                            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                            {
                                this.StopHelperTasks();
                                return;
                            }
                        }
                    },
                this.taskToken);

            // display results
            var resultsTask = Task.Run(
                () =>
                    {
                        while (!this.taskToken.IsCancellationRequested)
                        {
                            if (this.gamesPlayed > 0)
                            {
                                for (var i = 0; i < players.Count; i++)
                                {
                                    Console.SetCursorPosition(16, i + 2);
                                    Console.Write(players[i].GetEquity(this.gamesPlayed).ToString("N2") + "%   ");
                                }
                            }

                            Console.SetCursorPosition(0, players.Count + 3);
                            Console.WriteLine("Games: " + this.gamesPlayed.ToString("N0"));
                            Console.WriteLine("Elapsed: " + (this.stopwatch.Elapsed.TotalSeconds).ToString("N1") + "s");
                        }
                    },
                this.taskToken);

            this.helperTasks.Clear();
            this.helperTasks.Add(cancelTask);
            this.helperTasks.Add(resultsTask);
        }

        private void StopHelperTasks()
        {
            this.taskTokenSource.Cancel();
        }

        private void WaitAllTasks()
        {
            try
            {
                // wait until tasks will finish last iteration
                Task.WaitAll(this.helperTasks.ToArray());
            }
            catch (Exception ex)
            {
                //check for any inside exceptions
                Console.WriteLine(ex);
            }
        }
    }
}