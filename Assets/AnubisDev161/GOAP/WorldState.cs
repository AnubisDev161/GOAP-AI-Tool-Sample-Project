using System.Collections.Generic;
using System;

namespace GOAP
{
    public class WorldState : IComparer<WorldState>
    {
        public Dictionary<string, bool> worldFacts { get; private set; }

        public WorldState(Dictionary<string, bool> worldFacts = null)
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
        public WorldState Copy()
        {
            WorldState mutatedBlackboard = new WorldState();
            foreach (var fact in worldFacts)
            {
                mutatedBlackboard.TryAddFact(new WorldFact(fact.Key, fact.Value));
            }

            return mutatedBlackboard;
        }

        public static bool operator ==(WorldState left, WorldState right)
        {
            if (left.worldFacts.Count != right.worldFacts.Count) return false;

            foreach (var fact in left.worldFacts)
            {
                if (!right.worldFacts.TryGetValue(fact.Key, out var value) || !value.Equals(fact.Value))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator !=(WorldState left, WorldState right)
        {
            if (left.worldFacts.Count != right.worldFacts.Count) return true;

            foreach (var fact in left.worldFacts)
            {
                if (!right.worldFacts.TryGetValue(fact.Key, out var value) || !value.Equals(fact.Value))
                {
                    return true;
                }
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is WorldState)) return false;

            var other = obj as WorldState;

            if (other.worldFacts.Count != worldFacts.Count) return false;

            foreach (var fact in worldFacts)
            {
                if (!other.worldFacts.TryGetValue(fact.Key, out var value) || !value.Equals(fact.Value))
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            string allFacts = "";

            foreach (var state in worldFacts)
            {
                allFacts += state.ToString() + " | ";
            }

            return allFacts;
        }

        public int Compare(WorldState x, WorldState y)
        {
            if (x == y) return 0;
            else
            {
                return 1;
            }
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