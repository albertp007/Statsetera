namespace Statsetera;

public static class Stats
{
    /// <summary>
    ///  Given a stream of data, calculate its exponential weighted moving
    ///  average.
    /// </summary>
    /// <typeparam name="T">type of items in sequence</typeparam>
    /// <param name="xs">enumerable of data points</param>
    /// <param name="decay">weight of all previous values and 1-decay is the weight of the current item</param>
    /// <param name="toDouble">converts an item of type T to double</param>
    /// <returns>the exponential weighted average</returns>
    public static double Ewma<T>(this IEnumerable<T> xs, double decay, 
        Func<T, double> toDouble)
    {
        return xs.Select(x => toDouble(x)).Aggregate(
            (acc, x) => (1-decay)*x + decay*acc);
    }
    
    /// <summary>
    /// Given a stream of data of type double, calculate its exponential weighted moving average.
    /// </summary>
    /// <param name="xs">enumerable of data points</param>
    /// <param name="decay">weight of all previous values and 1-decay is the weight of the current item</param>
    /// <returns>the exponential weighted average</returns>
    public static double Ewma(IEnumerable<double> xs, double decay)
    {
        return Ewma(xs, decay, x => x);
    }

    /// <summary>
    /// Calculates the mean of a sequence incrementally
    /// </summary>
    /// <typeparam name="T">type of items in sequence</typeparam>
    /// <param name="seq">input sequence</param>
    /// <param name="toDouble">converts an item of type T to double</param>
    /// <returns>each element in the return sequence is the updated mean for each input element</returns>
    public static IEnumerable<double> MeanIncremental<T>(
        this IEnumerable<T> seq, Func<T, double> toDouble)
    {
        double total = 0;
        int count = 0;
        foreach (T current in seq)
        {
            total += toDouble(current);
            ++count;
            yield return total / count;
        }
    }

    /// <summary>
    /// Calculates the sum of square and the sum of a sequence
    /// </summary>
    /// <typeparam name="T">type of items in sequence</typeparam>
    /// <param name="seq">input sequence</param>
    /// <param name="toDouble">converts an item of type T to double</param>
    /// <returns>tuple containing the sum of squares in the first element, the sum in the second elemment and the number of items in the sequence in the last item</returns>
    public static (double, double, int) SumOfSquares<T>(this IEnumerable<T> seq,
        Func<T, double> toDouble)
    {
        /// Item1 has sum of (Xi square)
        /// Item2 has sum of Xi
        /// Item3 has number number of elements in the sequence
        return seq.Select(x => toDouble(x)).Aggregate(
            (0.0, 0.0, 0), 
            (acc, x) => (acc.Item1 + x * x, acc.Item2 + x, acc.Item3 + 1));
    }

    /// <summary>
    /// Calculates the sum of cubes, sum of squares, and the sum of the input sequence
    /// </summary>
    /// <typeparam name="T">type of items in sequence</typeparam>
    /// <param name="seq">the input sequence</param>
    /// <param name="toDouble">converts an item of type T to double</param>
    /// <returns>the sum of cubes in the first element, sum of squares in the second element, sum in the third element and the number of items in the last item</returns>
    public static (double, double, double, int) SumOfCubes<T>(
        this IEnumerable<T> seq, Func<T, double> toDouble)
    {
        /// Item1 has sum of (Xi square)
        /// Item2 has sum of Xi
        /// Item3 has number number of elements in the sequence
        return seq.Select(x => toDouble(x)).Aggregate(
            (0.0, 0.0, 0.0, 0),
            (acc, x) =>
            {
                double square = x * x;
                double cube = square * x;
                return (acc.Item1 + cube, acc.Item2 + square, acc.Item3 + x,
                    acc.Item4 + 1);
            });
    }

    /// <summary>
    /// Calculates the sum of error squared of a sequence. i.e. sum of (Xi - Xbar)^2
    /// </summary>
    /// <typeparam name="T">type of items in sequence</typeparam>
    /// <param name="seq">the input sequence</param>
    /// <param name="toDouble">converts an item of type T to double</param>
    /// <returns>the sum of error squared in the first item and the number of items in the sequence in the last item</returns>
    public static (double, int) SumOfErrorSquared<T>(this IEnumerable<T> seq,
        Func<T, double> toDouble)
    {
        return seq.SumOfSquares(toDouble).Then(t =>
        {
            (double ssq, double sum, int n) = t;
            return (ssq - sum * sum / n, n);
        });
    }

    /// <summary>
    /// Calculates the population variance of a sequence of data
    /// </summary>
    /// <typeparam name="T">type of items in sequence</typeparam>
    /// <param name="seq">the input sequence</param>
    /// <param name="toDouble">converts an item of type T to double</param>
    /// <returns>the population variance</returns>
    public static double PopulationVariance<T>(this IEnumerable<T> seq,
        Func<T, double> toDouble)
    {
        return seq.SumOfErrorSquared(toDouble).Then(x => x.Item1 / x.Item2);

    }

    /// <summary>
    /// Calculates the sample variance of a sequence of data
    /// </summary>
    /// <typeparam name="T">type of items in sequence</typeparam>
    /// <param name="seq">the input sequence</param>
    /// <param name="toDouble">converts an item of type T to double</param>
    /// <returns>the sample variance</returns>
    public static double SampleVariance<T>(this IEnumerable<T> seq,
        Func<T, double> toDouble)
    {
        return seq.SumOfErrorSquared(toDouble).Then(
            // using (n-1) as denominator
            ssq => ssq.Item1 / (ssq.Item2 - 1) );
    }
    
