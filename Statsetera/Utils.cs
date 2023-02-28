using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statsetera;

public static class Utils
{
    /// <summary>
    /// Lags a sequence of items
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="seq"></param>
    /// <param name="lag"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static IEnumerable<(T Lagged, T Current)> Lag<T>(
        this IEnumerable<T> seq, int lag)
    where T : IComparable<T>
    {
        if (lag <= 0)
        {
            throw new ArgumentException("lag must be strictly positive");
        }
        CircularBuffer<T> buffer = new CircularBuffer<T>(lag);
        foreach (T current in seq)
        {
            if (buffer.Count == lag)
            {
                // buffer is already full, pop the oldest element
                // first before adding the current element
                T laggedItem = buffer.Pop();
                buffer.Add(current);
                yield return (Lagged: laggedItem, Current: current);
            }
            else
            {
                buffer.Add(current);
            }
        }
    }

    /// <summary>
    /// Processes a sequence of data given an initial state value which is then updated by the input function called accumulate, given the current state and the value of the current item in the sequence, returning a sequence of state values
    /// </summary>
    /// <typeparam name="T">type of items in sequence</typeparam>
    /// <typeparam name="TAcc">type of the initial state value</typeparam>
    /// <param name="seq">the input sequence</param>
    /// <param name="initial">the initial state value</param>
    /// <param name="accumulate">function which takes a current state value of type TAcc, a current value of type T in the sequence and returns a new state value of type TAcc</param>
    /// <returns></returns>
    public static IEnumerable<TAcc> Accumulate<T, TAcc>(this IEnumerable<T> seq,
        TAcc initial, Func<TAcc, T, TAcc> accumulate)
    {
        TAcc accumulator = initial;
        foreach (T current in seq)
        {
            accumulator = accumulate(accumulator, current);
            yield return accumulator;
        }
    }
    public static IEnumerable<T> QuickSort<T>(this IEnumerable<T> seq)
    where T : IComparable<T>
    {
        if (seq.Count() == 0)
        {
            return Enumerable.Empty<T>();
        }
        else
        {
            T first = seq.First();
            return Enumerable.Empty<T>();
        }
    }

    public static IEnumerable<T> InsertionSort<T>(this IEnumerable<T> seq)
    where T : IComparable<T>
    {
        T[] arr = seq.ToArray();
        int size = arr.Count();
        // trivial cases
        switch (size)
        {
            case 0:
                return Enumerable.Empty<T>();

            case 1:
                return seq;

            default:
                for (int i = 1; i < size; ++i)
                {
                    T key = arr[i];
                    int j = i - 1;
                    while (j >= 0 && -key.CompareTo(arr[j]) < 0)
                    {
                        arr[j + 1] = arr[j];
                        --j;
                    }
                    arr[j + 1] = key;
                }
                return arr;
        }
    }

    /// <summary>
    /// Extension function which creates a composite function from two functions f and g which calls function f first then pipe the result into the function g. The intention is that the sequence of calling the functions follows the sequence of the functions as they appear in code.  Instead of writing g(f(x)), we can write something similar to f->g(x)
    /// </summary>
    /// <typeparam name="A">input type of the first function</typeparam>
    /// <typeparam name="B">output type of the first function and also the input type of the second function</typeparam>
    /// <typeparam name="C">output type of the second function</typeparam>
    /// <param name="f">First function to be called</param>
    /// <param name="g">Second function to be called, taking the output of the first function as the input</param>
    /// <returns>a composite function the equivalent of g(f(x))</returns>
    public static Func<A, C> Then<A, B, C>(this Func<A, B> f, Func<B, C> g)
    => (n) => g(f(n));

    /// <summary>
    /// Extension function which applies a function f to an input type T1 and transforms it into a type T2. This facilitates the piping of output of one function to another function.
    /// <typeparam name="T1">input type of the function</typeparam>
    /// <typeparam name="T2">output type of the function</typeparam>
    /// <param name="t1">input value</param>
    /// <param name="f">function to be applied to the input value</param>
    /// <returns>the output of function f</returns>
    public static T2 Then<T1, T2>(this T1 t1, Func<T1, T2> f) => f(t1);

    /// <summary>
    /// Extension function to unzip an enumerable of pairs into 2 enumerables
    /// </summary>
    /// <typeparam name="T1">type of the first element of the pair</typeparam>
    /// <typeparam name="T2">type of the second element of the pair</typeparam>
    /// <param name="seq">input sequence</param>
    /// <returns>a pair of IEnumerable of type T1 and T2</returns>
    public static (IEnumerable<T1>, IEnumerable<T2>)
    Unzip<T1, T2>(this IEnumerable<(T1, T2)> seq) =>
        (seq.Select(x => x.Item1), seq.Select(x => x.Item2));

    /// <summary>
    /// Extension function to unzip an enumerable of triplets into 3 enumerables
    /// </summary>
    /// <typeparam name="T1">type of the first element of the triplet</typeparam>
    /// <typeparam name="T2">type of the second element of the triplet</typeparam>
    /// <typeparam name="T3">type of the third element of the triplet</typeparam>
    /// <param name="seq">input sequence</param>
    /// <returns>a triplet of IEnumerables of type T1, T2 and T3</returns>
    public static (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>)
        Unzip<T1, T2, T3>(this IEnumerable<(T1, T2, T3)> seq) =>
            (seq.Select(x => x.Item1), seq.Select(x => x.Item2),
             seq.Select(x => x.Item3));

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <param name="seq"></param>
    /// <returns></returns>
    public static (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>,
        IEnumerable<T4>)
        Unzip<T1, T2, T3, T4>(this IEnumerable<(T1, T2, T3, T4)> seq) => (
            seq.Select(x => x.Item1),
            seq.Select(x => x.Item2),
            seq.Select(x => x.Item3),
            seq.Select(x => x.Item4)
        );

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <param name="seq"></param>
    /// <returns></returns>
    public static (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>,
        IEnumerable<T4>, IEnumerable<T5>)
        Unzip<T1, T2, T3, T4, T5>(
        this IEnumerable<(T1, T2, T3, T4, T5)> seq) => (
            seq.Select(x => x.Item1),
            seq.Select(x => x.Item2),
            seq.Select(x => x.Item3),
            seq.Select(x => x.Item4),
            seq.Select(x => x.Item5)
        );

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <param name="seq"></param>
    /// <returns></returns>
    public static (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>,
        IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>)
        Unzip<T1, T2, T3, T4, T5, T6>(
        this IEnumerable<(T1, T2, T3, T4, T5, T6)> seq) => (
            seq.Select(x => x.Item1),
            seq.Select(x => x.Item2),
            seq.Select(x => x.Item3),
            seq.Select(x => x.Item4),
            seq.Select(x => x.Item5),
            seq.Select(x => x.Item6)
        );
}
