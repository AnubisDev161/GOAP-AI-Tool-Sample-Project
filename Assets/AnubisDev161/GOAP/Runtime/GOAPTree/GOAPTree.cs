using System.Collections.Generic;
using UnityEngine;
using GOAP.Core;

namespace GOAP.Tree
{
    public class GOAPTree
    {
        List<GOAPAction> availableActions;

        public Queue<GOAPAction> GeneratePlan(WorldState blackboard, GOAPGoal goal, List<GOAPAction> availableActions)
        {
            this.availableActions = availableActions;

            var startNode = CreateStartNode(goal);
            var bestPlan = BuildGraph(startNode, blackboard);

            if (bestPlan == null)
            {
                Debug.LogError("Plan is null!");
            }

            return bestPlan;
        }
        private Queue<GOAPAction> BuildGraph(GOAPNode goal, WorldState currentWorldState)
        {
            WorldState goalWorldState = new WorldState(goal.requiredWorldState.worldFacts);
            List<GOAPNode> closedList = new List<GOAPNode>();
            ListBasedPriorityQueue openQueue = new ListBasedPriorityQueue();
            Dictionary<GOAPNode, float> costSoFar = new Dictionary<GOAPNode, float>();

            openQueue.Push(goal);

            while (openQueue.count > 0)
            {
                var currentNode = openQueue.Pop();

                if (WorldStateCompare.IsWorldStateBAchieved(currentWorldState, currentNode.requiredWorldState))
                {
                    // valid plan found

                    return ReconstructPath(currentNode);
                }

                closedList.Add(currentNode);


                if (closedList.Count > 100)
                {
                    Debug.Log("Stuck in loop!");
                    break;
                }
                
                foreach (var action in availableActions)
                {
                    if (!HasAnyRequiredEffects(action, currentNode.requiredWorldState.worldFacts))
                    {
                        continue;
                    }

                    // create copy of parent's world state and apply the action's effects
                    var mutatedWorldState = currentNode.requiredWorldState.Copy();
                    action.RemoveEffectsAndAddPreconditionsToState(mutatedWorldState);


                    if (IsInList(closedList, mutatedWorldState))
                    {
                        continue;
                    }

                    var tentativeGCost = currentNode.gCost + action.GetCost();
                    var hCost = 0;// CalculateHeuristic(mutatedWorldState, goalWorldState);
                    var fCost = tentativeGCost + hCost;



                    //if (openQueue.Contains(mutatedWorldState) && tentativeGCost < openQueue.GetItem(mutatedWorldState).gCost)
                    //{
                    //    var item = openQueue.GetItem(mutatedWorldState);
                    //    item.gCost = tentativeGCost + item.hCost;
                    //    item.parent = currentNode;

                    //}
                    //else if (!openQueue.Contains(mutatedWorldState))
                    {
                        var nodeToAdd = new GOAPNode(action, currentNode, mutatedWorldState, fCost, tentativeGCost, hCost);
                        openQueue.Push(nodeToAdd);
                        
                        //Debug.Log(mutatedWorldState.ToString());
                    }
                }
            }

            Debug.LogError("No valid plan found!");
            return null;
        }

        private bool IsInList(List<GOAPNode> list, WorldState worldStateToFind)
        {
            foreach (var node in list)
            {
                if (worldStateToFind == node.requiredWorldState)
                {
                    return true;
                }
            }

            return false;
        }

        private GOAPNode FindNodeInList(List<GOAPNode> list,  WorldState worldState)
        {
            foreach (var node in list)
            {
                if (node.requiredWorldState == worldState)
                {
                    return node;
                }
            }

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
            GOAPNode startNode = new GOAPNode(null, null, goal.desiredConditions, 0, 0, 0);

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

    public class SortedQueue
    {
        private List<GOAPNode> nodes = new List<GOAPNode>();

        public GOAPNode Pop()
        {
            var node = nodes[0];
            nodes.Remove(node);
            return node;
        }
        public void Push(GOAPNode node)
        {
            nodes.Add(node);
            nodes.Sort((x, y) => x.fCost.CompareTo(y.fCost));
        }
        public float Count()
        {
            return nodes.Count;
        }
    }

}
