using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GOAP.GOAPGraph.Editor
{
    // Only work around
    [CustomPropertyDrawer(typeof(Destination))]
    public class CustomMoveToDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create property container element.
            var container = new VisualGameObjectReferenceElement(property);
          
            return container;
        }
    }
}
