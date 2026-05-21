using GOAP.Data;
using GOAPGraph;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class GOAPBrain
    {
        public WorldState currentWorldState { get; private set; }
        private GOAPPlanner planner;
        public GOAPGoalSelector goalSelector;

        private List<GOAPAction> availableActions;
        private List<GOAPGoal> availableGoals;
        public GOAPAgent agent {  get; private set; }

        public GOAPGraphAsset graphInstance { get; private set; }

        public GOAPBrain(GOAPAgent agent, GOAPGraphAsset graphInstance)
        {
            graphInstance.Initialize(agent);
            this.agent = agent;
            this.graphInstance = graphInstance;
            currentWorldState = new WorldState();
            planner = new GOAPPlanner();
            goalSelector = new GOAPGoalSelector(currentWorldState);
        }

        private void OnCurrentWorldStateChangedByTrigger()
        {
            agent.OnCurrentWorldStateChangedByTrigger();
        }

        public Queue<GOAPAction> CreatePLan()
        {
            if (currentWorldState.worldFacts.Count == 0)
            {
                currentWorldState.worldStateChangedByTrigger -= OnCurrentWorldStateChangedByTrigger;
                currentWorldState = FetchStartState();
                currentWorldState.worldStateChangedByTrigger += OnCurrentWorldStateChangedByTrigger;
            }

            availableActions = FetchActions();
            availableGoals = FetchGoals();

            return CreateValidPlan();
        }

        private Queue<GOAPAction> CreateValidPlan()
        {
            var bestGoal = goalSelector.GetBestGoal(currentWorldState, availableGoals);

            if (bestGoal == null)
            {
                Debug.LogError("No achievable goal found, could not create plan");
                return null;
            }

            var newPlan = planner.GeneratePlan(currentWorldState, bestGoal, availableActions);

            if (availableGoals != null && availableGoals.Count > 0)
            {
                if (newPlan == null || newPlan.Count == 0)
                {
                    Debug.LogError($"Plan not valid, removing current {bestGoal} goal and choosing a different one");
                    availableGoals.Remove(bestGoal);
                    return CreateValidPlan();
                }
            }
            else
            {
                Debug.LogError($"Could not find any valid goal");
            }

            return newPlan;
        }

        private WorldState FetchStartState()
        {
            var startNode = graphInstance.GetStartNode();
            var effects = GetNodeEffects(startNode);
            var startState = new WorldState(effects);

            // TODO add connection to blackboard world facts, only world facts declared in the blackboard are valid to use
            return startState;
        }

        private List<GOAPGoal> FetchGoals()
        {
            List<GOAPGoal> goals = new List<GOAPGoal>();
            var goalNodes = graphInstance.GetGoalNodes();
            foreach (var goalNode in goalNodes)
            {
                var preconditions = GetNodePreconditions(goalNode);

                GOAPGoal goal = new GOAPGoal(goalNode.priority, preconditions, goalNode.name);
                goals.Add(goal);
            }

            return goals;
        }

        private List<GOAPAction> FetchActions()
        {
            List<GOAPAction> actions = new List<GOAPAction>();
            var actionNodes = graphInstance.GetActionNodes();

            // retrieve the data from the GOAP graph nodes to create actual actions
            foreach (var actionNode in actionNodes)
            {
                var effects = GetNodeEffects(actionNode);
                var preconditions = GetNodePreconditions(actionNode);

                GOAPAction action = new GOAPAction(preconditions, effects, actionNode.name, actionNode.cost, actionNode);
                actions.Add(action);
            }

            return actions;
        }

        private Dictionary<string, WorldFact> GetNodeEffects(GOAPGraphNode graphNode)
        {
            var effects = new Dictionary<string, WorldFact>();
            var effectNodes = graphNode.GetEffectNodes(graphInstance);


            // TODO Implement type conversion, currently only bool values are accepted!
            foreach (var effectNode in effectNodes)
            {
                var effect = effectNode.GetData();

                effect.operationType = (effectNode as Effect).operationType;

                if (!ValidateWorldFact(effect)) continue;

                effects.Add(effect.name, effect);
            }

            return effects;
        }

        private bool ValidateWorldFact(WorldFact worldFact)
        {
            if (graphInstance.Blackboard.Contains(worldFact.name))
            {
                var key = graphInstance.Blackboard.GetKey(worldFact.name);

                if (key.worldFactType == worldFact.valueType)
                {
                    return true;
                }
                else
                {
                    Debug.LogError($"World Fact with name: {worldFact.name}, is used as precondition or effect but has a different valueType [{worldFact.valueType}] than the declared world fact in the world state blackboard [{key.worldFactType}]. ");
                    return false;
                }
            }

            Debug.LogError($"World Fact with name: {worldFact.name}, is used as precondition or effect but is not declared in world state blackboard");
            return false;
        }

        private Dictionary<string, WorldFact> GetNodePreconditions(GOAPGraphNode graphNode)
        {
            var preconditions = new Dictionary<string, WorldFact>();

            var preconditionNodes = graphNode.GetPreconditionNodes(graphInstance);
            foreach (var preconditionNode in preconditionNodes)
            {
                var precondition = preconditionNode.GetData();

                precondition.acceptedValue = (preconditionNode as Precondition).acceptedValue;

                if (!ValidateWorldFact(precondition)) continue;

                preconditions.Add(precondition.name, precondition);
            }

            return preconditions;
        }
    }
}