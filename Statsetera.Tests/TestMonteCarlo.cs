using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statsetera.Tests;

[TestClass]
public class TestMonteCarlo
{
    [TestMethod]
    public void TestMeanIncremental()
    {
        var s = Stats.MeanIncremental(Enumerable.Range(1, 10),
            x => Convert.ToDouble(x)).ToArray();
        foreach (double d in s)
        {
            Console.Write($"{d},");
        }
        var expected = new double[] {
            1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0, 5.5
        };
        CollectionAssert.AreEqual(expected, s);

    }
    [TestMethod]
    public void TestLinearCongruent()
    {
        var s = MonteCarlo.LinearCongruent(0, 8, 5, 1).Take(10).ToArray();

        var expected = new int[] { 1, 6, 7, 4, 5, 2, 3, 0, 1, 6 };
        foreach (int i in s)
        {
            Console.Write($"{i},");
        }
        CollectionAssert.AreEqual(expected, s);
    }
    [TestMethod]
    public void TestRandomWalk()
    {
        var result = MonteCarlo.RandomWalk(
            rng: Utils.DefaultRandomSource(0)).Take(20).ToArray();
        foreach (int i in result)
        {
            Console.Write($"{i},");
        }

        // the following 'magic' sequence was obtained by setting a fixed seed
        // of 0 to the random source, so that it will return the same sequence
        // every time
        var expected = new int[]
        {
            -1,-2,-3,-4,-5,-6,-7,-8,-7,-8,-9,-8,-7,-6,-7,-6,-7,-6,-5,-4
        };

        CollectionAssert.AreEqual(expected, result);
    }
    [TestMethod]
    public void TestNaiveIntegration()
    {
        // integrating quarter circle
        double s = MonteCarlo.NaiveIntegrate(100000, 0, 1,
            x => Math.Sqrt(1 - x * x), rng: Utils.DefaultRandomSource(0));
        Console.WriteLine($"pi estimate = {s * 4}");
        Assert.AreEqual(3.146137693090427, s*4, 1e-15);
    }
}
