using DotNetUtils.General.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> PrioritizeBy<T, TKey>(this IEnumerable<T> prioritizables, Func<T, TKey> prioritizer)
            where T : class, IPrioritizable<T>
        {
            return prioritizables.GroupBy(prioritizer).Select(g => g.MaxBy(p => p.Priority));
        }

        public static IEnumerable<T> Order<T>(this IEnumerable<T> orderables)
            where T : class, IOrderable<T>
        {
            return orderables.OrderBy(o => o.Order);
        }
    }
}
