using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetUtils.General.Interfaces
{
    public interface IFluentOrderable<T> : IOrderable
        where T : class, IFluentOrderable<T>
    {
    }

    public static class IFluentOrderableExtensions
    {
        public static T SetOrder<T>(this IFluentOrderable<T> orderable, Int32 order)
            where T : class, IFluentOrderable<T>
        {
            orderable.Order = order;

            return orderable as T;
        }
    }
}
