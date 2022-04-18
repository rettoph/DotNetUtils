using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minnow.Collections.Concurrent
{
    public class ConcurrentPool<T> : IPool<T>
            where T : class
    {
        private ConcurrentStack<T> _pool;
        private UInt16 _poolSize;
        private UInt16 _maxPoolSize;

        public ConcurrentPool(UInt16 maxPoolSize) : this(ref maxPoolSize)
        {

        }
        public ConcurrentPool(ref UInt16 maxPoolSize)
        {
            _maxPoolSize = maxPoolSize;
            _poolSize = 0;
            _pool = new ConcurrentStack<T>();
        }

        /// <inheritdoc />
        public virtual Boolean Any()
            => _pool.Any();

        /// <inheritdoc />
        public virtual Boolean TryPull(out T instance)
        {
            if(_pool.TryPop(out instance))
            {
                _poolSize--;
                return true;
            }    

            return false;
        }

        /// <inheritdoc />
        public virtual Boolean TryReturn(T instance)
        {
            if (_poolSize < _maxPoolSize)
            {
                _pool.Push(instance);
                _poolSize++;
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public virtual Int32 Count()
            => _poolSize;
    }
}
