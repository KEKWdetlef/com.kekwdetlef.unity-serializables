using System;
using UnityEditor;

namespace Kekwdetlef.Serializables.Editor
{
    [CustomPropertyDrawer(typeof(SUniqueList<>))]
    internal class SUniqueListPropertyDrawer : BaseListPropertyDrawer
    {
        protected override void GetPropertys(SerializedProperty property, out SerializedProperty sSelfProperty, out Type listItemType, out SerializedProperty newValueProperty)
        {
            sSelfProperty = property.FindPropertyRelative(SUniqueList<byte>.SSelfProperty);
            newValueProperty = property.FindPropertyRelative(SUniqueList<byte>.NewValueProperty_Editor);

            listItemType = fieldInfo.FieldType.GetGenericArguments()[0];
        }

        // protected override bool IsAddable(object[] currentObjects, object newObject)
        // {
        //     foreach (object item in currentObjects)
        //     {
        //         if (item == newObject)
        //         {
        //             return false;
        //         }
        //     }

        //     return true;
        // }

        protected override bool IsLengthEditable() => true;
    }
}
