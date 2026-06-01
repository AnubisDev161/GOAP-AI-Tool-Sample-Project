using GOAP.GOAPGraph;
using System.Collections.Generic;

namespace GOAP.Core
{
    public class GOAPGoal
    {
        private float priority;
        public Dictionary<string, WorldFact> desiredConditions {  get; private set; }
        public string name { get; private set; }
        private bool removeDesiredConditionsFromWorldStateAfterAchieving;
        public GoalWorldStateNode goalNode { get; private set; }

        public GOAPGoal(float priority, Dictionary<string, WorldFact> desiredConditions,  string goalName = "baseGoal", bool removeDesiredConditionsFromWorldStateAfterAchieving = false, GoalWorldStateNode goalNode = null)
        {
            this.priority = priority;
            this.desiredConditions = desiredConditions;
            this.name = goalName;
            this.removeDesiredConditionsFromWorldStateAfterAchieving = removeDesiredConditionsFromWorldStateAfterAchieving;
            this.goalNode = goalNode;
        }

        public virtual bool IsValid()
        {
            return true;
        }

        public virtual float GetPriority()
        {
            return priority;
        }

        public override string ToString()
        {
            string allFacts = "";

            foreach (var state in desiredConditions)
            {
                allFacts += state.Value.ToString() + " | ";
            }

            return $"[{name}] {allFacts}";
        }

        public void Achieved(WorldState currentWorldState)
        {
            if (removeDesiredConditionsFromWorldStateAfterAchieving)
            {
                foreach (var condition in desiredConditions)
                {
                    currentWorldState.TryRemoveFact(condition.Key);
                }
            }

            if (goalNode != null)
            {
                goalNode.OnAchieved();
            }
        }
    }
}