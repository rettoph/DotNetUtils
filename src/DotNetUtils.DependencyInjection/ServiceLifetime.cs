using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetUtils.DependencyInjection
{
    public enum ServiceLifetime
    {
        Singleton,
        Scoped,
        Transient
    }
}
