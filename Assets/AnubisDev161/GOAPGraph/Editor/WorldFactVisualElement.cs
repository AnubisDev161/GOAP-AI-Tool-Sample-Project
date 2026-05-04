using System;
using System.ComponentModel;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Search.SearchValue;

namespace GOAPGraph.Editor
{
    public class WorldFactVisualElement : VisualElement
    {
        public void AddNestedPropertyAndSubscribeToCallback(PropertyField propertyField)
        {
            propertyField.RegisterValueChangeCallback(OnNestedPropertyChanged);
            Add(propertyField);
            
        }

        public void AddPropertyAndSubscribeToCallback(PropertyField propertyField)
        {
            propertyField.RegisterValueChangeCallback(OnPropertyChanged);
            Add(propertyField);
        }

        private void OnNestedPropertyChanged(SerializedPropertyChangeEvent evt)
        {
            
        }

        private void OnPropertyChanged(SerializedPropertyChangeEvent evt)
        {

            var worldFact = (WorldFact)evt.target;
            foreach (var elmenent in contentContainer.Children())
            {
                if (elmenent.name == worldFact.GetType().Name)
                {

                }
            }
        }

    }
}
