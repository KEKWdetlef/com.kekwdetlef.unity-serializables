using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kekwdetlef.Serializables.Editor
{
    internal abstract class BaseListPropertyDrawer : PropertyDrawer
    {
        protected abstract void GetPropertys(SerializedProperty property, out SerializedProperty sSelfProperty, out Type listItemType, out SerializedProperty newValueProperty);
        protected abstract bool IsLengthEditable();

        [Obsolete("Dont implement this, it isnt used anymore", false)]
        protected virtual bool IsAddable(object[] currentObjects, object newObject) => true;

        public sealed override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            GetPropertys(property, out SerializedProperty sSelfProperty, out Type listItemType, out SerializedProperty newValueProperty);

            VisualElement root = CreateRoot();

            IntegerField listLengthField = CreateListLengthField(IsLengthEditable());
            Action<object[]> onItemsDropped = null;

            Foldout foldout = CreateFoldoutContainer(property.displayName, sSelfProperty, listItemType, listLengthField, (objects) => onItemsDropped?.Invoke(objects));
            root.Add(foldout);

            VisualElement contentContainer = CreateContentContainer();
            foldout.Add(contentContainer);

            SListView itemsContainer = CreateListView(sSelfProperty, listItemType, listLengthField);
            onItemsDropped = itemsContainer.AddRange;
            contentContainer.Add(itemsContainer);

            VisualElement seperator = CreateSeperator();
            contentContainer.Add(seperator);

            Button addButton = CreateAddButton();
            addButton.RegisterCallback<ClickEvent>((_) => itemsContainer.Add(newValueProperty.boxedValue));
            contentContainer.Add(addButton);

            PropertyField newValueField = CreateNewValueField(newValueProperty);
            contentContainer.Add(newValueField);

            return root;
        }

        private VisualElement CreateRoot() => new VisualElement()
        {
            style =
            {
                marginTop = 3,
            }
        };

        private IntegerField CreateListLengthField(bool isEditable) => new IntegerField()
        {
            enabledSelf = isEditable,
            isDelayed = true,

            style =
            {
                marginRight = 2,
            },
        };

        private Foldout CreateFoldoutContainer(string displayName, SerializedProperty listProperty, Type listItemType, IntegerField listSizeField, Action<object[]> onItemsDropped)
        {
            Foldout foldout = new Foldout()
            {
                text = displayName,
                value = listProperty.isExpanded,
            };

            foldout.RegisterValueChangedCallback((callbackContext) => listProperty.isExpanded = callbackContext.newValue);

            VisualElement header = CreateFoldoutHeader();
            foldout.hierarchy.Insert(0, header);

            Toggle toggle = foldout.Q<Toggle>();
            toggle.style.flexGrow = 1;
            toggle.style.borderTopLeftRadius = 3;
            toggle.style.borderTopRightRadius = 3;
            toggle.style.borderBottomRightRadius = 3;
            toggle.style.borderBottomLeftRadius = 3;

            toggle.RegisterCallback<DragEnterEvent>((_) => toggle.style.backgroundColor = Color.gray3);
            toggle.RegisterCallback<DragLeaveEvent>((_) => toggle.style.backgroundColor = StyleKeyword.Null);
            toggle.RegisterCallback<DragExitedEvent>((_) => toggle.style.backgroundColor = StyleKeyword.Null);
            
            toggle.RegisterCallback<DragUpdatedEvent>((_) =>
            {
                object[] draggedObjects = DragAndDrop.objectReferences;
                if(ContainsUsableItems(draggedObjects, listItemType))
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                }
                else
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                }
            });

            toggle.RegisterCallback<DragPerformEvent>((_) =>
            {
                object[] draggedObjects = DragAndDrop.objectReferences;
                if(!ContainsUsableItems(draggedObjects, listItemType)) 
                { return; }

                onItemsDropped?.Invoke(draggedObjects);
                DragAndDrop.AcceptDrag();
            });

            foldout.hierarchy.Remove(toggle);
            header.Add(toggle);

            header.Add(listSizeField);

            return foldout;

            static bool ContainsUsableItems(object[] objects, Type listItemType)
            {
                foreach (object draggedObject in objects)
                {
                    if (listItemType.IsAssignableFrom(draggedObject.GetType()))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private VisualElement CreateFoldoutHeader() => new VisualElement()
        {
            style =
            {
                flexDirection = FlexDirection.Row,
            }
        };

        private VisualElement CreateContentContainer() => new VisualElement()
        {
            style =
            {
                borderTopColor = Color.black,
                borderTopWidth = 1,

                borderRightColor = Color.black,
                borderRightWidth = 1,
                
                borderBottomColor = Color.black,
                borderBottomWidth = 1,
                
                borderLeftColor = Color.black,
                borderLeftWidth = 1,

                borderTopLeftRadius = 3,
                borderTopRightRadius = 3,
                borderBottomRightRadius = 3,
                borderBottomLeftRadius = 3,
                

                marginTop = 2,
                marginLeft = -24,
                marginRight = 2,
                backgroundColor = Color.gray2,

                minWidth = 300,
            }
        };

        private SListView CreateListView(SerializedProperty listProperty, Type listItemType, IntegerField listLengthField) => new SListView(listProperty, listItemType, listLengthField)
        {
            style =
            {
                marginLeft = 14,
            }
        };

        private VisualElement CreateSeperator() => new VisualElement()
        {
            style =
            {
                height = 2,
                marginTop = 4,
                marginBottom = 10,
                flexGrow = 1,
                backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f)
            }
        };

        private Button CreateAddButton() => new Button()
        {
            text = "Add",
            style =
            {
                maxWidth = 50,
                marginLeft = 14,
            },
        };

        private PropertyField CreateNewValueField(SerializedProperty property)
        {
            PropertyField field = new PropertyField(property, "New Value")
            {
                style =
                {
                    marginBottom = 6,
                    marginRight = 6,
                    marginLeft = 14,
                },
            };
            field.BindProperty(property);
            return field;
        }
    }
}