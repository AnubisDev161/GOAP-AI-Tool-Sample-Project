using System;
using System.Linq;
using UnityEditor;
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
            //Debug.Log(name);
            //Debug.Log(value);
            //Debug.Log(valueType);
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

        public object GetValue()
        {
            if (valueType == ValueType.Bool) return Convert.ToBoolean(value);
            if (valueType == ValueType.Int) return Convert.ToInt32(value);
            if (valueType == ValueType.Float) return Convert.ToSingle(value);
            if (valueType == ValueType.String) return value;
            if (valueType == ValueType.Vector3) return ConvertValueToVector3(value);
    
            return null;
        }

        public static Vector3 ConvertValueToVector3(string value)
        {
            var numbers = value.Split(", ");

            if (numbers.Length > 3)
            {
                throw new ArgumentException("could not convert value to vector, value contains more than 3 floats!");
            }

            var test = numbers[0].Split("(");

            numbers[0] = numbers[0].TrimStart('(');
            numbers[2] = numbers[2].Remove(numbers[2].Length - 1);

            return new Vector3(Convert.ToSingle(numbers[0]), Convert.ToSingle(numbers[1]), Convert.ToSingle(numbers[2]));
        }

        public static bool IsRequiredValueType(ValueType requiredValueType, string value)
        {
            // check if value is a vector3
            if (requiredValueType == ValueType.Vector3 && value[0] == '(' && value[value.Length -1] == ')')
            {
                return true;
            }

         
            string potentialFloat = "";
            if (value.Length > 0)
            {
                // check if value is a float, but the required one is not a float, remove the actual value "f"
                if (value[value.Length - 1] == 'f' && requiredValueType != ValueType.Float)
                {
                    value = value.Remove(value.Length - 1);
                    return false;
                }

                // check if value is a float. If this is the case, set the potential float equal to the value without the "f"
                if (value[value.Length - 1] == 'f' && requiredValueType == ValueType.Float)
                {
                    potentialFloat = value.Remove(value.Length - 1);

                }
            }

            // check if value is a bool
            bool boolValue;
            if (bool.TryParse(value, out boolValue) && requiredValueType == ValueType.Bool) return true;

            // check if value is an int
            int intValue;
            if (int.TryParse(value, out intValue) && requiredValueType == ValueType.Int) return true;

            // check if value is a bool
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
        String,
        Vector3

    }
}
