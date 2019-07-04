using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ND1
{
    class Program
    {
        static void Main()
        {
            var input = Console.ReadLine();

            if (!IsNumber(input))
            {
                Console.WriteLine("not number");
                return;
            }

            var number = int.Parse(input);

            if (!IsInRange(number, -9, 9))
            {
                Console.WriteLine("not in range");
                //return;
            }

            var textNumber = ChangeNumberToText(number);

            Console.WriteLine(textNumber);
            Console.ReadKey();
        }

        private static Dictionary<int, string> Names = new Dictionary<int, string>
        {
            {0, "" },
            {1, "vienas" },
            {2, "du" },
            {3, "tris" },
            {4, "keturi" },
            {5, "penki" },
            {6, "sesi" },
            {7, "septyni" },
            {8, "astuoni" },
            {9, "devyni" },
            {10, "desimt" },
            {11, "vienuolika" },
            {12, "dvylika" },
            {13, "trylika" },
        };

        private static Dictionary<int, string> Names1x = new Dictionary<int, string>
        {
            {2, "dvidesimt" },
        };

        private static Dictionary<int, string> Names1xx = new Dictionary<int, string>
        {
            {1, "simtas" },
        };

        private static string ParseSingle(int number, bool skipFirst)
        {
            if (skipFirst && number == 1)
            {
                return string.Empty;
            }

            if (Names.TryGetValue(number, out var value))
            {
                return value;
            }

            return "unknown1";
        }

        private static Dictionary<int, string> groupNames = new Dictionary<int, string>
        {
            {1, "simtai" },
            {2, "tukstanciai" },
            {3, "milijonai" },
        };

        private static string GetGroupName(int nr, int group)
        {
            var name = Names[nr] + " " + groupNames[group];

            if (nr == 1)
            {
                name.Replace("")
            }

            return "unknownGroup";
        }

        private static string ChangeNumberToText(int number)
        {
            if (number == 0)
            {
                return "nulis";
            }

            var name = new StringBuilder();

            if (number < 0)
            {
                name.Append("minus ");
                number = Math.Abs(number);
            }

          

            var text = number.ToString();
            var groups = Math.Ceiling(text.Length / 3f);

            for (int i = 0; i < groups; i++)
            {
                var group = int.Parse(text.Substring(i * 3, Math.Min(3, text.Length - i * 3)));

                var nr1 = group / 100;
                var nr2 = group % 100 / 10;
                var nr3 = group % 100 % 10;

                name.Append(ParseSingle(nr1, true));
                name.Append(GetGroupName(nr1, groups - i));

            }
          

            //if (Names.TryGetValue(number, out var value))
            //{
            //    text.Append(value);
            //}
            //else
            //{
            //    if (number / 100 > 0)
            //    {
            //        var nr1 = number / 100;
            //        var nr2 = number % 100 / 10;
            //        var nr3 = number % 100 % 10;

            //        if (Names1xx.TryGetValue(nr1, out var value2))
            //        {
            //            text.Append(value2);
            //        }
            //        else
            //        {
            //            text.Append(Names[nr1]).Append(" simtai");
            //        }

            //        text.Append(" ");

            //        if (nr2 < 2)
            //        {
            //            text.Append(Names[nr3]).Append("olika");
            //        }
            //        else
            //        {
            //            if (Names1x.TryGetValue(nr2, out var value3))
            //            {
            //                text.Append(value3);
            //            }
            //            else
            //            {
            //                text.Append(Names[nr2]).Append("asdesimt");
            //            }

            //            text.Append(" ").Append(Names[nr3]);
            //        }

            //    }
            //    else if (number / 10 > 0)
            //    {
            //        var nr1 = number / 10;
            //        var nr2 = number % 10;

            //        if (nr1 < 2)
            //        {
            //            text.Append(Names[nr2]).Append("olika");
            //        }
            //        else
            //        {
            //            if (Names1x.TryGetValue(nr1, out var value2))
            //            {
            //                text.Append(value2);
            //            }
            //            else
            //            {
            //                text.Append(Names[nr1]).Append("asdesimt");
            //            }

            //            text.Append(" ").Append(Names[nr2]);
            //        }
            //    }
            //}

            return name.ToString();
        }

      

        private static bool IsInRange(int x, int min, int max)
        {
            if (x < min || x > max)
            {
                return false;
            }

            return true;
        }

        public static bool IsNumber(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            if (text[0] == '-')
            {
                if (text.Length < 2)
                {
                    return false;
                }

                return text.Skip(1).All(char.IsDigit);
            }
            else
            {
                return text.All(char.IsDigit);
            }
        }
    }
}
