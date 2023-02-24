using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statsetera.Tests;

[TestClass]
public class TestStats
{
    [TestMethod]
    public void TestEwma()
    {
        var ewma = Stats.Ewma(new double[] {
            40.0, 45.0, 43.0, 31.0, 20.0 },
            0.7);
        Console.WriteLine($"Ewma: {ewma}");
        Assert.AreEqual(33.0655, ewma);
    }
    [TestMethod]
    public void TestSampleStdDev()
    {
        var s = new double[] { 2.0, 4.0, 6.0, 8.0, 10.0 }.SampleStdDev();
        Console.WriteLine(s);
        Assert.AreEqual(3.1622776601683795, s, 1e-15);
    }
    [TestMethod]
    public void TestSumOfSquares() 
    {
        var s = new double[] { 1.0, 2.0, 3.0, 4.0, 5.0 }.SumOfSquares(x=>x);
        Console.WriteLine($"{s.Item1}, {s.Item2}, {s.Item3}");
        Assert.AreEqual((55.0, 15.0, 5), s);
    }
    [TestMethod]
    public void TestSumOfCubes()
    {
        var s = new double[] { 1.0, 2.0, 3.0, 4.0, 5.0 }.SumOfCubes(x => x);
        Console.WriteLine($"{s.Item1}, {s.Item2}, {s.Item3}, {s.Item4}");
        Assert.AreEqual((225, 55.0, 15.0, 5), s);
    }
    [TestMethod]
    public void TestEmpiricalDistributionFunction()
    {
        double[] data = new double[] { 1.0, 5.0, 4.0, 4.0, 2.0, 3.0, 2.0, 1.0 };
        var F = Stats.EmpiricalDistributionFunction(data);
        Console.WriteLine(F(-10.0));
        Assert.AreEqual(F(-10.0), 0);

        Console.WriteLine(F(1.0));
        Assert.AreEqual(F(1.0), 0.25);
        
        Console.WriteLine(F(2.0));
        Assert.AreEqual(F(2.0), 0.5);

        Console.WriteLine(F(3.0));
        Assert.AreEqual(F(3.0), 0.625);

        Console.WriteLine(F(4.0));
        Assert.AreEqual(F(4.0), 0.875);

        Console.WriteLine(F(4.8));
        Assert.AreEqual(F(4.8), 0.875);

        Console.WriteLine(F(5.0));
        Assert.AreEqual(F(5.0), 1.0);

        Console.WriteLine(F(100.0));
        Assert.AreEqual(F(100.0), 1.0);
    }
}
