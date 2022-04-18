using Minnow.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
    public static class IEnumerableExtensions
    {
        public static Map<TKey1, TKey2> ToMap<TKey1, TKey2, TValue>(
            this IEnumerable<TValue> elements,
            Func<TValue, TKey1> keySelector1,
            Func<TValue, TKey2> keySelector2)
        {
            return new Map<TKey1, TKey2>(elements.Select(keySelector1), elements.Select(keySelector2));
        }

        public static DoubleDictionary<TKey1, TKey2, TValue> ToDoubleDictionary<TKey1, TKey2, TValue>(
            this IEnumerable<TValue> elements,
            Func<TValue, TKey1> keySelector1,
            Func<TValue, TKey2> keySelector2)
        {
            return new DoubleDictionary<TKey1, TKey2, TValue>(
                keySelector1,
                keySelector2,
                elements);
        }

        public static DoubleDictionary<TKey1, TKey2, TValue> ToDoubleDictionary<TKey1, TKey2, TValue, TInput>(
            this IEnumerable<TInput> input,
            Func<TInput, TKey1> keySelector1,
            Func<TInput, TKey2> keySelector2,
            Func<TInput, TValue> elementSelector)
        {
            var kkvps = input.Select(i => (keySelector1(i), keySelector2(i), elementSelector(i)));

            return new DoubleDictionary<TKey1, TKey2, TValue>(kkvps);
        }

        public static DoubleDictionary<TKey1, TKey2, TValue> ToDoubleDictionary<TKey1, TKey2, TValue>(
            this IEnumerable<(TKey1 key1, TKey2 key2, TValue value)> kkvps)
        {
            return new DoubleDictionary<TKey1, TKey2, TValue>(kkvps);
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
                action(item);
        
            return source;
        }
        
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T, Int32> action)
        {
            Int32 index = 0;
            foreach (T item in source)
                action(item, index++);
        
            return source;
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.MinBy(selector, null);
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey>? comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");
            comparer ??= Comparer<TKey>.Default;

            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence contains no elements");
                }
                var min = sourceIterator.Current;
                var minKey = selector(min);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, minKey) < 0)
                    {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }
                return min;
            }
        }

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.MaxBy(selector, null);
        }

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey>? comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");
            comparer ??= Comparer<TKey>.Default;

            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence contains no elements");
                }
                var max = sourceIterator.Current;
                var maxKey = selector(max);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, maxKey) > 0)
                    {
                        max = candidate;
                        maxKey = candidateProjected;
                    }
                }
                return max;
            }
        }

        /// <summary>
        /// Attempt to run <paramref name="func"/> within
        /// the LINQ <see cref="IEnumerable{T}.Aggregate"/>
        /// method. If no items exist within the recieved <paramref name="source"/>
        /// then simply return the <paramref name="fallback"/> value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="func"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public static T TryAggregate<T>(this IEnumerable<T> source, Func<T, T, T> func, T fallback)
        {
            if (source.Any())
                return source.Aggregate(func);
            else
                return fallback;
        }

        public static Boolean TryGetElementAt<T>(this IEnumerable<T> source, Int32 index, [MaybeNullWhen(false)] out T? instance)
        {
            instance = source.ElementAtOrDefault(index);
            return instance is null;
        }

        public static T Random<T>(this IEnumerable<T> source, Random? rand = null)
        {
            rand ??= new Random();
            var skip = (int)(rand.NextDouble() * source.Count());
            return source.Skip(skip).Take(1).First();
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, params T[] items)
            => source.Concat(items.AsEnumerable());
    }
}
