using DotNetUtils.General.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetUtils.General.Interfaces
{
    public static class IPrioritizableExtensions
    {
        public static T SetPriority<T>(this IPrioritizable<T> prioritizable, Int32 priority)
            where T : class, IPrioritizable<T>
        {
            prioritizable.Priority = priority;

            return prioritizable as T;
        }
    }
}
