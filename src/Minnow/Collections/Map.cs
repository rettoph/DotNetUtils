using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minnow.Collections
{
    public class Map<T1, T2>
    {
        public class Indexer<T3, T4>
        {
            private Dictionary<T3, T4> _dictionary;

            public IEnumerable<T3> Keys => _dictionary.Keys;
            public IEnumerable<T4> Values => _dictionary.Values;

            public Indexer(Dictionary<T3, T4> dictionary)
            {
                _dictionary = dictionary;
            }
            public T4 this[T3 index]
            {
                get => _dictionary[index];
            }
        }

        private Dictionary<T1, T2> _forward = new Dictionary<T1, T2>();
        private Dictionary<T2, T1> _reverse = new Dictionary<T2, T1>();

        public Indexer<T1, T2> Forward { get; private set; }
        public Indexer<T2, T1> Reverse { get; private set; }

        public ICollection<T1> Values1 => _forward.Keys;
        public ICollection<T2> Values2 => _reverse.Keys;

        public T2 this[T1 key] => this.Forward[key];
        public T1 this[T2 key] => this.Reverse[key];

        public Map(IEnumerable<T1> first, IEnumerable<T2> second) : this(first.Zip(second))
        {
        }

        public Map(IEnumerable<(T1, T2)> values) : this()
        {
            _forward = values.ToDictionary(
                keySelector: v => v.Item1,
                elementSelector: v => v.Item2);

            _reverse = values.ToDictionary(
                keySelector: v => v.Item2,
                elementSelector: v => v.Item1);
        }

        public Map()
        {
            this.Forward = new Indexer<T1, T2>(_forward);
            this.Reverse = new Indexer<T2, T1>(_reverse);
        }

        public void Add(T1 t1, T2 t2)
        {
            if(_forward.TryAdd(t1, t2))
            {
                if(!_reverse.TryAdd(t2, t1))
                {
                    _forward.Remove(t1);
                }
            }
        }
    }
}
