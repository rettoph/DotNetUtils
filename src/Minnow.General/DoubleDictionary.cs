using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Minnow.General
{
    public class DoubleDictionary<TKey1, TKey2, TValue>
    {
        private readonly Boolean _keySelectorsDefined;
        private readonly Func<TValue, TKey1> _key1Selector;
        private readonly Func<TValue, TKey2> _key2Selector;

        private Dictionary<TKey1, TValue> _dic1;
        private Dictionary<TKey2, TValue> _dic2;

        public TValue this[TKey1 key] => _dic1[key];
        public TValue this[TKey2 key] => _dic2[key];

        public ICollection<TValue> Values => _dic1.Values;
        public ICollection<TKey1> Keys1 => _dic1.Keys;
        public ICollection<TKey2> Keys2 => _dic2.Keys;

        #region Constructors
        public DoubleDictionary()
        {
            _keySelectorsDefined = false;

            _dic1 = new Dictionary<TKey1, TValue>();
            _dic2 = new Dictionary<TKey2, TValue>();
        }
        public DoubleDictionary(
            Func<TValue, TKey1> key1Selector,
            Func<TValue, TKey2> key2Selector)
        {
            _key1Selector = key1Selector;
            _key2Selector = key2Selector;
            _keySelectorsDefined = true;

            _dic1 = new Dictionary<TKey1, TValue>();
            _dic2 = new Dictionary<TKey2, TValue>();
        }
        public DoubleDictionary(
            Func<TValue, TKey1> keySelector1,
            Func<TValue, TKey2> keySelector2,
            IEnumerable<TValue> values)
        {
            _key1Selector = keySelector1;
            _key2Selector = keySelector2;
            _keySelectorsDefined = true;

            _dic1 = values.ToDictionaryByValue(v => keySelector1(v));
            _dic2 = values.ToDictionaryByValue(v => keySelector2(v));
        }
        #endregion

        #region Helper Methods
        public Boolean TryAdd(TKey1 key1, TKey2 key2, TValue value)
        {
            if (_dic1.TryAdd(key1, value))
            {
                if (_dic2.TryAdd(key2, value))
                {
                    return true;
                }

                // Something went wrong, roll back _dic1.
                _dic1.Remove(key1);
            }

            return false;
        }

        public Boolean TryAdd(TValue value)
        {
            
            if(!_keySelectorsDefined)
            {
                Debug.Assert(_keySelectorsDefined, $"{this.GetType().GetPrettyName()}::{nameof(TryAdd)} - Unable to add value. KeySelectors were not defined.");
                return false;
            }

            TKey1 key1 = _key1Selector(value);
            TKey2 key2 = _key2Selector(value);

            return this.TryAdd(key1, key2, value);
        }

        public Boolean TryGetValue(TKey1 key, out TValue value)
        {
            return _dic1.TryGetValue(key, out value);
        }

        public Boolean TryGetValue(TKey2 key, out TValue value)
        {
            return _dic2.TryGetValue(key, out value);
        }

        public void Clear()
        {
            _dic1.Clear();
            _dic2.Clear();
        }
        #endregion
    }
}
