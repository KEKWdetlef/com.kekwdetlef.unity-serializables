using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kekwdetlef.Serializables
{
    [Serializable]
    internal class SKeyValuePair<TKey, TValue>
    {
        [SerializeField] private TKey key;
        [SerializeField] private TValue value;

        internal TKey Key => key;
        internal TValue Value => value;

        internal SKeyValuePair(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }

        internal SKeyValuePair(KeyValuePair<TKey, TValue> pair)
        {
            key = pair.Key;
            value = pair.Value;
        }
    }
}
