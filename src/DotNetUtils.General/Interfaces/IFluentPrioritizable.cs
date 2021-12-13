using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetUtils.General.Interfaces
{
    public interface IFluentPrioritizable<T> : IPrioritizable
        where T : class, IFluentPrioritizable<T>
    {
    }

    public static class IFluentPrioritizableExtensions
    {
        public static T SetPriority<T>(this IFluentPrioritizable<T> prioritizable, Int32 priority)
            where T : class, IFluentPrioritizable<T>
        {
            prioritizable.Priority = priority;

            return prioritizable as T;
        }
    }
}
