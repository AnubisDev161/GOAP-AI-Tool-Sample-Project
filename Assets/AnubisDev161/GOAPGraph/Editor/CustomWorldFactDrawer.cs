using GOAPGraph;
using GOAPGraph.Editor;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GOAPGraph.Editor
{
    [CustomPropertyDrawer(typeof(WorldFact))]
    public class WorldFactDrawer : PropertyDrawer
    {
        private SerializedProperty test;
        // Draw the property inside the given rect

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 120;
            
        }
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create property container element.
            var container = new CustomVisaulElement();
            
            var foldOut = new Foldout();
            foldOut.value = false;

            var valueTypeProperty = property.FindPropertyRelative("valueType");
           
            var valueProperty = property.FindPropertyRelative("value");


            test = valueProperty;

            var valueType = (ValueType)valueTypeProperty.boxedValue;
           

            TextField textField;

            Toggle toggle;
            bool requiresTextField;

            
            DefineValueByType(valueProperty, valueType, out textField, out toggle, out requiresTextField);

            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();

            // Create property fields.
            var valueField = new PropertyField(valueProperty);
            var valueTypeField = new PropertyField(valueTypeProperty);
            var nameField = new PropertyField(property.FindPropertyRelative("name"));

            foldOut.Add(nameField);

            valueTypeField.RegisterValueChangeCallback(Test);

            if (toggle.value == true)
            {
                foldOut.Add(toggle);
            }
            else if (textField.value == "Default")
            {
                foldOut.Add(textField);
            }



            // Add either a textField or a toggle according to the property's value
     

            foldOut.Add(valueTypeField);

            // Add fields to the container.
            container.Add(foldOut);

           // toggle.TrackPropertyValue

            return container;
        }

        private void Test(SerializedPropertyChangeEvent evt)
        {
            // throw new System.NotImplementedException();

           var valueType =  (ValueType)evt.changedProperty.boxedValue;

            //Debug.Log(evt.changedProperty.boxedValue);

            switch (valueType)
            {
                case ValueType.Bool:
                    test.boxedValue = true;
                    test.serializedObject.ApplyModifiedProperties();
                    test.serializedObject.Update();
                    break;
                case ValueType.Int:
                    test.boxedValue = (int)0;
                    test.serializedObject.ApplyModifiedProperties();
                    test.serializedObject.Update();
                    break;
                case ValueType.Float:
                    test.boxedValue = 0.0f;
                    test.serializedObject.ApplyModifiedProperties();
                    test.serializedObject.Update();
                    break;
                case ValueType.String:
                    test.boxedValue = "Default";
                    test.serializedObject.ApplyModifiedProperties();
                    test.serializedObject.Update();
                    break;

                default:
                    Debug.LogError("Could not convert valueType to known value");

                    break;
            }
        }

        private void OnValueTypeChanged(SerializedProperty property)
        {
            test.serializedObject.ApplyModifiedProperties();
            test.serializedObject.Update();
        }

        private void OnValueChanged(SerializedPropertyChangeEvent evt)
        {
            // CreatePropertyGUI(onlyForTesting);
            // Value changing seems to wrok, the node's data is updated and the create propertyGUI method in the correct moment,
            // you only need to call the method from were it is usually being called
            //onlyForTestingParent.f
            //DefineValueByType()

            
        }

        public static void DefineValueByType(SerializedProperty valueProperty, ValueType valueType, out TextField textField, out Toggle toggle, out bool requiresTextField)
        {
            textField = new TextField("value");
            toggle = new Toggle("value");

            requiresTextField = true;

            switch (valueType)
            {
                case ValueType.Bool:
                    valueProperty.boxedValue = true;
                    valueProperty.serializedObject.ApplyModifiedProperties();
                    valueProperty.serializedObject.Update();
                    requiresTextField = false;
                    toggle.value = true;
                    break;
                case ValueType.Int:
                    valueProperty.boxedValue = (int)0 ;
                    textField.value = "0";
                    valueProperty.serializedObject.ApplyModifiedProperties();
                    valueProperty.serializedObject.Update();
                    break;
                case ValueType.Float:
                    valueProperty.boxedValue = 0.0f;
                    textField.value = "0.0f";
                    valueProperty.serializedObject.ApplyModifiedProperties();
                    valueProperty.serializedObject.Update();
                    break;
                case ValueType.String:
                    valueProperty.boxedValue = "Default";
                    valueProperty.serializedObject.ApplyModifiedProperties();
                    valueProperty.serializedObject.Update();
                    textField.value = "Default";
                    break;

                default:
                    Debug.LogError("Could not convert valueType to known value");
                    textField = null;
                    toggle = null;
                    break;
            }
        }
    }
}