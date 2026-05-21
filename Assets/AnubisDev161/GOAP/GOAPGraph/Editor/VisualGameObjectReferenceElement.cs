using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GOAP.GOAPGraph.Editor
{
    // This is only a work around!
    public class VisualGameObjectReferenceElement : VisualElement
    {
        private SerializedProperty serializedProperty;

        public VisualGameObjectReferenceElement(SerializedProperty serializedProperty)
        {
            this.serializedProperty = serializedProperty;

            var sceneID = serializedProperty.FindPropertyRelative("sceneID");

            var destinationField = new ObjectField("Target");
            destinationField.RegisterValueChangedCallback(OnFieldValueChanged);

            if (sceneID.intValue != 0)
            {
                destinationField.value = EditorUtility.InstanceIDToObject(sceneID.intValue);
            }

            contentContainer.Add(destinationField);
        }

        // TODO Rplace vector3Value with actual serialized gameObject 
        private void OnFieldValueChanged(ChangeEvent<UnityEngine.Object> evt)
        {
            var gameObject = serializedProperty.FindPropertyRelative("position");
            var sceneID = serializedProperty.FindPropertyRelative("sceneID");

            if (evt.newValue is not GameObject)
            {
                gameObject.vector3Value = Vector3.zero;
                sceneID.intValue = 0;
            }
            else
            {
                var pos = (evt.newValue as GameObject).transform.position;
                gameObject.vector3Value = pos;
                sceneID.intValue = (evt.newValue as GameObject).GetInstanceID();
            }


            serializedProperty.serializedObject.ApplyModifiedProperties();
            serializedProperty.serializedObject.Update();
        }
    }
}