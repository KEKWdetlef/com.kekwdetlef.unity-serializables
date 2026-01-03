using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kekwdetlef.Serializables
{
    [Serializable]
    public sealed class SDictionary<TKey, TValue> : ISerializationCallbackReceiver
    {

#if UNITY_EDITOR

        public const string NewValueProperty_Editor = nameof(newValue_editor);
        [SerializeField] private SKeyValuePair<TKey, TValue> newValue_editor;

#endif // UNITY_EDITOR

#region Serialization

        public const string SSelfProperty = nameof(sSelf);
        [SerializeField] private SKeyValuePair<TKey, TValue>[] sSelf;

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Raw = new Dictionary<TKey, TValue>();

            if (sSelf == null)
            { return; }

            foreach (SKeyValuePair<TKey, TValue> pair in sSelf)
            {
                Raw[pair.Key] = pair.Value;
            }

            sSelf = null;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (Raw == null)
            {
                sSelf = new SKeyValuePair<TKey, TValue>[0];
                return;
            }

            sSelf = new SKeyValuePair<TKey, TValue>[Raw.Count];

            int i = 0;
            foreach (KeyValuePair<TKey, TValue> pair in Raw)
            {
                sSelf[i] = new SKeyValuePair<TKey, TValue>(pair);
                i++;
            }
        }

#endregion //Serialization

        public Dictionary<TKey, TValue> Raw { get; private set; }

        public static implicit operator Dictionary<TKey, TValue>(SDictionary<TKey, TValue> sDictionary) => sDictionary?.Raw;
        private SDictionary() {}
    }
}
