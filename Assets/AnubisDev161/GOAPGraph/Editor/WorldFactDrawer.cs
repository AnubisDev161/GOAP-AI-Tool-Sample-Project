using GOAPGraph;
using GOAPGraph.Editor;
using System;
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
        // Draw the property inside the given rect
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 120;
            
        }

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create property container element.
            var container = new VisualWorldFactElement(property);

            return container;
        }
    }
}