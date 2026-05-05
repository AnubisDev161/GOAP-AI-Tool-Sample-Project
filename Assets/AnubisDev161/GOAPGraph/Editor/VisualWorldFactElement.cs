using System;
using System.ComponentModel;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Search.SearchValue;

namespace GOAPGraph.Editor
{
    public class VisualWorldFactElement : VisualElement
    {
        private SerializedProperty serializedWorldFact;

        private Foldout foldOut;
        public VisualWorldFactElement(SerializedProperty worldFactProperty)
        {
            this.serializedWorldFact = worldFactProperty;
            foldOut = new Foldout();
            foldOut.value = false;

            Init();
        }

        private void Init()
        {
            var valueTypeProperty = serializedWorldFact.FindPropertyRelative("valueType");
            var valueProperty = serializedWorldFact.FindPropertyRelative("value");
            var valueType = (ValueType)valueTypeProperty.boxedValue;


            TextField textField;
            Toggle toggle;
            bool requiresTextField;

            DefineFieldByType(valueProperty, valueType, out textField, out toggle, out requiresTextField);

            serializedWorldFact.serializedObject.ApplyModifiedProperties();
            serializedWorldFact.serializedObject.Update();

            // Create property fields.
            var valueField = new PropertyField(valueProperty);
            var valueTypeField = new PropertyField(valueTypeProperty);
            var nameField = new PropertyField(serializedWorldFact.FindPropertyRelative("name"));

            foldOut.Add(nameField);

         

            // Add either a textField or a toggle according to the property's value
            if (requiresTextField)
            {
                foldOut.Add(textField);
            }
            else if (!requiresTextField)
            {
                foldOut.Add(toggle);
            }


            valueTypeField.RegisterValueChangeCallback(ValueTypeChangedCallback);
            textField.RegisterValueChangedCallback(OnValueFieldChanged);
            toggle.RegisterValueChangedCallback(OnValueFieldChanged);

            foldOut.Add(valueTypeField);


            contentContainer.Add(foldOut);
        }

        private void ValueTypeChangedCallback(SerializedPropertyChangeEvent evt)
        {
            var valueTypeProperty = evt.changedProperty;
            var valueProperty = serializedWorldFact.FindPropertyRelative("value");

            if (IsRequiredValueType((ValueType)valueTypeProperty.boxedValue, valueProperty)) return;

            object newValue = null;

            switch ((ValueType)valueTypeProperty.boxedValue)
            {
                case ValueType.Bool:
                    newValue = false;
                    break;
                case ValueType.Int:
                    newValue = 0;
                    break;
                case ValueType.Float:
                    newValue = 0.0f;
                    break;
                case ValueType.String:
                    newValue = "Default";
                    break;
            }

            valueProperty.boxedValue = newValue;
            valueProperty.serializedObject.ApplyModifiedProperties();
            valueProperty.serializedObject.Update();
        }

        private bool IsRequiredValueType(ValueType requiredValueType, SerializedProperty serializedProperty)
        {
            var type = serializedProperty.boxedValue as Type;

            if (serializedProperty.boxedValue is bool && requiredValueType == ValueType.Bool) return true;
            if (serializedProperty.boxedValue is int && requiredValueType == ValueType.Int) return true;
            if (serializedProperty.boxedValue is float && requiredValueType == ValueType.Float) return true;
            if (serializedProperty.boxedValue is string && requiredValueType == ValueType.String) return true;

            return false;
        }

        private void OnValueFieldChanged(ChangeEvent<string> evt)
        {
            var serializedValueProperty = serializedWorldFact.FindPropertyRelative("value");

            object result = EvaluateInputDataType(evt.newValue, serializedValueProperty.boxedValue);


            if (result == null) return;

            serializedValueProperty.boxedValue = result;



            serializedValueProperty.serializedObject.ApplyModifiedProperties();
            serializedValueProperty.serializedObject.Update();
        }

        private object EvaluateInputDataType(string newValue, object boxedValue)
        {
            object result = null;

            if (boxedValue is bool) return result;

            // if (newValue as int?) return;

            float floatValue;
            float.TryParse(newValue, out floatValue);

            int intValue;
            int.TryParse(newValue, out intValue);


            if (floatValue != 0 && boxedValue is float)
            {
                result = floatValue;
            }
            else if (intValue != 0 && boxedValue is int)
            {
                result = intValue;
            }
            else if (boxedValue is string)
            {
                result = newValue.ToString();
            }

            return result;
        }

        private void OnValueFieldChanged(ChangeEvent<bool> evt)
        {
            var serializedValueProperty = serializedWorldFact.FindPropertyRelative("value");

            if (!(serializedValueProperty.boxedValue is bool)) return;

            serializedValueProperty.boxedValue = evt.newValue;

            serializedValueProperty.serializedObject.ApplyModifiedProperties();
            serializedValueProperty.serializedObject.Update();
        }

        public void DefineFieldByType(SerializedProperty valueProperty, ValueType valueType, out TextField textField, out Toggle toggle, out bool requiresTextField)
        {
            textField = new TextField("value");
            toggle = new Toggle("value");

            requiresTextField = true;

            switch (valueType)
            {
                case ValueType.Bool:
                    //valueProperty.serializedObject.ApplyModifiedProperties();
                    //valueProperty.serializedObject.Update();
                    requiresTextField = false;
                    toggle.value = ((bool)valueProperty.boxedValue);
                    break;
                case ValueType.Int:
                    textField.value = (valueProperty.boxedValue).ToString();
                    //valueProperty.serializedObject.ApplyModifiedProperties();
                    //valueProperty.serializedObject.Update();
                    break;
                case ValueType.Float:

                    textField.value = (valueProperty.boxedValue).ToString();
                    //valueProperty.serializedObject.ApplyModifiedProperties();
                    //valueProperty.serializedObject.Update();
                    break;
                case ValueType.String:
                    textField.value = (valueProperty.boxedValue).ToString();
                    //valueProperty.serializedObject.ApplyModifiedProperties();
                    //valueProperty.serializedObject.Update();
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

