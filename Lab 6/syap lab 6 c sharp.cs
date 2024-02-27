using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

class Program
{
    static void Main()
    {
        var poly1 = new List<Tuple<int, int>> { Tuple.Create(27, 9),
Tuple.Create(2, 9),
Tuple.Create(93, 9),
Tuple.Create(28, 9),
Tuple.Create(10, 9),
Tuple.Create(50, 8),
Tuple.Create(86, 8),
Tuple.Create(13, 8),
Tuple.Create(41, 8),
Tuple.Create(1, 7),
Tuple.Create(56, 7),
Tuple.Create(89, 7),
Tuple.Create(62, 7),
Tuple.Create(63, 6),
Tuple.Create(23, 6),
Tuple.Create(86, 6),
Tuple.Create(82, 6),
Tuple.Create(83, 6),
Tuple.Create(16, 5),
Tuple.Create(22, 5),
Tuple.Create(94, 5),
Tuple.Create(9, 5),
Tuple.Create(25, 4),
Tuple.Create(5, 4),
Tuple.Create(45, 4),
Tuple.Create(75, 4),
Tuple.Create(90, 4),
Tuple.Create(66, 4),
Tuple.Create(45, 4),
Tuple.Create(11, 3),
Tuple.Create(46, 3),
Tuple.Create(45, 3),
Tuple.Create(98, 3),
Tuple.Create(48, 3),
Tuple.Create(18, 3),
Tuple.Create(99, 3),
Tuple.Create(75, 3),
Tuple.Create(87, 2),
Tuple.Create(59, 2),
Tuple.Create(26, 2),
Tuple.Create(23, 2),
Tuple.Create(1, 2),
Tuple.Create(21, 1),
Tuple.Create(66, 1),
Tuple.Create(23, 1),
Tuple.Create(92, 1),
Tuple.Create(30, 1),
Tuple.Create(73, 1),
Tuple.Create(74, 1),
Tuple.Create(26, 1)};
        var poly2 = new List<Tuple<int, int>> { Tuple.Create(27, 9),
Tuple.Create(2, 9),
Tuple.Create(93, 9),
Tuple.Create(28, 9),
Tuple.Create(10, 9),
Tuple.Create(50, 8),
Tuple.Create(86, 8),
Tuple.Create(13, 8),
Tuple.Create(41, 8),
Tuple.Create(1, 7),
Tuple.Create(56, 7),
Tuple.Create(89, 7),
Tuple.Create(62, 7),
Tuple.Create(63, 6),
Tuple.Create(23, 6),
Tuple.Create(86, 6),
Tuple.Create(82, 6),
Tuple.Create(83, 6),
Tuple.Create(16, 5),
Tuple.Create(22, 5),
Tuple.Create(94, 5),
Tuple.Create(9, 5),
Tuple.Create(25, 4),
Tuple.Create(5, 4),
Tuple.Create(45, 4),
Tuple.Create(75, 4),
Tuple.Create(90, 4),
Tuple.Create(66, 4),
Tuple.Create(45, 4),
Tuple.Create(11, 3),
Tuple.Create(46, 3),
Tuple.Create(45, 3),
Tuple.Create(98, 3),
Tuple.Create(48, 3),
Tuple.Create(18, 3),
Tuple.Create(99, 3),
Tuple.Create(75, 3),
Tuple.Create(87, 2),
Tuple.Create(59, 2),
Tuple.Create(26, 2),
Tuple.Create(23, 2),
Tuple.Create(1, 2),
Tuple.Create(21, 1),
Tuple.Create(66, 1),
Tuple.Create(23, 1),
Tuple.Create(92, 1),
Tuple.Create(30, 1),
Tuple.Create(73, 1),
Tuple.Create(74, 1),
Tuple.Create(26, 1)};

        //var stopwatch = new Stopwatch();
        //stopwatch.Start();

        //var result = MultiplyPolynomials(poly1, poly2);

        //stopwatch.Stop();
        //Console.WriteLine($"Time elapsed: {stopwatch.Elapsed}");

        //PrintPolynomial(result);
        int numIterations = 10000;
        TimeSpan totalElapsedTime = TimeSpan.Zero;
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int i = 0; i < numIterations; i++)
        {
            List<Tuple<int,int>> result = MultiplyPolynomials(poly1, poly2);
        }
        stopwatch.Stop();
        totalElapsedTime = stopwatch.Elapsed;

        Console.WriteLine("Total computation time: " + totalElapsedTime.TotalMilliseconds + " ms");
        //var result1 = MultiplyPolynomials(poly1, poly2);
        //PrintPolynomial(result1);
    }

    static List<Tuple<int, int>> MultiplyPolynomials(List<Tuple<int, int>> poly1, List<Tuple<int, int>> poly2)
    {
        var result = new List<Tuple<int, int>>();

        foreach (var term1 in poly1)
        {
            foreach (var term2 in poly2)
            {
                var coeff = term1.Item1 * term2.Item1;
                var exp = term1.Item2 + term2.Item2;
                result.Add(Tuple.Create(coeff, exp));
            }
        }

        var grouped = new Dictionary<int, int>();

        foreach (var item in result)
        {
            if (item.Item1 != 0)
            {
                if (grouped.ContainsKey(item.Item2))
                {
                    grouped[item.Item2] += item.Item1;
                }
                else
                {
                    grouped[item.Item2] = item.Item1;
                }
            }
        }

        var sortedResult = new List<Tuple<int, int>>();
        foreach (var pair in grouped.OrderByDescending(x => x.Key))
        {
            sortedResult.Add(Tuple.Create(pair.Value, pair.Key));
        }

        return sortedResult;

    }

    static void PrintPolynomial(List<Tuple<int, int>> poly)
    {
        for (int i = 0; i < poly.Count; i++)
        {
            var term = poly[i];
            var coeff = term.Item1;
            var exp = term.Item2;

            if (coeff == 0) continue;

            if (i > 0 && coeff > 0)
                Console.Write("+");

            if (exp == 0)
                Console.Write(coeff);
            else if (coeff == 1 && exp == 1)
                Console.Write("x");
            else if (coeff == 1)
                Console.Write($"x^{exp}");
            else if (exp == 1)
                Console.Write($"{coeff}x");
            else
                Console.Write($"{coeff}x^{exp}");
        }

        Console.WriteLine();
    }
}
