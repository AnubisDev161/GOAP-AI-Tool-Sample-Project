using GOAP.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class GOAPBlackbaord
    {
        private Dictionary<string, BlackboardKey> blackboardKeys = new Dictionary<string, BlackboardKey>();

        public BlackboardKey GetKey(string key)
        {
            if (blackboardKeys.ContainsKey(key))
            {
                return blackboardKeys[key];
            }

            return null;
        }

        public bool AddKey(string key, BlackboardKey value)
        {

            if (blackboardKeys.ContainsKey(key)) return false;

            blackboardKeys.Add(key, value);
            return true;
        }

        public class BlackboardKey
        {
            public object key;

            public BlackboardKeyType keyType;
        }

        public enum BlackboardKeyType
        {
            Bool,
            Int,
            Float,
            String,
            Vector3,
            Gameobject
        }

    }
}