using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetUtils.General.Interfaces
{
    public interface IOrderable<T>
        where T : class, IOrderable<T>
    {
        Int32 Order { get; set; }
    }
}
