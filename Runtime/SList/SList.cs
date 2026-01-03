using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kekwdetlef.Serializables
{
    [Serializable]
    public sealed class SList<T>
    {

#if UNITY_EDITOR

        public const string NewValueProperty_Editor = nameof(newValue_editor);
        [SerializeField] private T newValue_editor;

#endif // UNITY_EDITOR

#region Serialization

        public const string SSelfProperty = nameof(sSelf);
        [SerializeField] private List<T> sSelf;

#endregion // Serialization

        public List<T> Raw => sSelf;

        public static implicit operator List<T>(SList<T> sList) => sList?.sSelf; 
        private SList() {}
    }
}
