using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

            //FillGoalList();
        }

        private void FillGoalList()
        {
            //// ---------------------------
            //// GOAL
            //// ---------------------------

            //string hasFire = "hasFire";
            //string hasFood = "hasFood";
            //string hasShelter = "hasShelter";
            //string haveAHome = "haveAHome";

            //// ---------------------------
            //// GOAL
            //// ---------------------------
            //Dictionary<string, bool> hasFireGoalWorldState = new Dictionary<string, bool>();
            //hasFireGoalWorldState.Add(hasFood, true);
            //hasFireGoalWorldState.Add(hasShelter, true);
            //goalList.Add(new GOAPGoal(100, hasFireGoalWorldState, haveAHome));

            //// ---------------------------
            //// GOAL
            //// ---------------------------

       

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