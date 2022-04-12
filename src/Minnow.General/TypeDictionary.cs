using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minnow.General
{
    public class TypeDictionary<TValue>
    {
        private Dictionary<Type, TValue> _dict;

        public TypeDictionary()
        {
            _dict = new Dictionary<Type, TValue>();
        }

        public TypeDictionary(IEnumerable<TValue> items)
        {
            _dict = items.GroupBy(i => i.GetType())
                .ToDictionary(g => g.Key, g => g.First());
        }

        public T Get<T>()
            where T : TValue
        {
            return (T)_dict[typeof(T)];
        }

        public void Add<T>(T value)
            where T : TValue
        {
            _dict.Add(typeof(T), value);
        }

        public bool TryAdd<T>(T value)
            where T : TValue
        {
            return _dict.TryAdd(typeof(T), value);
        }
    }
}
