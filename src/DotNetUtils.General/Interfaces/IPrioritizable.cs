using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetUtils.General.Interfaces
{
    public interface IPrioritizable<T>
        where T : class, IPrioritizable<T>
    {
        Int32 Priority { get; set; }
    }
}
