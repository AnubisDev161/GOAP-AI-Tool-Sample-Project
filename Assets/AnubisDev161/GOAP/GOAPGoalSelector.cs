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

            FillGoalList();
        }

        private void FillGoalList()
        {
            // ---------------------------
            // GOAL
            // ---------------------------

            string hasFire = "hasFire";

            // ---------------------------
            // GOAL
            // ---------------------------
            Dictionary<string, bool> hasFireGoalWorldState = new Dictionary<string, bool>();
            hasFireGoalWorldState.Add(hasFire, true);
            goalList.Add(new GOAPGoal(100, hasFireGoalWorldState, hasFire));

            // ---------------------------
            // GOAL
            // ---------------------------

            string hasFood = "hasFood";

            // ---------------------------
            // GOAL
            // ---------------------------
            Dictionary<string, bool> hasFoodGoalWorldState = new Dictionary<string, bool>();
            hasFoodGoalWorldState.Add(hasFood, true);
            goalList.Add(new GOAPGoal(5, hasFoodGoalWorldState, hasFood));
        }

        // TODO add more criteria to find the "best" goal
        public GOAPGoal GetBestGoal()
        {
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