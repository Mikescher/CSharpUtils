using System.Collections.Generic;

namespace MSHC.Lang.Collections
{
    public static class DictionaryExtensions
    {
        public static bool AddIfNotContains<TKey, TVal>(this IDictionary<TKey, TVal> dict, TKey key, TVal val)
        {
            if (dict.ContainsKey(key)) return false;
            dict.Add(key, val);
            return true;
        }
    }
}
