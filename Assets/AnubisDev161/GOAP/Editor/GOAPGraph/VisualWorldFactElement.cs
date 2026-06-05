using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using GOAP.Core;
using UnityEditor.UIElements;
using System;

namespace GOAP.GOAPGraph.Editor
{
    public class VisualWorldFactElement : VisualElement
    {
        private SerializedProperty serializedWorldFact;

        private Foldout foldOut;

        public VisualWorldFactElement(SerializedProperty worldFactProperty)
        {
            this.serializedWorldFact = worldFactProperty;
            foldOut = new Foldout();
            foldOut.value = true;
            Init();
        }

        private void Init()
        {
            var valueProperty = serializedWorldFact.FindPropertyRelative("value");
            var valueTypeProperty = serializedWorldFact.FindPropertyRelative("valueType");
            var valueType = (WorldFactType)valueTypeProperty.boxedValue;

            // Create a suitable field according to the property's value
            VisualElement valueField = CreateFieldByType(valueProperty, valueType);

            serializedWorldFact.serializedObject.ApplyModifiedProperties();
            serializedWorldFact.serializedObject.Update();

            // Create property fields.
            var valueTypeField = new EnumField("ValueType", WorldFactType.Bool); /* new PropertyField(valueTypeProperty);*/

            valueTypeField.BindProperty(serializedWorldFact.FindPropertyRelative("valueType"));
            valueTypeField.RegisterValueChangedCallback(ValueTypeChangedCallback);

            var nameField = new TextField("Name");
            nameField.BindProperty(serializedWorldFact.FindPropertyRelative("name"));

            foldOut.Add(nameField);
            foldOut.Add(valueField);
            foldOut.Add(valueTypeField);
            contentContainer.Add(foldOut);
        }

        private void ValueTypeChangedCallback(ChangeEvent<Enum> evt)
        {
            var valueType =(WorldFactType)evt.newValue;
            var valueProperty = serializedWorldFact.FindPropertyRelative("value");

            if (WorldFact.IsRequiredValueType(valueType, valueProperty.stringValue)) return;

            object newValue = null;

            switch (valueType)
            {
                case WorldFactType.Bool:
                    newValue = false;
                    break;
                case WorldFactType.Int:
                    newValue = 0;
                    break;
                case WorldFactType.Float:
                    newValue = 0.0f;
                    break;
                default:
                    UnityEngine.Debug.LogError("Could not convert valueType to known value");
                    break;
            }

            valueProperty.stringValue = newValue.ToString();
            serializedWorldFact.serializedObject.ApplyModifiedProperties();
            serializedWorldFact.serializedObject.Update();

            Redraw();
        }

        private void Redraw()
        {
            foldOut.Clear();
            contentContainer.Clear();
            foldOut = new Foldout();
            Init();
        }

        private void OnValueFieldChanged(ChangeEvent<bool> evt)
        {
            var serializedValueTypeProperty = serializedWorldFact.FindPropertyRelative("valueType");
            if (!((WorldFactType)serializedValueTypeProperty.boxedValue is WorldFactType.Bool)) return;
            SetWorldFactValue(evt.newValue.ToString());
        }

        private void OnValueFieldChanged(ChangeEvent<int> evt)
        {
            var serializedValueTypeProperty = serializedWorldFact.FindPropertyRelative("valueType");
            if (!((WorldFactType)serializedValueTypeProperty.boxedValue is WorldFactType.Int)) return;
            SetWorldFactValue(evt.newValue.ToString());
        }

        private void OnValueFieldChanged(ChangeEvent<float> evt)
        {
            var serializedValueTypeProperty = serializedWorldFact.FindPropertyRelative("valueType");
            if (!((WorldFactType)serializedValueTypeProperty.boxedValue is WorldFactType.Float)) return;
            SetWorldFactValue(evt.newValue.ToString());
        }

        private void SetWorldFactValue(string newValue)
        {
            var serializedValueProperty = serializedWorldFact.FindPropertyRelative("value");
            serializedValueProperty.stringValue = newValue;
            serializedWorldFact.serializedObject.ApplyModifiedProperties();
            serializedWorldFact.serializedObject.Update();
        }

        public VisualElement CreateFieldByType(SerializedProperty valueProperty, WorldFactType valueType)
        {
            VisualElement field = null;
            const string VALUE_TITLE = "Value";

            switch (valueType)
            {
                case WorldFactType.Bool:
                    field = new Toggle(VALUE_TITLE);
                    var toggle = (field as Toggle);
                    toggle.RegisterValueChangedCallback(OnValueFieldChanged);
                    if (valueProperty.stringValue == "") return field;
                    toggle.value = System.Convert.ToBoolean(valueProperty.stringValue);
                    break;
                case WorldFactType.Int:
                    field = new IntegerField(VALUE_TITLE);
                    var intField = (field as IntegerField);
                    intField.RegisterValueChangedCallback(OnValueFieldChanged);
                    if (valueProperty.stringValue == "") return field;
                    intField.value = System.Convert.ToInt32(valueProperty.stringValue);
                    break;
                case WorldFactType.Float:
                    field = new FloatField(VALUE_TITLE);
                    var floatField = (field as FloatField);
                    floatField.RegisterValueChangedCallback(OnValueFieldChanged);
                    if (valueProperty.stringValue == "") return field;

                    var value = valueProperty.stringValue.Remove(valueProperty.stringValue.Length - 1);

                    floatField.value = float.Parse(value);
                    break;
                default:
                    UnityEngine.Debug.LogError("Could not convert valueType to known value");
                    break;
            }

            return field;
        }
    }
}

