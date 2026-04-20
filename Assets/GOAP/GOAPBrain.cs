using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class GOAPBrain
    {
        public GOAPBlackboard blackboard { get; private set; }
        private GOAPPlanner planner;
        private GOAPGoalSelector goalSelector;

        public GOAPBrain(GOAPBlackboard blackboard)
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