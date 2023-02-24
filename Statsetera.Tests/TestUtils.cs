using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statsetera.Tests;

[TestClass]
public class TestUtils
{
    [TestMethod]
    public void TestThen()
    {
        var times2 = (double d) => d * 2;
        var times3 = (double d) => d * 3;
        var times6 = times2.Then(times3);

        Console.WriteLine($"100 times 6 = {times6(100)}");
        // this double comparison should be exact, no need for delta
        Assert.AreEqual(600.0, times6(100));
    }
    [TestMethod]
    public void TestAccumulate()
    {
        var s = new int[] { 1, 2, 3, 4, 5 }.Accumulate(0, 
            (acc, current) => acc + current).ToArray();
        foreach (int i in s)
        {
            Console.WriteLine($"Intermediate sum: {i}");
        }
        CollectionAssert.AreEqual(new int[] { 1, 3, 6, 10, 15 }, s);
    }
    [TestMethod]
    public void TestLag()
    {
        var s = new int[] { 1, 3, 4, 8, 10 };
        var lag1 = s.Lag(1).ToArray();
        var lag2 = s.Lag(2).ToArray();
        var lag3 = s.Lag(3).ToArray();
        var lag4 = s.Lag(4).ToArray();

        CollectionAssert.AreEqual(
            new (int, int)[] { (1, 3), (3, 4), (4, 8), (8, 10) }, lag1);

        CollectionAssert.AreEqual(
            new (int, int)[] { (1, 4), (3, 8), (4, 10) }, lag2);

        CollectionAssert.AreEqual(new (int, int)[] { (1, 8), (3, 10) }, lag3);

        CollectionAssert.AreEqual(new (int, int)[] { (1, 10) }, lag4);
    }
}
