using System.Collections.Generic;

namespace CommonFramework.Runtime.Extensions
{
    /// <summary>
    /// Dictionary extension helper class to replace or add items
    /// </summary>
    public static class DictionaryExtension
    {
        public static Dictionary<TKey, TValue> TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key) == false) dict.Add(key, value);
            return dict;
        }

        public static Dictionary<TKey, TValue> AddOrReplace<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            dict[key] = value;
            return dict;
        }

    }
}
