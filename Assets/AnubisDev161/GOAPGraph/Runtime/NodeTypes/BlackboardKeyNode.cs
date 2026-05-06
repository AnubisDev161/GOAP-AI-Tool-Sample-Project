using System;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

namespace GOAPGraph
{
    [NodeInfo("Blackbaord Key", "Blackbaord/World Fact", hasFlowInput: false, hasFlowOutput: false, hasInputParams: true, hasOutputParams: true, paramPortsHaveSingleCapacity: false)]
    public class BlackbaordKeyNode : GOAPGraphNode
    {
        [ExposedWorldFactProperty]
        public WorldFact worldFact;

        public WorldFact GetData()
        {
            return worldFact;

        }

        public BlackbaordKeyNode()
        {
            {
                worldFact.InitValue();
            }
        }
    }


    [Serializable]
    public struct WorldFact : ISerializationCallbackReceiver
    {
        [ExposedProperty]
        public string name;

        [SerializeReference]
        public object value;

        [SerializeField]
        private int intValue;

        [SerializeField]
        private float floatValue;

        [SerializeField]
        private string stringValue;

        [SerializeField]
        private bool boolValue;

        [ExposedProperty]
        public ValueType valueType;

        

        public void InitValue()
        {
            if (intValue != 0) value = intValue;
            else if (floatValue != 0) value = floatValue;
            else if (stringValue != "") value = stringValue;
            else value = boolValue;
        }

        public void OnAfterDeserialize()
        {
            Debug.Log(name);
            Debug.Log(value);
            Debug.Log(valueType);
        }

        public void OnBeforeSerialize()
        {
            if (value is int) intValue = (int)value;
            if (value is float) floatValue = (float)value;
            if (value is string) stringValue = (string)value;
            if (value is bool) boolValue = (bool)value;


        }

        public static bool operator ==(WorldFact x, WorldFact y)
        {
            return x.valueType == y.valueType && x.name == y.name && x.value == y.value;
        }

        public static bool operator !=(WorldFact x, WorldFact y)
        {
            return x.valueType == y.valueType || x.name != y.name || x.value != y.value;
        }
    }

    [Serializable]
    public enum ValueType
    {
        Bool,
        Int,
        Float,
        String
    }
}
