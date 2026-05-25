using System.Collections.Generic;
using UnityEngine;
using GOAP.Core;

namespace GOAP.Tree
{
    public class GOAPTree
    {
        private List<GOAPAction> availableActions;

        // Depending on the size of your plans, this number needs to be adjusted to either decrease or increase the allowed number of iterations per plan.
        private int maxClosedListSize = 400;

        public Queue<GOAPAction> GeneratePlan(WorldState currentWorldState, GOAPGoal goal, List<GOAPAction> availableActions, GOAPGraph.GOAPGraphAsset graphInstance)
        {
            this.availableActions = availableActions;

            var startNode = CreateStartNode(goal);
            var bestPlan = BuildGraph(startNode, currentWorldState, graphInstance);

            if (bestPlan == null)
            {
                Debug.LogError("Plan is null!");
            }

            return bestPlan;
        }
        private Queue<GOAPAction> BuildGraph(GOAPNode goal, WorldState currentWorldState, GOAPGraph.GOAPGraphAsset graphInstance)
        {
            WorldState goalWorldState = new WorldState(goal.requiredWorldState.worldFacts);
            Dictionary<WorldState, GOAPNode> closedList = new Dictionary<WorldState, GOAPNode>();
            ListBasedPriorityQueue openQueue = new ListBasedPriorityQueue();

            openQueue.Push(goal);

            while (openQueue.count > 0)
            {
                var currentNode = openQueue.Pop();
                Debug.Log("Open queue size: " + openQueue.count);

                if (WorldStateCompare.IsWorldStateBAchieved(currentWorldState, currentNode.requiredWorldState))
                {
                    // valid plan found
                    Debug.Log($"Valid plan found, iterations: {closedList.Count}");
                    Debug.Log($"Available actions: {availableActions.Count}");
                    return ReconstructPath(currentNode);
                }

                closedList.Add(currentNode.requiredWorldState, currentNode);

                if (closedList.Count > maxClosedListSize)
                {
                    Debug.Log("Stuck in loop!");
                    break;
                }
                
                foreach (var action in availableActions)
                {
                    if (!HasAnyRequiredEffects(action, currentNode.requiredWorldState.worldFacts) || !action.IsAchvievable(graphInstance))
                    {
                        continue;
                    }
                  
                    // create copy of parent's world state and apply the action's effects
                    var mutatedWorldState = currentNode.requiredWorldState.Copy();
                    action.RemoveEffectsAndAddPreconditionsToState(mutatedWorldState);

                    var tentativeGCost = currentNode.gCost + action.GetCost();
                    var hCost = CalculateHeuristic(mutatedWorldState, goalWorldState);

                    if (closedList.ContainsKey(mutatedWorldState))
                    {
                        continue;
                    }

                    if (openQueue.Contains(mutatedWorldState) && tentativeGCost < openQueue.GetItem(mutatedWorldState).gCost)
                    {
                        openQueue.ReplaceItem(new GOAPNode(action, currentNode, mutatedWorldState, tentativeGCost, hCost));

                    }
                    else if (!openQueue.Contains(mutatedWorldState))
                    {
                        var nodeToAdd = new GOAPNode(action, currentNode, mutatedWorldState, tentativeGCost, hCost);
                        openQueue.Push(nodeToAdd);
                    }
                }
            }

            Debug.LogError("No valid plan found!");
            return null;
        }

        private int CalculateHeuristic(WorldState worldState, WorldState goalState)
        {
            int h = 0;
            foreach (var goalFact in goalState.worldFacts)
            {
                if (!worldState.worldFacts.TryGetValue(goalFact.Key, out var value) || value != goalFact.Value)
                {
                    h++;
                }
            }

            return h;
        }

        private Queue<GOAPAction> ReconstructPath(GOAPNode currentWorldState)
        {
            var path = new Queue<GOAPAction>();
            var node = currentWorldState;

            while (node.action != null)
            {
                path.Enqueue(node.action);
                node = node.parent;
            }

            return path;
        }

        private GOAPNode CreateStartNode(GOAPGoal goal)
        {
            GOAPNode startNode = new GOAPNode(null, null, goal.desiredConditions, 0, 0);

            return startNode;
        }

        private bool HasAnyRequiredEffects(GOAPAction action, Dictionary<string, WorldFact> preconditions)
        {
            bool satisfiesAtLeastOne = false;
            foreach (var effect in action.effects)
            {
                if (preconditions.TryGetValue(effect.Key, out WorldFact value))
                {
                    if (value.IsRequiredOperation(effect.Value))
                    {
                        satisfiesAtLeastOne = true; // match
                    }
                    else
                    {
                        // Contradiction
                        return false;
                    }
                }
            }

            return satisfiesAtLeastOne;
        }
    }
}
