using GOAP.GOAPGraph;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP.Core
{
    public class GOAPAction
    {
        public string name { get; private set; } = ("Base Action");
        private float cost;
        public Dictionary<string, WorldFact> preconditions { get; private set; }
        public Dictionary<string, WorldFact> effects { get; private set; }

        public Action<bool, GOAPAction> executed;

        private ActionNode actionGraphNode;

        public GOAPAction(Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, string name = "Base Action", float cost = 1, ActionNode graphNode = null)
        {
            this.preconditions = preconditions;
            this.name = name;
            this.effects = effects;
            this.cost = cost;
            this.actionGraphNode = graphNode;
        }

        public void PrintPreconditions()
        {
            foreach (var preCon in preconditions)
            {
                Debug.Log($"Action {name} is evaluating preconditions " + "| Precondition: Name " + preCon.Key.ToString() + " - Value " + preCon.Value + " | ");
            }
        }

        public bool CheckIfPrconditionsMet(Dictionary<string, WorldFact> worldFacts)
        {
            foreach (var preCon in preconditions)
            {
                WorldFact value;

                if (!worldFacts.TryGetValue(preCon.Key, out value))
                {
                    return false;
                }

                if (!preCon.Value.IsRequiredValue(value)) return false;
            }

            return true;
        }

        public virtual bool BeginExecute(WorldState worldState, GOAPGraphAsset graphAsset)
        {
            PrintPreconditions();

            if (!CheckIfPrconditionsMet(worldState.worldFacts))
            {
                Debug.LogError("Precondtions not met " + $"could not run action: {name}");
                executed?.Invoke(false, this);
                return false;
            }

            actionGraphNode.executeFinished += OnGraphNodeExecuteFinished;
            actionGraphNode.OnExecute(graphAsset, worldState, preconditions, effects);
            return true;
        }

        private void OnGraphNodeExecuteFinished(GOAPGraphAsset graphAsset, WorldState worldState, bool success)
        {
            FinishExecute(graphAsset, worldState, success);
        }

        protected virtual bool FinishExecute(GOAPGraphAsset graphAsset, WorldState worldState, bool success)
        {
            RemovePreconditionsFromWorldState(worldState);

            if (success)
            {
                Debug.Log($"Precondtions met : " + " Action executed successfully" + $" Action name : {name} | Action cost: {cost}");
                AndAddEffectsToState(worldState);
            }

            actionGraphNode.executeFinished -= OnGraphNodeExecuteFinished;
            executed?.Invoke(success, this);
            return true;
        }

        // If the plan is being executed, you need to start at the current world state
        public void AndAddEffectsToState(WorldState currentWorldState)
        {
            foreach (var effect in effects)
            {
                effect.Value.ChangeWorldFactAccordingToOperationType(currentWorldState.worldFacts);
            }
        }

        public void RemovePreconditionsFromWorldState(WorldState currentWorldState)
        {
            // remove all the preconditions of the action from tbhe current world state if removePreconditions is true
            if (actionGraphNode.RemovePreconditionsFromWorldState)
            {
                foreach (var precondition in preconditions)
                {
                    if (currentWorldState.worldFacts.TryGetValue(precondition.Key, out var value) && value.IsRequiredValue(precondition.Value))
                    {
                        currentWorldState.TryRemoveFact(precondition.Key);
                    }
                }
            }
        }

        // If the plan is being planned, you need to start at the goal world state and remove all the effects of the action from the required world state
        public void RemoveEffectsAndAddPreconditionsToState(WorldState requiredWorldState)
        {
            foreach (var effect in effects)
            {
                if (requiredWorldState.worldFacts.TryGetValue(effect.Key, out WorldFact value) && value.IsRequiredValue(effect.Value))
                {
                    requiredWorldState.TryRemoveFact(effect.Key);
                }
            }

            foreach (var precondition in preconditions)
            {
                requiredWorldState.worldFacts[precondition.Key] = precondition.Value;
            }
        }

        public float GetCost()
        {
            return cost;
        }

        public override string ToString()
        {
            
            return "[Name: " + name + " cost: " + cost + " ]";
        }

        public void OnAbandonCurrentPlan(GOAPGraphAsset currentGraph, WorldState worldState)
        {
            actionGraphNode.OnAbandonCurrentPlan(currentGraph, worldState);
        }

        public bool IsAchvievable(GOAPGraphAsset currentGraph)
        {
           // if (!CheckIfPrconditionsMet(currentGraph.agent.goapBrain.currentWorldState.worldFacts)) return false;
            return actionGraphNode.IsAchvievable(currentGraph);
        }
    }
}
