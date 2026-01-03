using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kekwdetlef.Serializables
{
    [Serializable]
    public sealed class SHashSet<T> : ISerializationCallbackReceiver
    {

#if UNITY_EDITOR

        public const string NewValueProperty_Editor = nameof(newValue_editor);
        [SerializeField] private T newValue_editor;

#endif // UNITY_EDITOR

#region Serialization

        public const string SSelfProperty = nameof(sSelf);
        [SerializeField] private T[] sSelf;

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Raw = new HashSet<T>();

            if (sSelf == null)
            { return ;}

            foreach (T value in sSelf)
            {
                Raw.Add(value);
            }

            sSelf = null;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (Raw == null)
            {
                sSelf = new T[0];
                return;
            }

            sSelf = new T[Raw.Count];

            int i = 0;
            foreach (T value in Raw)
            {
                sSelf[i] = value;
                i++;
            }
        }

#endregion // Serialization

        public HashSet<T> Raw { get; private set; }

        public static implicit operator HashSet<T>(SHashSet<T> sHashSet) => sHashSet?.Raw;
        private SHashSet() {}
    }
}
