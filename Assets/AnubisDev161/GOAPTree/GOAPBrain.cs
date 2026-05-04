using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class GOAPBrain
    {
        public WorldState blackboard { get; private set; }
        private GOAPPlanner planner;
        public GOAPGoalSelector goalSelector;

        public GOAPBrain(WorldState blackboard)
        {
            this.blackboard = blackboard;
            planner = new GOAPPlanner();
            goalSelector = new GOAPGoalSelector(blackboard);

        }
        public Queue<GOAPAction> CreatePLan(GOAPGoal goal)
        {
            return planner.GeneratePlan(blackboard, goal);
        }
    }
}