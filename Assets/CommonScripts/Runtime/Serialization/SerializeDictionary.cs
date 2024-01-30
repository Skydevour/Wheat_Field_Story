using System;
using System.Collections.Generic;
using UnityEngine;

namespace CommonFramework.Runtime.Serialization
{
    [Serializable]
    public class SerializeDictionary<K, V> : Dictionary<K, V>, ISerializationCallbackReceiver
    {
        [SerializeField]
        [System.Reflection.Obfuscation(Exclude = true)]
        List<K> keys = new List<K>();

        [SerializeField]
        [System.Reflection.Obfuscation(Exclude = true)]
        List<V> values = new List<V>();

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            foreach (KeyValuePair<K, V> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            if (keys != null && values != null && keys.Count == values.Count)
            {
                this.Clear();
                for (int i = 0; i < keys.Count; ++i)
                {
                    if (keys[i] == null || values[i] == null)
                    {
                        continue;
                    }

                    this.Add(keys[i], values[i]);
                }
            }
        }
    }
}