    /// <summary>
    /// Calculates the sample standard deviation of a sequence of data
    /// </summary>
    /// <typeparam name="T">type of items in sequence</typeparam>
    /// <param name="seq">the input sequence</param>
    /// <param name="toDouble">converts an item of type T to double</param>
    /// <returns>the sample standard deviation</returns>
    public static double SampleStdDev<T>(this IEnumerable<T> seq, 
        Func<T, double> toDouble)
    {
        return seq.SampleVariance(toDouble).Then(Math.Sqrt);
    }
    
    /// <summary>
    /// Calculates the population standard deviation
    /// </summary>
    /// <typeparam name="T">type of items in the sequence</typeparam>
    /// <param name="seq">the input sequence</param>
    /// <param name="toDouble">converts an item of type T to double</param>
    /// <returns>the population standard deviation</returns>
    public static double PopulationStdDev<T>(this IEnumerable<T> seq, 
        Func<T, double> toDouble)
    {
        return seq.PopulationVariance(toDouble).Then(Math.Sqrt);
    }
    
    /// <summary>
    /// Calculates the sample standard of deviation of a sequence of doubles
    /// </summary>
    /// <param name="seq">the input sequence</param>
    /// <returns>the sample standard deviation</returns>
    public static double SampleStdDev(this IEnumerable<double> seq)
    {
        return SampleStdDev(seq, x => x);
    }

    /// <summary>
    /// Calculates the population standard deviation of a sequence of doubles
    /// </summary>
    /// <param name="seq">the input sequence</param>
    /// <returns>the populuation standard deviation</returns>
    public static double PopulationStdDev(this IEnumerable<double> seq)
    {
        return PopulationStdDev(seq, x => x);
    }

    /// <summary>
    /// Calculates the sample skew of a sequence of data
    /// </summary>
    /// <typeparam name="T">type of items in the sequence</typeparam>
    /// <param name="seq">the input sequence</param>
    /// <param name="toDouble">converts an item of type T to double</param>
    /// <returns>the sample skew</returns>
    public static double SampleSkew<T>(this IEnumerable<T> seq, 
        Func<T, double> toDouble)
    {
        (double sumOfCubes, double sumOfSquares, double sum, int n) =
            seq.SumOfCubes(toDouble);

        double sumDeviationCubed = sumOfCubes - 3*sum*sumOfSquares/n + 
            2*sum*sum*sum/n/n;
        double sampleStd = Math.Sqrt((sumOfSquares - sum*sum/n)/(n-1));
        return sumDeviationCubed/Math.Pow(sampleStd, 3)*n/(n-1)/(n-2);
    }

    /// <summary>
    /// Calculates the sample skew of a sequence of doubles
    /// </summary>
    /// <param name="seq">the input sequence</param>
    /// <returns>the sample skew</returns>
    public static double SampleSkew(this IEnumerable<double> seq)
    {
        return SampleSkew(seq, x => x);
    }

    /// <summary>
    /// Creates an empirical distribution function given a sequence of data, treating each item as a point mass.
    /// </summary>
    /// <param name="data">the input sequence of data</param>
    /// <returns>a function which takes a double value x and returns the fraction of data items which has value smaller than x, i.e. the probability of the event X < x</returns>
    public static Func<double, double, (double, double, double)>
        EmpiricalDistributionFunction(IEnumerable<double> data)
    {
        IEnumerable<double> sorted = data.OrderBy(x => x);
        // sorted set is for doing close match later
        SortedSet<double> sortedSet = new SortedSet<double>();
        Dictionary<double, int> fTable = new Dictionary<double, int>();
        
        int count = 0;
        foreach(double t in sorted)
        {
            count++;
            sortedSet.Add(t);
            fTable[t] = count;
        }
        Func<double, double> cdf = x =>
        {
            if (x < sortedSet.Min)
            {
                return 0.0;
            }
            else if (x > sortedSet.Max)
            {
                return 1.0;
            }
            else
            {
                double low = sortedSet.GetViewBetween(double.MinValue, x).Max;
                return fTable[low] / (double)count;
            }
        };
        Func<double, double> epsilon = alpha => 
            Math.Sqrt(Math.Log(2 / alpha) / count / 2);

        return (x, alpha) =>
        {
            var p = cdf(x);
            var e = epsilon(alpha);
            var l = Math.Max(p - e, 0);
            var u = Math.Min(p + e, 1);
            return (p, l, u);
        };
    }

    /// <summary>
    /// Generic function for bootstrap resampling to calculate a statistic and its standard error
    /// </summary>
    /// <typeparam name="T">type of the input data</typeparam>
    /// <param name="samples">the input samples</param>
    /// <param name="n">number of rounds of resampling</param>
    /// <param name="resample">function that does the resampling given the original samples</param>
    /// <param name="statistic">function to calculate the statistic given the re-sampled samples</param>
    /// <returns>a pair with the calculated statistic in the first element and its standard error in the second element</returns>
    public static (double, double) Bootstrap<T>(
        this IEnumerable<T> samples, int n,
        Func<T[], T[]> resample, 
        Func<T[], double> statistic)
    {
        var sampleArray = samples.ToArray();
        var bootstrap = Enumerable.Range(0, n)
            .Select(_ => resample(sampleArray))
            .Select(sample => statistic(sample));
        return (bootstrap.Average(), bootstrap.PopulationStdDev());
    }
}
