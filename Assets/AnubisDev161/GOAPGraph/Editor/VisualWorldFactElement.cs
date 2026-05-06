using Codice.Client.Common.GameUI;
using System;
using System.ComponentModel;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


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

            VisualElement valueField =  CreateFieldByType(valueProperty, valueType);

            serializedWorldFact.serializedObject.ApplyModifiedProperties();
            serializedWorldFact.serializedObject.Update();

            // Create property fields.
            var valueFieldName = new PropertyField(valueProperty);
            var valueTypeField = new PropertyField(valueTypeProperty);
            var nameField = new PropertyField(serializedWorldFact.FindPropertyRelative("name"));

            foldOut.Add(nameField);

            // Add either a textField or a toggle according to the property's value
            foldOut.Add(valueField);
            //foldOut.Add(valueFieldName);
            

            
            valueTypeField.RegisterValueChangeCallback(ValueTypeChangedCallback);
            

            foldOut.Add(valueTypeField);
            contentContainer.Add(foldOut);
        }

        private void ValueTypeChangedCallback(SerializedPropertyChangeEvent evt)
        {
            var valueTypeProperty = evt.changedProperty;
            var valueProperty = serializedWorldFact.FindPropertyRelative("value");
            


            if (WorldFact.IsRequiredValueType((ValueType)valueTypeProperty.boxedValue, valueProperty.stringValue)) return;

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

            valueProperty.stringValue = newValue.ToString();
            serializedWorldFact.serializedObject.ApplyModifiedProperties();
            serializedWorldFact.serializedObject.Update();
            var window = EditorWindow.GetWindow(typeof(GOAPGraphEditorWindow)) as GOAPGraphEditorWindow;
            window.SaveAndRedrawGraph();
        }

        private void OnValueFieldChanged(ChangeEvent<bool> evt)
        {
            var serializedValueTypeProperty = serializedWorldFact.FindPropertyRelative("valueType");
            if (!((ValueType)serializedValueTypeProperty.boxedValue is ValueType.Bool)) return;
            SetWorldFactValue(evt.newValue.ToString());
        }

        private void OnValueFieldChanged(ChangeEvent<string> evt)
        {
            var serializedValueTypeProperty = serializedWorldFact.FindPropertyRelative("valueType");
            if (!((ValueType)serializedValueTypeProperty.boxedValue is ValueType.String)) return;
            SetWorldFactValue(evt.newValue.ToString());
        }

        private void OnValueFieldChanged(ChangeEvent<int> evt)
        {
            var serializedValueTypeProperty = serializedWorldFact.FindPropertyRelative("valueType");
            if (!((ValueType)serializedValueTypeProperty.boxedValue is ValueType.Int)) return;
            SetWorldFactValue(evt.newValue.ToString());
        }

        private void OnValueFieldChanged(ChangeEvent<float> evt)
        {
            var serializedValueTypeProperty = serializedWorldFact.FindPropertyRelative("valueType");
            if (!((ValueType)serializedValueTypeProperty.boxedValue is ValueType.Float)) return;
            SetWorldFactValue(evt.newValue.ToString());
        }

        private void SetWorldFactValue(string newValue)
        {
            var serializedValueProperty = serializedWorldFact.FindPropertyRelative("value");
            serializedValueProperty.stringValue = newValue;
            serializedWorldFact.serializedObject.ApplyModifiedProperties();
            serializedWorldFact.serializedObject.Update();
        }

      

        private object EvaluateInputDataType(string newValue, object boxedValue)
        {
            object result = null;

            if (boxedValue is bool) return result;

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

      

        public VisualElement CreateFieldByType(SerializedProperty valueProperty, ValueType valueType)
        {
            VisualElement field = null;
            

            switch (valueType)
            {
                case ValueType.Bool:
                    field = new Toggle("value");
                    var toggle = (field as Toggle);
                    toggle.RegisterValueChangedCallback(OnValueFieldChanged);
                    if (valueProperty.stringValue == "") return field;
                    toggle.value = Convert.ToBoolean(valueProperty.stringValue);
                    break;
                case ValueType.Int:
                    field = new IntegerField("value");
                    var intField = (field as IntegerField);
                    intField.RegisterValueChangedCallback(OnValueFieldChanged);
                    if (valueProperty.stringValue == "") return field;
                    intField.value = Convert.ToInt32(valueProperty.stringValue);
                    break;
                case ValueType.Float:
                    field = new FloatField("value");
                    var floatField = (field as FloatField);
                    floatField.RegisterValueChangedCallback(OnValueFieldChanged);
                    if (valueProperty.stringValue == "") return field;


                    var value = valueProperty.stringValue.Remove(valueProperty.stringValue.Length - 1);

                    floatField.value = float.Parse(value);
                    break;
                case ValueType.String:
                    field = new TextField("value");
                    var textField = (field as TextField);
                    textField.RegisterValueChangedCallback(OnValueFieldChanged);
                    textField.value = valueProperty.stringValue;
                    break;
                default:
                    Debug.LogError("Could not convert valueType to known value");
                    break;
            }

            return field;
        }
    }
}

