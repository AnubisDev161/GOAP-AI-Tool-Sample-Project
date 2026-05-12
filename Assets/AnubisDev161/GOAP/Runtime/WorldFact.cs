using System;
using System.Globalization;
using UnityEngine;

namespace GOAP.Data
{
    [Serializable]
    public struct WorldFact : ISerializationCallbackReceiver
    {
        [ExposedProperty]
        public string name;

        [SerializeField]
        public string value;

        [ExposedProperty]
        public ValueType valueType;

        [ExposedProperty]
        public AcceptedValue acceptedValue; 
        
        public void OnAfterDeserialize()
        {

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
            return x.valueType != y.valueType || x.name != y.name || x.value != y.value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj is not WorldFact) return false;

            return (WorldFact)obj == this;
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

            numbers[0] = numbers[0].TrimStart('(');
            numbers[2] = numbers[2].Remove(numbers[2].Length - 1);

            foreach (var number in numbers)
            {
                number.Replace(',', '.');
            }
            var vector = new Vector3(Convert.ToSingle(numbers[0], CultureInfo.InvariantCulture), Convert.ToSingle(numbers[1], CultureInfo.InvariantCulture), Convert.ToSingle(numbers[2], CultureInfo.InvariantCulture));

            return vector;
        }

        public static bool IsRequiredValueType(ValueType requiredValueType, string value)
        {
            // check if value is a vector3
            if (requiredValueType == ValueType.Vector3 && value[0] == '(' && value[value.Length - 1] == ')')
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

        public override string ToString()
        {
            return $"{name}, {value}, {valueType.ToString()}";
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

    [Serializable]
    public enum AcceptedValue
    {
        Any,
        Grater,
        Samller,
        Equals
    }
}