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

        public static bool operator >(WorldFact x, WorldFact y)
        {
            if (x.valueType != y.valueType) return false;
            if (x.valueType == ValueType.Bool) return false;

            if (x.valueType == ValueType.Int)
            {
                return (int)x.GetValue() > (int)y.GetValue();
            }

            if (x.valueType == ValueType.Float)
            {
                return (float)x.GetValue() > (float)y.GetValue();
            }

            return false;
        }

        public static bool operator <(WorldFact x, WorldFact y)
        {
            if (x.valueType != y.valueType) return false;
            if (x.valueType == ValueType.Bool) return false;

            if (x.valueType == ValueType.Int)
            {
                return (int)x.GetValue() < (int)y.GetValue();
            }

            if (x.valueType == ValueType.Float)
            {
                return (float)x.GetValue() < (float)y.GetValue();
            }

            return false;
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
            if (valueType == ValueType.Float) return Convert.ToSingle(value.Remove(value.Length -1));

            return null;
        }

        public bool SetValue(string value)
        {
            if (IsRequiredValueType(valueType, value))
            {
                this.value = value;
                return true;
            }

            return false;
        }

        public bool IsRequiredValue(WorldFact other)
        {
            switch (this.acceptedValue)
            {
                case AcceptedValue.Equals:
                    return (other == this);
                case AcceptedValue.Grater:
                    return other > this;
                case AcceptedValue.Samller:
                    return other < this;
                default:
                    Debug.LogError("Accepted type could not be evaluated");
                    return false;
            }
        }

        public static bool IsRequiredValueType(ValueType requiredValueType, string value)
        {
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

            return false;
        }

        public override string ToString()
        {
            return $"{name}, {acceptedValue}, {value}, {valueType.ToString()}";
        }
    }

    [Serializable]
    public enum ValueType
    {
        Bool,
        Int,
        Float,
    }

    [Serializable]
    public enum AcceptedValue
    {
        None,
        Equals,
        Grater,
        Samller,
    }

    [Serializable]
    public enum OperationType
    {
        None,
        Set,
        Increase,
        Decrease
    }
}