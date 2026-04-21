using System;
using System.Collections;
using System.Collections.Generic;

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

            if (bestPlan.Count == 0)
            {
                Console.WriteLine("No valid PLan found!");
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
                GOAPNode previousAction = null;

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
                    var hCost = CalculateHeuristic(mutatedWorldState, goalWorldState);
                    var fCost = tentativeGCost + hCost;


                    var nodeToAdd = new GOAPNode(action, parentNode, mutatedWorldState, fCost, tentativeGCost, hCost);


                 //   if (previousAction != null && previousAction.gCost < nodeToAdd.gCost) continue;
                    
                    previousAction = nodeToAdd;


                    openQueue.Push(nodeToAdd);
                    
                }
            }

            MessageBus.print("No valid plan found!");
            return null;
        }

        private int CalculateHeuristic(WorldState worldState, WorldState goalState)
        {
            int h = 0;
            foreach (var goalFact in goalState.worldFacts)
            {
                if (!worldState.worldFacts.TryGetValue(goalFact.Key, out bool value) || value != goalFact.Value)
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
