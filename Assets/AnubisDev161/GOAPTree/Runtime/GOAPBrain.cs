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


        public Queue<GOAPAction> CreatePLan()
        {
            availableActions = FetchActions();
            availableGoals = FetchGoals();  

            var bestGoal = goalSelector.GetBestGoal(currentWorldState, availableGoals);
            return planner.GeneratePlan(currentWorldState, bestGoal, availableActions);
        }

        private List<GOAPGoal> FetchGoals()
        {
            List<GOAPGoal> goals = new List<GOAPGoal>();
            var goalNodes = graphInstance.GetGoalNodes();
            foreach (var goalNode in goalNodes)
            {
                var preconditions = GetPrecondiotionsFromBlackboardNodes(goalNode);

                GOAPGoal goal = new GOAPGoal(1, preconditions, goalNode.name);
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
                var effects = GetEffectsFromBlackboardNodes(actionNode);
                var preconditions = GetPrecondiotionsFromBlackboardNodes(actionNode);

                GOAPAction action = new GOAPAction(preconditions, effects, actionNode.name);
                actions.Add(action);
            }

            return actions;
        }

        private Dictionary<string, bool> GetEffectsFromBlackboardNodes(ActionNode actionNode)
        {
            var effects = new Dictionary<string, bool>();

            var effectNodes = actionNode.GetEffectNodes(graphInstance);
            foreach (var effectNode in effectNodes)
            {
                var effect = effectNode.GetData();

                effects.Add(effect.name, Convert.ToBoolean(effect.value));
            }

            return effects;
        }

        private Dictionary<string, bool> GetPrecondiotionsFromBlackboardNodes(ActionNode actionNode)
        {
            var preconditions = new Dictionary<string, bool>();

            var preconditionNodes = actionNode.GetPreconditionNodes(graphInstance);
            foreach (var preconditionNode in preconditionNodes)
            {
                var precondition = preconditionNode.GetData();

                preconditions.Add(precondition.name, Convert.ToBoolean(precondition.value));
            }

            return preconditions;
        }

        private Dictionary<string, bool> GetPrecondiotionsFromBlackboardNodes(GoalWorldStateNode actionNode)
        {
            var preconditions = new Dictionary<string, bool>();

            var preconditionNodes = actionNode.GetPreconditionNodes(graphInstance);
            foreach (var preconditionNode in preconditionNodes)
            {
                var precondition = preconditionNode.GetData();

                preconditions.Add(precondition.name, Convert.ToBoolean(precondition.value));
            }

            return preconditions;
        }

    }
}