using System;
using System.Collections.Generic;
using System.Text;

namespace Minnow.DependencyInjection
{
    public enum ServiceLifetime
    {
        Singleton,
        Scoped,
        Transient
    }
}
