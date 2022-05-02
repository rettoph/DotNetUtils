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
        private T[] _array;
        private int _position;

        public readonly int Length;
        public int Position => _position;
        public int Size => Math.Min(this.Position, this.Length);
        public T[] Array => _array;
        public T this[int index] => _array[index];

        public Buffer(int length)
        {
            _array = new T[length];
            _position = 0;

            this.Length = length;
        }

        public void Add(T item)
        {
            _array[_position++ % this.Length] = item;
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

            if (_array.Length == capacity)
            {
                return;
            }

            _array = new T[capacity];
        }
        public void Reset()
        {
            _position = 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if(_position < this.Length)
            {
                for (int i = 0; i < _position; i++)
                {
                    yield return _array[i];
                }
            }
            else
            {
                for(int i=0; i<this.Length; i++)
                {
                    yield return _array[(_position + i) % this.Length];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
