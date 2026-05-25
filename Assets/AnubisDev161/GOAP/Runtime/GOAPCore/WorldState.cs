using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GOAP.Core
{
    public class WorldState
    {
        public Action worldStateChangedByTrigger;
        public Dictionary<string, WorldFact> worldFacts { get; private set; }

        public WorldState(Dictionary<string, WorldFact> worldFacts = null)
        {
            if (worldFacts == null)
            {
                worldFacts = new Dictionary<string, WorldFact>();
            }

            this.worldFacts = worldFacts;
        }

        public bool TryAddFact(WorldFact worldFact, bool callWorldStateChangedByTrigger = false)
        {
            bool stateChanged = false;
            if (!worldFacts.ContainsKey(worldFact.name))
            {
                worldFacts.Add(worldFact.name, worldFact);
                stateChanged = true;
            }
            else if (worldFacts[worldFact.name] != worldFact)
            {
                worldFacts[worldFact.name] = worldFact;
                stateChanged =  true;
            }

            if (stateChanged)
            {
                if (callWorldStateChangedByTrigger)
                {
                    worldStateChangedByTrigger?.Invoke();
                }

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
                worldFacts[worldFact.name] = worldFact;
                return true;
            }

            return false;
        }
        public WorldState Copy()
        {
            WorldState mutatedWorldState = new WorldState();
            foreach (var fact in worldFacts)
            {
                mutatedWorldState.TryAddFact(fact.Value);
            }

            return mutatedWorldState;
        }

        public static bool operator ==(WorldState left, WorldState right)
        {
            //if (left.worldFacts.Count != right.worldFacts.Count) return false;

            //foreach (var fact in left.worldFacts)
            //{
            //    if (!right.worldFacts.TryGetValue(fact.Key, out var value) || value != fact.Value || value.operationType != fact.Value.operationType || value.acceptedValue != fact.Value.acceptedValue)
            //    {
            //        return false;
            //    }
            //}

            //return true;

            if (left.worldFacts.Count != right.worldFacts.Count) return false;
            if (left.worldFacts.Count == 0) return true;

            int leftIndex = 0;
            int rightIndex = 0;

            foreach (var fact in left.worldFacts)
            {
                foreach(var rightFact in right.worldFacts)
                {
                    if (fact.Key == rightFact.Key && leftIndex == rightIndex)
                    {
                        if (rightFact.Value == rightFact.Value && rightFact.Value.operationType == rightFact.Value.operationType && fact.Value.acceptedValue == rightFact.Value.acceptedValue)
                        {
                            return true;
                        }
                    }
                    rightIndex++;
                }

                leftIndex++;
            }

            return false;
        }

        public static bool operator !=(WorldState left, WorldState right)
        {
            //if (left.worldFacts.Count != right.worldFacts.Count) return true;

            //foreach (var fact in left.worldFacts)
            //{
            //    if (!right.worldFacts.TryGetValue(fact.Key, out var value) || value != fact.Value || value.operationType != fact.Value.operationType || value.acceptedValue != fact.Value.acceptedValue)
            //    {
            //        return true;
            //    }
            //}

            //return false;

            if (left.worldFacts.Count != right.worldFacts.Count) return true;

            int leftIndex = 0;
            int rightIndex = 0;

            foreach (var fact in left.worldFacts)
            {
                foreach (var rightFact in right.worldFacts)
                {
                    if (fact.Key == rightFact.Key && leftIndex == rightIndex)
                    {
                        if (rightFact.Value == rightFact.Value && rightFact.Value.operationType == rightFact.Value.operationType && fact.Value.acceptedValue == rightFact.Value.acceptedValue)
                        {
                            return false;
                        }
                    }
                    rightIndex++;
                }

                leftIndex++;
            }

            return true;
        }

        public override string ToString()
        {
            string allFacts = "";

            foreach (var state in worldFacts)
            {
                allFacts += state.Value.ToString() + " | ";
            }

            return allFacts;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is WorldState)) return false;

            return (WorldState)obj == this;
        }
    }
}