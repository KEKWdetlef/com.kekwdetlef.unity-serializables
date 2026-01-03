using System;
using UnityEditor;
using UnityEngine;

namespace Kekwdetlef.Serializables.Editor
{
    [CustomPropertyDrawer(typeof(SHashSet<>))]
    internal class SHashSetPropertyDrawer : BaseListPropertyDrawer
    {
        protected override void GetPropertys(SerializedProperty property, out SerializedProperty sSelfProperty, out Type listItemType, out SerializedProperty newValueProperty)
        {
            sSelfProperty = property.FindPropertyRelative(SHashSet<byte>.SSelfProperty);
            newValueProperty = property.FindPropertyRelative(SHashSet<byte>.NewValueProperty_Editor);

            listItemType = fieldInfo.FieldType.GetGenericArguments()[0];
        }

        // protected override bool IsAddable(object[] currentObjects, object newObject)
        // {
        //     foreach (object item in currentObjects)
        //     {
        //         Debug.Log($"{item.GetHashCode()} : {newObject.GetHashCode()}");
        //         if (Equals(item, newObject))
        //         { return false; }

        //         if (item == newObject)
        //         { return false; }

        //         if(ReferenceEquals(item, newObject))
        //         { return false;}
        //     }

        //     return true;
        // }

        protected override bool IsLengthEditable() => false;
    }
}
