using DotNetUtils.General.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetUtils.General.Interfaces
{
    public static class IOrderableExtensions
    {
        public static T SetOrder<T>(this IOrderable<T> orderable, Int32 order)
            where T : class, IOrderable<T>
        {
            orderable.Order = order;

            return orderable as T;
        }
    }
}
