﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace System
{
    public static class DictionaryHelper
    {
        /// <summary>
        /// Construct a new readonly dictionary that contains values for every
        /// possible enum key and populate with defaults.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> BuildEnumDictionary<TKey, TValue>(IDictionary<TKey, TValue> with = null, TValue fallback = default)
            where TKey : Enum
        {
            return EnumHelper.GetValues<TKey>().ToDictionary(
                keySelector: k => k,
                elementSelector: k => with?.ContainsKey(k) ?? false ? with[k] : fallback);
        }

        /// <summary>
        /// Construct a new readonly dictionary that contains values for every
        /// possible enum key and populate with defaults.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> BuildEnumDictionary<TKey, TValue>(Func<TKey, TValue> fallback, IDictionary<TKey, TValue> with = null)
            where TKey : Enum
        {
            return EnumHelper.GetValues<TKey>().ToDictionary(
                keySelector: k => k,
                elementSelector: k => with?.ContainsKey(k) ?? false ? with[k] : fallback(k));
        }
    }
}
