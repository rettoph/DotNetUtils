using System;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
    public static class ListExtensions
    {
        public static void TryAddRange<T>(this List<T> list, IEnumerable<T> collection)
        {
            if (collection?.Any() ?? false)
                list.AddRange(collection);
        }

        public static T AddAndReturn<T>(this IList<T> list, T item)
        {
            list.Add(item);
            return item;
        }

        public static T AddAndReturn<T, TList>(this IList<TList> list, T item)
            where T : TList
        {
            list.Add(item);
            return item;
        }
    }
}
