using System;
using UnityEditor.UIElements;
using UnityEngine;

namespace GOAPGraph
{
    [NodeInfo("Blackbaord Key", "Blackbaord/World Fact", hasFlowInput: false, hasFlowOutput: false, hasInputParams: true, hasOutputParams: true, paramPortsHaveSingleCapacity: false)]
    public class BlackbaordKeyNode : GOAPGraphNode
    {
        [ExposedWorldFactProperty]
        public WorldFact worldFact = new WorldFact("DialogueText", false, ValueType.Bool);

        public WorldFact GetData()
        {
            return worldFact;

        }
    }


    [Serializable]
    public struct WorldFact : ISerializationCallbackReceiver
    {
        [ExposedProperty]
        public string name;

        [SerializeReference]
        public object value;

        [ExposedProperty]
        public ValueType valueType;

        public WorldFact(string name, object value, ValueType valueType)
        {
            this.name = name;

            this.value = value;

            this.valueType = valueType;
        }

        public void OnAfterDeserialize()
        {
            Debug.Log(name);
            Debug.Log(value);
            Debug.Log(valueType);

        }

        public void OnBeforeSerialize()
        {

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
