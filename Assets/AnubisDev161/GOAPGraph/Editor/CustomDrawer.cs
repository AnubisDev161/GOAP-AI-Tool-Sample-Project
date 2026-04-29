using GOAPGraph;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer (typeof(WorldFact))]
    public class WorldFactDrawer : PropertyDrawer
{
    private SerializedProperty onlyForTesting;
    // Draw the property inside the given rect

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 120;
    }

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        // Create property container element.
        var container = new VisualElement();

        var foldOut = new Foldout();
        foldOut.value = false;

        onlyForTesting = property;
        var valueTypeProperty = property.FindPropertyRelative("valueType");
        var valueProperty = property.FindPropertyRelative("value");
     


        var valueType = (ValueType)valueTypeProperty.boxedValue;

        TextField textField;
        Toggle toggle;

        DefineValueByType(valueProperty, valueType, out textField, out toggle);

        
        // Create property fields.
        var valueField = new PropertyField(valueProperty);
        var valueTypeField = new PropertyField(valueTypeProperty);
        var nameField = new PropertyField(property.FindPropertyRelative("name"));


        foldOut.Add(nameField);
       
        // Add either a textField or a toggle according to the property's value
        if (valueProperty.boxedValue.GetType() == typeof(bool))
        {
            foldOut.Add(toggle);
        }
        else
        {
            foldOut.Add(textField);
        }
      
        foldOut.Add(valueTypeField);
      

        // Add fields to the container.
        container.Add(foldOut);

        valueTypeField.TrackPropertyValue(valueTypeProperty, OnValueChanged);

        return container;
    }

    private void OnValueChanged(SerializedProperty property)
    {
        CreatePropertyGUI(onlyForTesting);
    }

    private void DefineValueByType(SerializedProperty valueProperty, ValueType valueType, out TextField textField, out Toggle toggle)
    {
        textField = new TextField("value");
        toggle = new Toggle("value");

        switch (valueType)
        {
            case ValueType.Bool:
                valueProperty.boxedValue = true;
                textField.value = "true";
                toggle.value = true;
                break;
            case ValueType.Int:
                valueProperty.boxedValue = 0;
                textField.value = "0";
                break;
            case ValueType.Float:
                valueProperty.boxedValue = 0.0f;
                textField.value = "0.0f";
                break;
            case ValueType.String:
                valueProperty.boxedValue = "Default";
                textField.value = "Default";
                break;

            default:
                Debug.LogError("Could not convert valueType to known value");
                textField = null;
                break;
        }
    }
}