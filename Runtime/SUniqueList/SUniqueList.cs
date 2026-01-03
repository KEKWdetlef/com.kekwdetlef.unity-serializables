using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kekwdetlef.Serializables
{
    [Serializable]
    public sealed class SUniqueList<T> : ISerializationCallbackReceiver
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
            Raw = new List<T>();

            if (sSelf == null)
            { return; }

            foreach (T item in sSelf)
            {
                if (!Raw.Contains(item))
                {
                    Raw.Add(item);
                }
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
            foreach (T item in Raw)
            {
                sSelf[i] = item;
                i++;
            }
        }

#endregion // Serialization

        public List<T> Raw { get; private set; }

        public static implicit operator List<T>(SUniqueList<T> sUniqueList) => sUniqueList?.Raw; 
        private SUniqueList() {}
    }
}
