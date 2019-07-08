namespace ND1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal static class Program
    {
        private static readonly Dictionary<int, string> DoubleNames = new Dictionary<int, string>
        {
            { 2, "dvidesimt" },
        };

        private static readonly Dictionary<int, string> GroupNames = new Dictionary<int, string>
        {
            { 1, "simtai" },
            { 2, "tukstanciai" },
            { 3, "milijonai" },
            { 4, "milijardai" },
        };

        private static readonly Dictionary<int, string> Names = new Dictionary<int, string>
        {
            { 0, "" },
            { 1, "vienas" },
            { 2, "du" },
            { 3, "tris" },
            { 4, "keturi" },
            { 5, "penki" },
            { 6, "sesi" },
            { 7, "septyni" },
            { 8, "astuoni" },
            { 9, "devyni" },
            { 10, "desimt" },
            { 11, "vienuolika" },
            { 12, "dvylika" },
            { 13, "trylika" },
        };

        public static bool IsNumber(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            text = text.TrimStart('-');

            if (text.Length == 0)
            {
                return false;
            }

            return text.All(char.IsDigit);
        }

        private static string ChangeNumberToText(long number)
        {
            if (number == 0)
            {
                return "nulis";
            }

            var text = new StringBuilder();

            if (number < 0)
            {
                text.Append("minus ");
                number = Math.Abs(number);
            }

            var numberStr = number.ToString();
            var groups = (int)Math.Ceiling(numberStr.Length / 3f);
            var offset = numberStr.Length - (groups * 3);

            //var split = number.ToString("N0").Split(CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator);

            for (var i = 0; i < groups; i++)
            {
                var groupNumber = groups - i;
                var group = int.Parse(numberStr.Substring(Math.Max(0, (i * 3) + offset), i == 0 ? 3 + offset : 3));

                var nr1 = group / 100;
                var nr2 = group % 100 / 10;
                var nr3 = group % 100 % 10;

                if (nr1 > 0)
                {
                    text.Append(ParseSingleNumber(nr1, true));

                    if (nr1 > 1)
                    {
                        text.Append(" ");
                    }

                    text.Append(GetGroupName(0, nr1, 1)).Append(" ");
                }

                if (nr2 > 0)
                {
                    text.Append(ParseDoubleNumber(nr2, nr3)).Append(" ");
                }
                else if (nr3 > 0)
                {
                    text.Append(ParseSingleNumber(nr3, nr1 != 0 && nr2 != 0)).Append(" ");
                }

                if (groupNumber != 1 && group > 0)
                {
                    text.Append(GetGroupName(nr2, nr3, groupNumber)).Append(" ");
                }
            }

            return text.ToString();
        }

        private static string GetGroupName(int left, int right, int gr)
        {
            try
            {
                var groupName = GroupNames[gr];

                if (right == 1)
                {
                    if (gr == 2)
                    {
                        // tukstanciai => tukstantis
                        groupName = groupName.Substring(0, groupName.Length - 4) + "tis";
                    }
                    else
                    {
                        // simtai => simtas
                        groupName = groupName.Substring(0, groupName.Length - 1) + "s";
                    }
                }
                else if (right == 0 || left == 1)
                {
                    // tukstanciai => tukstanciu
                    groupName = groupName.Substring(0, groupName.Length - 2) + "u";
                }

                return groupName;
            }
            catch
            {
                return "(unknown group)";
            }
        }

        private static bool IsInRange(long x, int min, int max)
        {
            return x >= min && x <= max;
        }

        private static void Main()
        {
            var input = Console.ReadLine();

            if (!IsNumber(input))
            {
                Console.WriteLine("not number");
                return;
            }

            var number = long.Parse(input);

            if (!IsInRange(number, -9, 9))
            {
                Console.WriteLine("not in range");
                //return;
            }

            var textNumber = ChangeNumberToText(number);

            Console.WriteLine(number.ToString("N0"));
            Console.WriteLine(textNumber);

            Console.ReadKey();
        }

        private static string ParseDoubleNumber(int number1, int number2)
        {
            try
            {
                var single = (number1 * 10) + number2;
                if (Names.TryGetValue(single, out var value))
                {
                    return value;
                }

                if (number1 < 2)
                {
                    return Names[number2] + "olika";
                }

                string text;

                if (DoubleNames.TryGetValue(number1, out value))
                {
                    text = value;
                }
                else
                {
                    text = Names[number1] + "asdesimt";
                }

                return text + " " + ParseSingleNumber(number2, false);
            }
            catch
            {
                return "(unknown double value)";
            }
        }

        private static string ParseSingleNumber(int number, bool skipFirst)
        {
            try
            {
                if (skipFirst && number == 1)
                {
                    return string.Empty;
                }

                return Names[number];
            }
            catch
            {
                return "(unknown single value)";
            }
        }
    }
}