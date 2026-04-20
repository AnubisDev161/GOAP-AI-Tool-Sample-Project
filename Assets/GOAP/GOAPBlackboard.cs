using System.Collections.Generic;
using System;
using UnityEngine;

namespace GOAP
{
    public class GOAPBlackboard
    {
        public Dictionary<string, bool> worldFacts { get; private set; }

        public GOAPBlackboard(Dictionary<string, bool> worldFacts = null)
        {
            if (worldFacts == null)
            {
                worldFacts = new Dictionary<string, bool>();
            }

            this.worldFacts = worldFacts;
        }

        public bool TryAddFact(WorldFact worldFact)
        {
            if (!worldFacts.ContainsKey(worldFact.name))
            {
                worldFacts.Add(worldFact.name, worldFact.value);
                return true;
            }
            else if (worldFacts[worldFact.name] != worldFact.value)
            {
                worldFacts[worldFact.name] = worldFact.value;
                return true;
            }

           return false;
        }

        public bool TryRemoveFact(string worldFact)
        {
            if (worldFacts.ContainsKey(worldFact))
            {
                worldFacts.Remove(worldFact);
                return true;
            }

            return false;
        }

        public bool TrySetFact(WorldFact worldFact)
        {
            if (worldFacts.ContainsKey(worldFact.name))
            {
                worldFacts[worldFact.name] = worldFact.value;
                return true;
            }

            return false;
        }
        public GOAPBlackboard Copy()
        {
            GOAPBlackboard mutatedBlackboard = new GOAPBlackboard();
            foreach (var fact in worldFacts)
            {
                mutatedBlackboard.TryAddFact(new WorldFact(fact.Key, fact.Value));
            }

            return mutatedBlackboard;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is GOAPBlackboard)) return false;

            var other = obj as GOAPBlackboard;

            if (other.worldFacts.Count != worldFacts.Count) return false;

            foreach (var fact in worldFacts)
            {
                if (!other.worldFacts.TryGetValue(fact.Key, out var value) || value == fact.Value)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public struct WorldFact
    {
        public string name {  get; private set; }
        public bool value { get; private set; }

        public WorldFact(string name, bool value)
        {
            this.name = name;
            this.value = value;
        }
    }
}