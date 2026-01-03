using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Kekwdetlef.Serializables.Editor
{
    internal class SListView : VisualElement
    {
        public override VisualElement contentContainer => null;

        private readonly SerializedProperty listProperty;
        private readonly VisualElement internalContentContainer;
        private readonly Type listItemType;
        private readonly IntegerField listLengthField;

        public SListView(SerializedProperty listProperty, Type listItemType, IntegerField listLengthField)
        {
            this.listProperty = listProperty;
            this.listItemType = listItemType;
            this.listLengthField = listLengthField;

            internalContentContainer = new VisualElement();
            hierarchy.Add(internalContentContainer);

            Rebuild();
            
            listLengthField.RegisterValueChangedCallback((callbackContext) => SetLength(callbackContext.newValue));
        }

        internal void Rebuild()
        {
            listLengthField.value = listProperty.arraySize;
            internalContentContainer.Clear();

            int listLength = listProperty.arraySize;
            if (listLength == 0)
            {
                Label emptyListLabel = CreateEmptyListLabel("List is empty");
                internalContentContainer.Add(emptyListLabel);
            }

            for (int i = 0; i < listLength; i++)
            {
                VisualElement itemContainer = CreateItemContainer();

                SerializedProperty itemProperty = listProperty.GetArrayElementAtIndex(i);
                PropertyField itemField = CreateItemField(itemProperty);
                itemContainer.Add(itemField);

                Button removeButton = CreateRemoveButton(i);
                itemContainer.Add(removeButton);

                removeButton.RegisterCallback<ClickEvent>((_) =>
                {
                    int owningIndex = (int)removeButton.userData;
                    listProperty.serializedObject.Update();
                    listProperty.DeleteArrayElementAtIndex(owningIndex);
                    
                    ApplyChanges();
                });

                internalContentContainer.Add(itemContainer);
            }
        }

        internal void Add(object objectToAdd)
        {
            listProperty.serializedObject.Update();

            if (TryAddRaw(objectToAdd))
            {
                ApplyChanges();
            }
        }

        internal void AddRange(object[] objectsToAdd)
        {
            if (objectsToAdd == null)
            { return; }

            listProperty.serializedObject.Update();
            bool anyAdded = false;

            foreach (object objectToAdd in objectsToAdd)
            {
                if (TryAddRaw(objectToAdd))
                {
                    anyAdded = true;
                }
            }

            if (anyAdded)
            {
                ApplyChanges();
            }
        }

        internal bool TryAddRaw(object objectToAdd)
        {
            if (objectToAdd == null)
            { return false; }

            if (!listItemType.IsAssignableFrom(objectToAdd.GetType()))
            {
                // TODO: do the thing where the system trys to set the first field of the type if the type doesnt match maybe?
            }

            int newItemIndex = listProperty.arraySize;
            listProperty.InsertArrayElementAtIndex(newItemIndex);
            listProperty.GetArrayElementAtIndex(newItemIndex).boxedValue = objectToAdd;
            return true;
        }

        private void ApplyChanges()
        {
            listProperty.serializedObject.ApplyModifiedProperties();
            Rebuild();

            if (listProperty.serializedObject.UpdateIfRequiredOrScript())
            {
                Rebuild();
            }
        }

        private void SetLength(int newLength)
        {
            int listLength = listProperty.arraySize;
            if (newLength == listLength)
            { return; }

            listProperty.serializedObject.Update();

            if (newLength < listLength)
            {
                for (int i = listLength - 1; i >= newLength; i--)
                {
                    listProperty.DeleteArrayElementAtIndex(i);
                }
            }
            else
            {
                for (int i = listLength; i < newLength; i++)
                {
                    listProperty.InsertArrayElementAtIndex(i);
                }
            }

            listProperty.serializedObject.ApplyModifiedProperties();
            Rebuild();
        }

        private Label CreateEmptyListLabel(string text) => new Label(text)
        {
            style =
            {
                marginTop = 4,
            },
        };

        private VisualElement CreateItemContainer() => new VisualElement()
        {
            style =
            {
                flexDirection = FlexDirection.Row,
            }
        };

        private PropertyField CreateItemField(SerializedProperty itemProperty)
        {
            PropertyField itemField = new PropertyField(itemProperty)
            {
                style =
                {
                    flexGrow = 1,
                },
            };

            itemField.BindProperty(itemProperty);
            return itemField;
        }

        private Button CreateRemoveButton(int owningIndex) => new Button()
        {
            iconImage = new Background(),
            style =
            {
                flexGrow = 0,
                maxHeight = 18,
                minHeight = 18,

                minWidth = 18,
                maxWidth = 18,
            },
            userData = owningIndex,
        };
    }
}
