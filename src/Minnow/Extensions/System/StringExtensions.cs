using K4os.Hash.xxHash;
using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class StringExtensions
    {
#if DEBUG
        public static Dictionary<string, uint> Lookup = new Dictionary<string, uint>();
#endif
        public static uint xxHash(this string value)
        {
#if DEBUG
            if(Lookup.TryGetValue(value, out uint hash))
            {
                return hash;
            }

            hash = XXH32.DigestOf(Encoding.UTF8.GetBytes(value));

            Lookup.Add(value, hash);

            return hash;
#endif

            return XXH32.DigestOf(Encoding.UTF8.GetBytes(value));
        }
    }
}
