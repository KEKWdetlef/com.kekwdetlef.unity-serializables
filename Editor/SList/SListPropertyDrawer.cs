using System;
using System.Reflection;
using UnityEditor;

namespace Kekwdetlef.Serializables.Editor
{
    [CustomPropertyDrawer(typeof(SList<>))]
    internal class SListPropertyDrawer : BaseListPropertyDrawer
    {
        protected override void GetPropertys(SerializedProperty property, out SerializedProperty sSelfProperty, out Type listItemType, out SerializedProperty newValueProperty)
        {
            sSelfProperty = property.FindPropertyRelative(SList<byte>.SSelfProperty);
            newValueProperty = property.FindPropertyRelative(SList<byte>.NewValueProperty_Editor);

            listItemType = fieldInfo.FieldType.GetGenericArguments()[0];
        }

        protected override bool IsLengthEditable() => true;
        // protected override bool IsAddable(object[] currentObjects, object newObject) => true;
    }
}
