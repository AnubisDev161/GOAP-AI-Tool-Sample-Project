using GOAP.Tree;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP.Core.Agent
{
    public class GOAPPlanner
    {
        private GOAPTree tree;
  
        public GOAPPlanner()
        {
            tree = new GOAPTree();
        }

        public Queue<GOAPAction> GeneratePlan(WorldState currentWorldState, GOAPGoal goal, List<GOAPAction> availableActions, GOAPGraph.GOAPGraphAsset graphInstance)
        {
            return tree.GeneratePlan(currentWorldState, goal, availableActions, graphInstance);
        }
    }
}
    