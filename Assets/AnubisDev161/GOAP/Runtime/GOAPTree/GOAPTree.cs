using System.Collections.Generic;
using UnityEngine;
using GOAP.Core;

namespace GOAP.Tree
{
    public class GOAPTree
    {
        private List<GOAPAction> availableActions;

        // Depending on the size of your plans, this number needs to be adjusted to either decrease or increase the allowed number of iterations per plan.
        private int maxClosedListSize = 300;

        public Queue<GOAPAction> GeneratePlan(WorldState currentWorldState, GOAPGoal goal, List<GOAPAction> availableActions, GOAPGraph.GOAPGraphAsset graphInstance)
        {
            this.availableActions = availableActions;

            var startNode = CreateStartNode(goal);
            var bestPlan = BuildGraph(startNode, currentWorldState, graphInstance);

            if (bestPlan == null)
            {
                Debug.Log(GOAPDebug.AddErrorColorAndSize("Plan is null!"));
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

                if (WorldStateCompare.IsWorldStateBAchieved(currentWorldState, currentNode.requiredWorldState))
                {
                    // valid plan found
                    return ReconstructPath(currentNode);
                }

                closedList.Add(currentNode.requiredWorldState, currentNode);

                if (closedList.Count > maxClosedListSize)
                {
                    Debug.Log(GOAPDebug.AddErrorColorAndSize("Max closed list size reached!"));
                    break;
                }
                
                foreach (var action in availableActions)
                {
                    if (!HasAnyRequiredEffects(action, currentNode.requiredWorldState.worldFacts, currentWorldState) || !action.IsAchvievable(graphInstance))
                    {
                        continue;
                    }
                  
                    // create copy of parent's world state and apply the action's effects
                    var mutatedWorldState = currentNode.requiredWorldState.Copy();
                    action.RemoveEffectsAndAddPreconditionsToState(mutatedWorldState);

                    var tentativeGCost = currentNode.gCost + action.GetCost();
                    var hCost = CalculateHeuristic(mutatedWorldState, goalWorldState);

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

            Debug.Log(GOAPDebug.AddErrorColorAndSize("No valid plan found!"));
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

        private bool HasAnyRequiredEffects(GOAPAction action, Dictionary<string, WorldFact> preconditions, WorldState currentWorldState)
        {
            bool satisfiesAtLeastOne = false;
            foreach (var effect in action.effects)
            {
                if (preconditions.TryGetValue(effect.Key, out WorldFact value))
                {
                    if (IsRequiredValue(value, effect.Value, currentWorldState))
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

        internal bool IsRequiredValue(WorldFact precondition, WorldFact effect, WorldState currentWorldState)
        {
            if (precondition.acceptedValue == AcceptedValue.Equals)
            {
                return precondition == effect;
            }
            else if (precondition.acceptedValue == AcceptedValue.Greater)
            {
               return IsValueGreater(precondition, effect, currentWorldState);
            }
            else
            {
               return IsValueSamller(precondition, effect, currentWorldState);
            }
        }

        private bool IsValueGreater(WorldFact precondition, WorldFact effect, WorldState currentWorldState)
        {
            if (effect.operationType == OperationType.Add)
            {
                currentWorldState.worldFacts.TryGetValue(effect.name, out var worldFact);

                if (worldFact.name != null && worldFact.value != string.Empty)
                {
                    switch (precondition.valueType)
                    {
                        case WorldFactType.Int:
                            return (int)worldFact.GetValue() + (int)effect.GetValue() > (int)precondition.GetValue();
                        case WorldFactType.Float:
                            return (float)worldFact.GetValue() + (float)effect.GetValue() > (float)precondition.GetValue();
                    }
                }
            }

            return effect > precondition;
        }

        private bool IsValueSamller(WorldFact precondition, WorldFact effect, WorldState currentWorldState)
        {
            if (effect.operationType == OperationType.Add)
            {
                currentWorldState.worldFacts.TryGetValue(effect.name, out var worldFact);

                if (worldFact.name != null && worldFact.value != string.Empty)
                {
                    switch (precondition.valueType)
                    {
                        case WorldFactType.Int:
                            return (int)worldFact.GetValue() + (int)effect.GetValue() < (int)precondition.GetValue();
                        case WorldFactType.Float:
                            return (float)worldFact.GetValue() + (float)effect.GetValue() < (float)precondition.GetValue();
                    }
                }
            }

            return effect < precondition;
        }
    }
}
