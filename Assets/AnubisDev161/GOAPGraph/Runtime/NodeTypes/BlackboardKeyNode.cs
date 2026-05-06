using Mono.Cecil;
using System;
using System.Linq;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

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
    }


    [Serializable]
    public struct WorldFact : ISerializationCallbackReceiver
    {
        [ExposedProperty]
        public string name;

        [SerializeField]
        public string value;

        [ExposedProperty]
        public ValueType valueType;


        public void OnAfterDeserialize()
        {
            if (valueType == ValueType.Float && !value.Contains("f"))
            {
                value += "f";
            }

            Debug.Log(name);
            Debug.Log(value);
            Debug.Log(valueType);
        }

        public void OnBeforeSerialize()
        {

            if (valueType == ValueType.Float && !value.Contains("f"))
            {
                value += "f";
            }
        }

        public static bool operator ==(WorldFact x, WorldFact y)
        {
            return x.valueType == y.valueType && x.name == y.name && x.value == y.value;
        }

        public static bool operator !=(WorldFact x, WorldFact y)
        {
            return x.valueType == y.valueType || x.name != y.name || x.value != y.value;
        }

        public static bool IsRequiredValueType(ValueType requiredValueType, string value)
        {
            //object testValue = value;



            //bool boolValue;
            //if (testValue is bool && requiredValueType == ValueType.Bool) return true;

            //float floatValue;
            //if (testValue is float && requiredValueType == ValueType.Float) return true;

            //int intValue;
            //if (testValue is int && requiredValueType == ValueType.Int) return true;


            //Mathf.Approximately()
            //if (testValue is string && requiredValueType == ValueType.String) return true;


            string potentialFloat = "";
            if (value.Length > 0)
            {
                if (value[value.Length - 1] == 'f' && requiredValueType != ValueType.Float)
                {
                    value = value.Remove(value.Length - 1);
                    return false;
                }


                if (value[value.Length - 1] == 'f' && requiredValueType == ValueType.Float)
                {
                    potentialFloat = value.Remove(value.Length - 1);

                }
            }

            bool boolValue;
            if (bool.TryParse(value, out boolValue) && requiredValueType == ValueType.Bool) return true;

            int intValue;
            if (int.TryParse(value, out intValue) && requiredValueType == ValueType.Int) return true;


            float floatValue;
            if (float.TryParse(potentialFloat, out floatValue) && requiredValueType == ValueType.Float) return true;
            

            if (boolValue.ToString() != value && intValue.ToString() != value && floatValue.ToString() != value && value != "" && requiredValueType == ValueType.String) return true;

            return false;
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
