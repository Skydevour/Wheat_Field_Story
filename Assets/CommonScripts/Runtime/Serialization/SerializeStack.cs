using System;
using System.Collections.Generic;
using UnityEngine;

namespace CommonFramework.Runtime.Serialization
{
    [Serializable]
    public class SerializeStack<V> : Stack<V>, ISerializationCallbackReceiver
    {
        [SerializeField]
        List<V> values = new List<V>();

        public void OnBeforeSerialize()
        {
            values.Clear();

            for (int i = 0; i < this.Count; i++)
            {
                values.Insert(0,this.Pop());
            }
        }

        public void OnAfterDeserialize()
        {
            this.Clear();

            for (int i = 0; i < values.Count; i++)
            {
                this.Push(values[i]);
            }
        }
    }
}