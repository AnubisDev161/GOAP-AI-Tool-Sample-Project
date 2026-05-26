using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP.Core.Agent
{
    [Serializable]
    public class GOAPBlackbaord : ISerializationCallbackReceiver
    {
        private Dictionary<string, BlackboardKey> blackboardKeys = new Dictionary<string, BlackboardKey>();

        private Dictionary<string, BlackboardKey> worldFacts = new Dictionary<string, BlackboardKey>();

        [SerializeField]
        public List<string> facts = new List<string>();

        [SerializeField]
        private List<BlackboardKey> factValues = new List<BlackboardKey>();

        [SerializeField]
        private List<string> keys = new List<string>();

        [SerializeReference]
        private List<BlackboardKey> values = new List<BlackboardKey>();

        public Dictionary<string, BlackboardKey> GetKeys()
        {
            return blackboardKeys;
        }

        public Dictionary<string, BlackboardKey> GetWorldFacts()
        {
            return worldFacts;
        }

        public bool Contains(string keyName)
        {
            if (blackboardKeys.ContainsKey(keyName)) return true;    
            if (worldFacts.ContainsKey(keyName)) return true;

            return false;
        }

        public BlackboardKey GetWorldFactWithExpectedType(string keyName, WorldFactType expectedType)
        {
            if (worldFacts.TryGetValue(keyName, out BlackboardKey key))
            {
                if (key.worldFactType != expectedType)
                {
                    Debug.LogError($"The BlackboardKey with name: {keyName} has a different worldFactType [{key.worldFactType}] than the expected WorldFactType [{expectedType}]");
                    return null;
                }

                return worldFacts[keyName];
            }

            Debug.LogError($"Blackboard does not contain a BlackboardKey with name: {keyName}");
            return null;
        }

        public BlackboardKey GetKeyWithExpectedType(string keyName, BlackboardKeyType expectedType)
        {
            if (blackboardKeys.TryGetValue(keyName, out BlackboardKey key))
            {
                if (key.keyType != expectedType)
                {
                    Debug.LogError($"The BlackboardKey with name: {keyName} has a different keyType [{key.keyType}] than the expected keyType [{expectedType}]");
                    return null;
                }

                return blackboardKeys[keyName];
            }

            Debug.LogError($"Blackboard does not contain a BlackboardKey with name: {keyName}");
            return null;
        }

        public bool SetKey(string keyName, BlackboardKey value)
        {
            if (blackboardKeys.ContainsKey(keyName))
            {
                blackboardKeys[keyName] = value;
                return true;
            }

            if (worldFacts.ContainsKey(keyName))
            {
                worldFacts[keyName] = value;
                return true;
            }

            Debug.LogError($"Blackboard does not contain a BlackboardKey with name: {keyName}");
            return false;
        }

        public bool AddKey(string keyName, BlackboardKeyType keyType, object value = null)
        {
            if (blackboardKeys.ContainsKey(keyName))
            {
                Debug.LogError($"Blackboard already contains a BlackboardKey with name: {keyName}");
                return false;
            }

            var newKey = new BlackboardKey(value, keyType, isWorldFact: false);
            blackboardKeys.Add(keyName, newKey);

            Debug.Log("Blackboard key added");
            return true;
        }

        public bool AddKey(string keyName, WorldFactType worldFactType, object value = null)
        {
            if (worldFacts.ContainsKey(keyName))
            {
                Debug.LogError($"Blackboard already contains a BlackboardKey with name: {keyName}");
                return false;
            }
            
            var newKey = new BlackboardKey(value, worldFactType: worldFactType, isWorldFact: true);
            worldFacts.Add(keyName, newKey);
  
            Debug.Log("Blackboard key added");
            return true;
        }

        public bool RemoveKey(string keyName)
        {
            if (blackboardKeys.ContainsKey(keyName))
            {
                blackboardKeys.Remove(keyName);
                Debug.Log("Blackboard key removed");
                return true;
            }

            if (worldFacts.ContainsKey(keyName))
            {
                worldFacts.Remove(keyName);
                Debug.Log("Blackboard key removed");
                return true;
            }
   
            Debug.LogError($"Trying to delete non existing BlackboardKey with name {keyName}");
            return false;
        }

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            facts.Clear();
            factValues.Clear();
           
            foreach (var element in blackboardKeys)
            {
                keys.Add(element.Key);
                values.Add(element.Value);
            }

            foreach (var element in worldFacts)
            {
                facts.Add(element.Key);
                factValues.Add(element.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            for (int i = 0; i < keys.Count; i++)
            {
                if (blackboardKeys.ContainsKey(keys[i])) continue;
                blackboardKeys.Add(keys[i], values[i]);
            }

            for (int i = 0; i < facts.Count; i++)
            {
                if (worldFacts.ContainsKey(facts[i])) continue;
                worldFacts.Add(facts[i], factValues[i]);
            }
        }

        [Serializable]
        public class BlackboardKey
        {
            public object value { get; set; }

            [SerializeField]
            public bool isWorldFact = false;

            [SerializeField]
            public BlackboardKeyType keyType;

            [SerializeField]
            public WorldFactType worldFactType;

            public BlackboardKey(object value, BlackboardKeyType keyType = BlackboardKeyType.Bool, WorldFactType worldFactType = WorldFactType.Bool, bool isWorldFact = false)
            {
                this.value = value;
                this.keyType = keyType;
                this.worldFactType = worldFactType;
                this.isWorldFact = isWorldFact;
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