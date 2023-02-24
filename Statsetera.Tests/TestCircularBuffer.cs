using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statsetera.Tests;

[TestClass]
public class TestCircularBuffer
{
    [TestMethod]
    public void Test1()
    {
        var buffer = new CircularBuffer<char>(5);
        buffer.Add('A');
        buffer.Add('B');
        buffer.Add('C');
        buffer.Add('D');
        buffer.Add('E');
        Console.WriteLine(buffer);
        CollectionAssert.AreEqual(
            new char[] { 'A', 'B', 'C', 'D', 'E' }, buffer.ToArray());

        buffer.Add('F');
        Console.WriteLine(buffer);
        CollectionAssert.AreEqual(
            new char[] { 'B', 'C', 'D', 'E', 'F' }, buffer.ToArray());

        char popped = buffer.Pop();
        Console.WriteLine($"Popped: {popped}");
        Console.WriteLine(buffer);
        Assert.AreEqual('B', popped);

        popped = buffer.Pop();
        Console.WriteLine(buffer);
        Assert.AreEqual('C', popped);

        popped = buffer.Pop();
        Console.WriteLine(buffer);
        Assert.AreEqual('D', popped);

        buffer.Add('G');
        Console.WriteLine(buffer);
        CollectionAssert.AreEqual(
            new char[] { 'E', 'F', 'G' }, buffer.ToArray());

        buffer.Pop();
        buffer.Pop();
        buffer.Pop();
        Console.WriteLine(buffer);
        Assert.AreEqual(0, buffer.Count);
    }
}
