using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using GOAP.Core;

namespace GOAP.GOAPGraph.Editor
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