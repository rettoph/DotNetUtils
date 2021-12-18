using System;
using System.Collections.Generic;
using System.Text;

namespace Minnow.General.Interfaces
{
    public interface IBuilder<T>
    {
        T Build();
    }
}
