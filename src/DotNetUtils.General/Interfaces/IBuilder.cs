using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetUtils.General.Interfaces
{
    public interface IBuilder<T>
    {
        T Build();
    }
}
