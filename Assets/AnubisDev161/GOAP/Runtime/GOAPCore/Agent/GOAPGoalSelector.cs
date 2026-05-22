using System.Collections.Generic;
using UnityEngine;

namespace GOAP.Core.Agent
{
    public class GOAPGoalSelector 
    {
        private WorldState blackboard;
        private List<GOAPGoal> goalList;
        public GOAPGoal currentGoal { get; private set; }
        public GOAPGoalSelector(WorldState blackboard)
        {
            this.blackboard = blackboard;
            goalList = new List<GOAPGoal>();
        }

        // TODO add more criteria to find the "best" goal
        public GOAPGoal GetBestGoal(WorldState worldState, List<GOAPGoal> availableGoals)
        {
            goalList = availableGoals;

            GOAPGoal bestGoal = null;
            foreach (GOAPGoal goal in goalList)
            {
                if (!WorldStateCompare.IsWorldStateBAchieved(worldState.worldFacts, goal.desiredConditions) && (bestGoal == null || bestGoal.GetPriority() < goal.GetPriority()))
                {
                    bestGoal = goal;
                }
            }

            currentGoal = bestGoal;

            ValidateCurrenGoal();

            return bestGoal;
        }

        private void ValidateCurrenGoal()
        {
            if (currentGoal == null)
            {
                string errorReason = (goalList.Count > 0) ? "All remaining goals are satisfied" : "no available goals found";
                Debug.LogError($"No valid goal found due to: {errorReason}");
            }
        }
    }
}