using GOAP.Data;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GOAP
{
    [Serializable]
    public class GOAPBlackbaord : ISerializationCallbackReceiver
    {
        private Dictionary<string, BlackboardKey> blackboardKeys = new Dictionary<string, BlackboardKey>();

        [SerializeField]
        private List<string> keys = new List<string>();

        [SerializeReference]
        private List<BlackboardKey> values = new List<BlackboardKey>();

        public Dictionary<string, BlackboardKey> GetKeys()
        {
            return blackboardKeys;
        }

        public BlackboardKey GetKey(string keyName)
        {
            if (blackboardKeys.ContainsKey(keyName))
            {
                return blackboardKeys[keyName];
            }

            Debug.LogError($"Blackboard does not contain a BlackboardKey with name {keyName}");
            return null;
        }

        public bool AddKey(string keyName, BlackboardKeyType keyType, object value = null)
        {
            if (blackboardKeys.ContainsKey(keyName))
            {
                Debug.LogError($"Blackboard already contains a BlackboardKey with name {keyName}");
                return false;
            }

            var newKey = new BlackboardKey(value, keyType);
            blackboardKeys.Add(keyName, newKey);
            Debug.Log("Blackboard key added");
            return true;
        }

        public bool RemoveKey(string keyName)
        {
            if (!blackboardKeys.ContainsKey(keyName))
            {
                Debug.LogError($"Trying to delete non existing BlackboardKey with name {keyName}");
                return false;
            }

            Debug.Log("Blackboard key removed");
            blackboardKeys.Remove(keyName);
            return true;
        
        }

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            foreach (var element in blackboardKeys)
            {
                keys.Add(element.Key);
                values.Add(element.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            for (int i = 0; i < keys.Count; i++)
            {
                blackboardKeys.Add(keys[i], values[i]);
            }
        }

        [Serializable]
        public class BlackboardKey
        {
            [DoNotSerialize]
            public object value;

            [SerializeField]
            public BlackboardKeyType keyType;

            public BlackboardKey(object value, BlackboardKeyType keyType)
            {
                this.value = value;
                this.keyType = keyType;
            }
        }

        [Serializable]
        public enum BlackboardKeyType
        {
            Bool,
            Int,
            Float,
            String,
            Vector3,
            GameObject
        }
    }
}