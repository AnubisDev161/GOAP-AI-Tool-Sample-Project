using GOAP.Data;
using System.Collections.Generic;

namespace GOAP
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

            GOAPGoal bestGoal = goalList[0];
            foreach (GOAPGoal goal in goalList)
            {
                if (bestGoal.GetPriority() < goal.GetPriority())
                {
                    bestGoal = goal;
                }
            }

            currentGoal = bestGoal;
            return bestGoal;
        }
    }
}