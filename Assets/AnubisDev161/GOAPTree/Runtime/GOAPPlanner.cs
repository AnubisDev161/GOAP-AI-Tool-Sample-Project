using GOAP.Data;
using GOAP.Tree;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class GOAPPlanner
    {
        private GOAPTree tree;
  
        public GOAPPlanner()
        {
            tree = new GOAPTree();
        }

        public Queue<GOAPAction> GeneratePlan(WorldState blackboard, GOAPGoal goal, List<GOAPAction> availableActions)
        {
            return tree.GeneratePlan(blackboard, goal, availableActions);
        }
    }
}
    