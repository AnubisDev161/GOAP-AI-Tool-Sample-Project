using System.Collections.Generic;

namespace GOAP
{
    public static class WorldStateCompare
    {
        public static bool IsWorldStateBAchieved(WorldState worldStateA, WorldState worldStateB)
        {
            foreach (var goalFact in worldStateB.worldFacts)
            {
                if (!worldStateA.worldFacts.TryGetValue(goalFact.Key, out bool value) || value != goalFact.Value)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsWorldStateBAchieved(Dictionary<string, bool> worldStateA, Dictionary<string, bool> worldStateB)
        {
            foreach (var goalFact in worldStateB)
            {
                if (!worldStateA.TryGetValue(goalFact.Key, out bool value) || value != goalFact.Value)
                {
                    return false;
                }
            }

            return true;
        }
    }
}