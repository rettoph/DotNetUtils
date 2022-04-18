using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minnow.Collections.Concurrent
{
    public class ConcurrentFactory<T> : Factory<T, ConcurrentPool<T>>
        where T : class
    {
        public ConcurrentFactory(Func<T> method, UInt16 poolSize = 50) : base(method, new ConcurrentPool<T>(ref poolSize))
        {
        }
    }
}
