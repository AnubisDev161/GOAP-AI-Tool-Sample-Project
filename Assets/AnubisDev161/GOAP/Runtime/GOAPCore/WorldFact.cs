using GOAP.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP.Core
{
    [Serializable]
    public struct WorldFact : ISerializationCallbackReceiver
    {
        [ExposedProperty]
        public string name;

        [SerializeField]
        public string value;

        [ExposedProperty]
        public WorldFactType valueType;

        [SerializeField]
        public AcceptedValue acceptedValue;

        [SerializeField]
        public OperationType operationType;

        public void OnAfterDeserialize()
        {

        }

        public void OnBeforeSerialize()
        {
            if (valueType == WorldFactType.Float && !value.Contains("f"))
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
            if (x.valueType == WorldFactType.Bool) return false;

            if (x.valueType == WorldFactType.Int)
            {
                return (int)x.GetValue() > (int)y.GetValue();
            }

            if (x.valueType == WorldFactType.Float)
            {
                return (float)x.GetValue() > (float)y.GetValue();
            }

            return false;
        }

        public static bool operator <(WorldFact x, WorldFact y)
        {
            if (x.valueType != y.valueType) return false;
            if (x.valueType == WorldFactType.Bool) return false;

            if (x.valueType == WorldFactType.Int)
            {
                return (int)x.GetValue() < (int)y.GetValue();
            }

            if (x.valueType == WorldFactType.Float)
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
            if (valueType == WorldFactType.Bool) return Convert.ToBoolean(value);
            if (valueType == WorldFactType.Int) return Convert.ToInt32(value);
            if (valueType == WorldFactType.Float) return Convert.ToSingle(value.Remove(value.Length - 1));

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
                    return other == this;
                case AcceptedValue.Greater:
                    return other > this;
                case AcceptedValue.Samller:
                    return other < this;
                default:
                    Debug.LogError("Accepted type could not be evaluated");
                    return false;
            }
        }

        public static bool IsRequiredValueType(WorldFactType requiredValueType, string value)
        {
            string potentialFloat = "";
            if (value.Length > 0)
            {
                // check if value is a float, but the required one is not a float, remove the actual value "f"
                if (value[value.Length - 1] == 'f' && requiredValueType != WorldFactType.Float)
                {
                    value = value.Remove(value.Length - 1);
                    return false;
                }

                // check if value is a float. If this is the case, set the potential float equal to the value without the "f"
                if (value[value.Length - 1] == 'f' && requiredValueType == WorldFactType.Float)
                {
                    potentialFloat = value.Remove(value.Length - 1);

                }
            }

            // check if value is a bool
            bool boolValue;
            if (bool.TryParse(value, out boolValue) && requiredValueType == WorldFactType.Bool) return true;

            // check if value is an int
            int intValue;
            if (int.TryParse(value, out intValue) && requiredValueType == WorldFactType.Int) return true;

            // check if value is a bool
            float floatValue;
            if (float.TryParse(potentialFloat, out floatValue) && requiredValueType == WorldFactType.Float) return true;

            return false;
        }

        public override string ToString()
        {
            return $"{name}, {acceptedValue}, {value}, {valueType.ToString()}";
        }

        public void InitValue()
        {
            switch (valueType)
            {
                case WorldFactType.Bool:
                    value = "False";
                    break;
                case WorldFactType.Int:
                    value = "0";
                    break;
                case WorldFactType.Float:
                    value = "0.0f";
                    break;
                default:
                    Debug.LogError("Could not convert valueType to known value");
                    break;
            }
        }

        public void ChangeWorldFactAccordingToOperationType(Dictionary<string, WorldFact> worldFacts)
        {
            WorldFact newValue;
            if (worldFacts.ContainsKey(name))
            {
                newValue = worldFacts[name];
            }
            else
            {
                newValue = new WorldFact();
                newValue.valueType = this.valueType;
                newValue.name = name;
                newValue.InitValue();
            }

            switch (operationType)
            {
                case OperationType.Set:
                    worldFacts[name] = this;
                    return;

                case OperationType.Add:
                    newValue.value = ChangeValue(newValue);
                    worldFacts[name] = newValue;
                    return;
            }
        }

        private string ChangeValue(WorldFact worldFact)
        {
            if (worldFact.valueType != this.valueType)
            {
                Debug.LogError("Fatal error, value to increase does not have the same value as the value to add");
                return string.Empty;
            }

            return AddOrSubtractValue(worldFact);
        }

        private string AddOrSubtractValue(WorldFact worldFact)
        {
            switch (valueType)
            {
                case WorldFactType.Bool:
                    Debug.LogError("Can't increase boolen!");
                    return string.Empty;

                case WorldFactType.Int:
                    int intValue;
                    intValue = Convert.ToInt32(worldFact.value) + Convert.ToInt32(this.value);

                    return Convert.ToString(intValue);

                case WorldFactType.Float:
                    float floatValue;
                    floatValue = Convert.ToSingle(worldFact.value.Remove(worldFact.value.Length - 1)) + Convert.ToSingle(this.value.Remove(this.value.Length - 1));
                   
                    return Convert.ToString(floatValue + "f");
                default:
                    Debug.LogError("Could not evaluate value type");
                    break;
            }

            return string.Empty;
        }
    }

    [Serializable]
    public enum WorldFactType
    {
        Bool,
        Int,
        Float,
    }

    [Serializable]
    public enum AcceptedValue
    {
        Equals,
        Greater,
        Samller,
    }

    [Serializable]
    public enum OperationType
    {
        Set,
        Add
    }
}