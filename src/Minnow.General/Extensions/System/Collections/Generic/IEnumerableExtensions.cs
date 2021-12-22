using Minnow.General;
using Minnow.General.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
    public static class IEnumerableExtensions
    {
        public static DoubleDictionary<TKey1, TKey2, TValue> ToDoubleDictionary<TKey1, TKey2, TValue>(
            this IEnumerable<TValue> values,
            Func<TValue, TKey1> keySelector1,
            Func<TValue, TKey2> keySelector2)
        {
            return new DoubleDictionary<TKey1, TKey2, TValue>(
                keySelector1,
                keySelector2,
                values);
        }

        public static DoubleDictionary<TKey1, TKey2, TValue> ToDoubleDictionary<TKey1, TKey2, TValue>(
            this IEnumerable<(TKey1 key1, TKey2 key2, TValue value)> kkvps)
        {
            return new DoubleDictionary<TKey1, TKey2, TValue>(kkvps);
        }

        public static IEnumerable<T> PrioritizeBy<T, TKey>(this IEnumerable<T> prioritizables, Func<T, TKey> prioritizer)
            where T : class, IPrioritizable
        {
            return prioritizables.GroupBy(prioritizer).Select(g => g.MaxBy(p => p.Priority));
        }

        public static IEnumerable<T> Order<T>(this IEnumerable<T> orderables)
            where T : class, IOrderable
        {
            return orderables.OrderBy(o => o.Order);
        }
    }
}
