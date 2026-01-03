using System;
using System.Reflection;
using UnityEditor;

namespace Kekwdetlef.Serializables.Editor
{
    [CustomPropertyDrawer(typeof(SDictionary<,>))]
    internal class SDictionaryPropertyDrawer : BaseListPropertyDrawer
    {
        protected override void GetPropertys(SerializedProperty property, out SerializedProperty sSelfProperty, out Type listItemType, out SerializedProperty newValueProperty)
        {
            sSelfProperty = property.FindPropertyRelative(SDictionary<byte, byte>.SSelfProperty);
            newValueProperty = property.FindPropertyRelative(SDictionary<byte, byte>.NewValueProperty_Editor);

            

            listItemType = fieldInfo.FieldType.GetField(newValueProperty.name, BindingFlags.Instance | BindingFlags.NonPublic).FieldType;
        }

        // protected override bool IsAddable(object[] currentObjects, object newObject)
        // {
        //     throw new NotImplementedException();
        // }

        protected override bool IsLengthEditable() => false;
    }
}
