using System;
using System.Collections.Generic;

namespace GOAP.Tree
{
    public class GOAPTree
    {
        List<GOAPAction> availableActions;

        public Queue<GOAPNode> GeneratePlan(GOAPBlackboard blackboard, GOAPGoal goal, List<GOAPAction> availableActions)
        {
            this.availableActions = availableActions;

            var startNode = CreateStartNode(goal);
            var bestPlan = BuildGraph(startNode, blackboard);

            if (bestPlan.Count == 0)
            {
                Console.WriteLine("No valid PLan found!");
            }

            return bestPlan;
        }
        private Queue<GOAPNode> BuildGraph(GOAPNode goal, GOAPBlackboard currentWorldState)
        {
            GOAPBlackboard goalWorldState = new GOAPBlackboard(goal.requiredWorldState.worldFacts);
            List<GOAPNode> closedList = new List<GOAPNode>();
            ListBasedPriorityQueue openQueue = new ListBasedPriorityQueue();
            openQueue.Push(goal);

            while (openQueue.count > 0)
            {
                var currentNode = openQueue.Pop();

                if (IsCurrentWorldStateAchieved(currentWorldState.worldFacts, currentNode.requiredWorldState.worldFacts))
                {
                    // valid plan found

                    return ReconstructPath(closedList);
                }

                closedList.Add(currentNode);

                foreach (var action in availableActions)
                {
                    var parentNode = currentNode;

                    if (!HasAnyRequiredEffects(action, parentNode.requiredWorldState.worldFacts))
                    {
                        continue;
                    }

                    // create copy of parent's world state and apply the action's effects
                    var mutatedWorldState = parentNode.requiredWorldState.Copy();
                    action.RemoveEffectsAndAddPreconditionsToState(mutatedWorldState);

                    var tentativeGCost = parentNode.gCost + action.GetCost();
                    var hCost = CalculateHeuristic(mutatedWorldState.worldFacts, goalWorldState.worldFacts);
                    var fCost = tentativeGCost + hCost;

                    var nodeToAdd = new GOAPNode(action, parentNode, mutatedWorldState, fCost, tentativeGCost, hCost);


                    if (closedList.Contains(nodeToAdd) && !TryReplaceExistingIfCheaper(closedList, nodeToAdd)) continue;
                  

                    openQueue.Push(nodeToAdd);
                }
            }

            MessageBus.print("No valid plan found!");
            return null;
        }

        private bool TryReplaceExistingIfCheaper(List<GOAPNode> closedList, GOAPNode nodeToAdd)
        {
            GOAPNode foundNode = null;
            foreach (var node in closedList)
            {
                if (node == nodeToAdd && (node.fCost < nodeToAdd.fCost || node.fCost == nodeToAdd.fCost && node.hCost < nodeToAdd.hCost))
                {
                    closedList.Remove(node);
                    return true;
                }
            }

            return false;
        }

        private int CalculateHeuristic(Dictionary<string, bool> worldState, Dictionary<string, bool> goalState)
        {
            int h = 0;
            foreach (var goalFact in goalState)
            {
                if (!worldState.TryGetValue(goalFact.Key, out bool value) || value != goalFact.Value)
                {
                    h++;
                }
            }

            return h;
        }

        private bool IsCurrentWorldStateAchieved(Dictionary<string, bool> nodeState, Dictionary<string, bool> currentWorldState)
        {
            foreach (var goalFact in currentWorldState)
            {
                if (!nodeState.TryGetValue(goalFact.Key, out bool value) || value != goalFact.Value)
                {
                    return false;
                }
            }

            return true;
        }

        private Queue<GOAPNode> ReconstructPath(List<GOAPNode> closedList)
        {
            var path = new Queue<GOAPNode>();
            foreach (var node in closedList)
            {
                if (node.action != null)
                {
                    path.Enqueue(node);
                }
            }

            return path;
        }

        private GOAPNode CreateStartNode(GOAPGoal goal)
        {
            GOAPNode startNode = new GOAPNode(null, null, goal.desiredConditions, 0, 0, 0);

            return startNode;
        }

        private bool HasAnyRequiredEffects(GOAPAction action, Dictionary<string, bool> preconditions)
        {
            bool satisfiesAtLeastOne = false;
            foreach (var effect in action.effects)
            {
                if (preconditions.TryGetValue(effect.Key, out bool value))
                {
                    if (value == effect.Value)
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


    

