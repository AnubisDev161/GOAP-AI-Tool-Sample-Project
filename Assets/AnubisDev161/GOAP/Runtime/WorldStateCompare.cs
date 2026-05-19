using System.Collections.Generic;

namespace GOAP.Data
{
    public static class WorldStateCompare
    {
        public static bool IsWorldStateBAchieved(WorldState worldStateA, WorldState worldStateB)
        {
            foreach (var goalFact in worldStateB.worldFacts)
            {
                if (!worldStateA.worldFacts.TryGetValue(goalFact.Key, out var value) || !goalFact.Value.IsRequiredValue(value))
                {
                    return false;
                }
            }

            return true;
        }
        public static bool IsWorldStateBAchieved(Dictionary<string, WorldFact> worldStateA, Dictionary<string, WorldFact> worldStateB)
        {
            foreach (var goalFact in worldStateB)
            {
                if (!worldStateA.TryGetValue(goalFact.Key, out var value) || !goalFact.Value.IsRequiredValue(value)) 
                {
                    return false;
                }
            }

            return true;
        }
    }
}