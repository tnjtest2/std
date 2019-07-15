namespace ND2
{
    using System;
    using System.Collections.Generic;

    internal static class Program
    {
        private static void Main()
        {
            for (var i = 100000; i < 999999; i++)
            {
                var array = ToArray(i);

                if (!IsUnique(array))
                {
                    continue;
                }

                var isMagicNumber = true;

                for (var j = 2; j <= 6; j++)
                {
                    if (!Contains(array, ToArray(i * j)))
                    {
                        isMagicNumber = false;
                        break;
                    }
                }

                if (isMagicNumber)
                {
                    Console.WriteLine(i);
                }
            }

            Console.ReadKey();
        }

        private static bool Contains(int[] array1, int[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            foreach (var a1 in array1)
            {
                var contains = false;

                foreach (var a2 in array2)
                {
                    if (a1 == a2)
                    {
                        contains = true;
                        break;
                    }
                }

                if (!contains)
                {
                    return false;
                }
            }

            return true;
        }

        private static int[] ToArray(int number)
        {
            //var stack = new Stack<int>();

            //while (number > 0)
            //{
            //    stack.Push(number % 10);
            //    number /= 10;
            //}

            //return stack.ToArray();

            var array = new int[number.ToString().Length];

            for (var i = array.Length - 1; i >= 0; i--)
            {
                array[i] = number % 10;
                number /= 10;
            }

            return array;
        }

        private static bool IsUnique(int[] array)
        {
            for (var i = 0; i < array.Length; i++)
            {
                for (var j = i + 1; j < array.Length; j++)
                {
                    if (array[i] == array[j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}