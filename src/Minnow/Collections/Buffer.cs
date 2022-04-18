using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minnow.Collections
{
    public class Buffer<T> : IEnumerable<T>
    {
        private T[] _buffer;
        private int _length;

        public Buffer(int capacity)
        {
            _buffer = new T[capacity];
            _length = 0;
        }

        public void Add(T item)
        {
            _buffer[_length++] = item;
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach(T item in items)
            {
                this.Add(item);
            }
        }

        public void Reset(int capacity)
        {
            this.Reset();

            if (_buffer.Length == capacity)
            {
                return;
            }

            _buffer = new T[capacity];
        }
        public void Reset()
        {
            _length = 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _length; i++)
            {
                yield return _buffer[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
