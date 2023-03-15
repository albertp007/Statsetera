using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Random;

namespace Statsetera;

public static class MonteCarlo
{
    /// <summary>
    /// Naive integration using Monte Carlo Estimator
    /// </summary>
    /// <param name="n">Number of uniform random numbers to generate</param>
    /// <param name="a">integrate from a</param>
    /// <param name="b">integrate to b</param>
    /// <param name="f">the function to integrate</param>
    /// <param name="rng">random number generator.  default is a new instance of Mersenee twister created by DefaultRandomSource</param>
    /// <returns>the integration of the function f from a to b</returns>
    public static double NaiveIntegrate(int n, double a, double b, 
        Func<double, double> f, RandomSource? rng = null)
    {
        rng ??= Utils.DefaultRandomSource();
        var randomSeq = rng.NextDoubleSequence();
        return randomSeq
            .Take(n)
            .Select(x => x * (b - a) + a) // scale [0, 1] to [a, b]
            .Select(x => f(x))
            .Average();
    }

    /// <summary>
    /// Generates a path of a random walk
    /// </summary>
    /// <param name="threshold">probability for choosing value a</param>
    /// <param name="a">this value will be added to the previous point with probability specified in the threshold parameter</param>
    /// <param name="b">this value will be added to the previous point if a is not chosen</param>
    /// <param name="rng">random number generator.  default is a new instance of Mersenee twister created by DefaultRandomSource</param>
    /// <returns>a sequence representing the path of a random walk</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static IEnumerable<int> RandomWalk(double threshold = 0.5,
        int a = 1, int b = -1, RandomSource? rng = null)
    {
        if (threshold < 0 || threshold > 1)
        {
            throw new ArgumentOutOfRangeException(
                "threshold probability must be between 0 and 1 inclusive");
        }
        rng ??= Utils.DefaultRandomSource();
        var randomSeq = rng.NextDoubleSequence();
        return randomSeq.Accumulate(0,
            (acc, current) => (current <= threshold) ? acc + a : acc + b);
    }

    /// <summary>
    /// Yet another implementation of the linear congruent random number generator.  Do NOT use
    /// </summary>
    /// <param name="seed"></param>
    /// <param name="range"></param>
    /// <param name="multiplier"></param>
    /// <param name="increment"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static IEnumerable<int> LinearCongruent(int seed, int range, 
        int multiplier, int increment)
    {
        if ( range < 1 )
        {
            throw new ArgumentOutOfRangeException("Range must be > 1");
        }
        if ( seed  < 0 || seed >= range )
        {
            throw new ArgumentOutOfRangeException(
                $"Seed must be between 0 and {range-1} inclusive");
        }
        if ( multiplier < 1 || multiplier >= range )
        {
            throw new ArgumentOutOfRangeException(
                $"multiplier must be between 1 and {range - 1} inclusive");
        }
        if ( increment < 0 || increment >= range )
        {
            throw new ArgumentOutOfRangeException(
                $"increment must be between 0 and {range - 1} inclusive");
        }
        int x = seed;
        while( true )
        {
            x = (multiplier * x + increment) % range;
            yield return x;
        }
    }
}
