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
    public static RandomSource DefaultRandomSource(int? seed = null) => 
        seed.HasValue ? new MersenneTwister(seed.Value) :
            new MersenneTwister();
    public static double NaiveIntegrate(int n, double a, double b, 
        Func<double, double> f, RandomSource? rng = null)
    {
        rng ??= DefaultRandomSource();
        var randomSeq = rng.NextDoubleSequence();
        return randomSeq
            .Take(n)
            .Select(x => x * (b - a) + a) // scale [0, 1] to [a, b]
            .Select(x => f(x))
            .Average();
    }

    public static IEnumerable<int> RandomWalk(double threshold = 0.5,
        int a = 1, int b = -1, RandomSource? rng = null)
    {
        if (threshold < 0 || threshold > 1)
        {
            throw new ArgumentOutOfRangeException(
                "threshold probability must be between 0 and 1 inclusive");
        }
        rng ??= DefaultRandomSource();
        var randomSeq = rng.NextDoubleSequence();
        return randomSeq.Accumulate(0,
            (acc, current) => (current <= threshold) ? acc + a : acc + b);
    }

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
