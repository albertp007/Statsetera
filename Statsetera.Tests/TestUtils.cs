﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    [TestMethod]
    public void TestUnzip2()
    {
        var s = new (int, int)[]
        {
            (1, 2),
            (4, 5),
            (7, 8)
        };
        var res = s.Unzip();
        CollectionAssert.AreEqual(new int[] { 1, 4, 7 }, res.Item1.ToArray());
        CollectionAssert.AreEqual(new int[] { 2, 5, 8 }, res.Item2.ToArray());
    }
    [TestMethod]
    public void TestUnzip3()
    {
        var s = new (int, int, int) []
        {
            (1, 2, 3),
            (4, 5, 6),
            (7, 8, 9)
        };
        var res = s.Unzip();
        CollectionAssert.AreEqual(new int[] { 1, 4, 7 }, res.Item1.ToArray());
        CollectionAssert.AreEqual(new int[] { 2, 5, 8 }, res.Item2.ToArray());
        CollectionAssert.AreEqual(new int[] { 3, 6, 9 }, res.Item3.ToArray());
    }
    [TestMethod]
    public void TestUnzip4()
    {
        var s = new (int, int, int, int)[]
        {
            (1, 2, 3, 4),
            (4, 5, 6, 7),
            (7, 8, 9, 10)
        };
        var res = s.Unzip();
        CollectionAssert.AreEqual(new int[] { 1, 4, 7 }, res.Item1.ToArray());
        CollectionAssert.AreEqual(new int[] { 2, 5, 8 }, res.Item2.ToArray());
        CollectionAssert.AreEqual(new int[] { 3, 6, 9 }, res.Item3.ToArray());
        CollectionAssert.AreEqual(new int[] { 4, 7, 10 }, 
            res.Item4.ToArray());
    }
    [TestMethod]
    public void TestUnzip5()
    {
        var s = new (int, int, int, int, int)[]
        {
            (1, 2, 3, 4, 5),
            (4, 5, 6, 7, 8),
            (7, 8, 9, 10, 11)
        };
        var res = s.Unzip();
        CollectionAssert.AreEqual(new int[] { 1, 4, 7 }, res.Item1.ToArray());
        CollectionAssert.AreEqual(new int[] { 2, 5, 8 }, res.Item2.ToArray());
        CollectionAssert.AreEqual(new int[] { 3, 6, 9 }, res.Item3.ToArray());
        CollectionAssert.AreEqual(new int[] { 4, 7, 10 },
            res.Item4.ToArray());
        CollectionAssert.AreEqual(new int[] { 5, 8, 11 },
            res.Item5.ToArray());
    }
    [TestMethod]
    public void TestUnzip6()
    {
        var s = new (int, int, int, int, int, int)[]
        {
            (1, 2, 3, 4, 5, 6),
            (4, 5, 6, 7, 8, 9),
            (7, 8, 9, 10, 11, 12)
        };
        var res = s.Unzip();
        CollectionAssert.AreEqual(new int[] { 1, 4, 7 }, res.Item1.ToArray());
        CollectionAssert.AreEqual(new int[] { 2, 5, 8 }, res.Item2.ToArray());
        CollectionAssert.AreEqual(new int[] { 3, 6, 9 }, res.Item3.ToArray());
        CollectionAssert.AreEqual(new int[] { 4, 7, 10 },
            res.Item4.ToArray());
        CollectionAssert.AreEqual(new int[] { 5, 8, 11 },
            res.Item5.ToArray());
        CollectionAssert.AreEqual(new int[] { 6, 9, 12 },
            res.Item6.ToArray());
    }
    [TestMethod]
    public void TestPartition()
    {
        var s = new int[] { 2, 8, 7, 1, 3, 5, 6, 4 };
        int i = Utils.Partition(s);
        CollectionAssert.AreEqual(new int[] { 2, 1, 3, 4, 7, 5, 6, 8 }, s);
        Assert.AreEqual(3, i);
    }
    [TestMethod]
    public void TestRandomizedSelect()
    {
        var s = new int[] { 2, 8, 7, 1, 3, 5, 6, 4 };
        int fourth = Utils.RandomizedSelect(s, 4);
        Assert.AreEqual(4, fourth);

        int fifth = Utils.RandomizedSelect(s, 5);
        Assert.AreEqual(5, fifth);
    }
}
