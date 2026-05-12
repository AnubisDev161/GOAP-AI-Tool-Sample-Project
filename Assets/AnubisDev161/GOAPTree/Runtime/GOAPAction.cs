using GOAPGraph;
using System;
using System.Collections.Generic;
using UnityEngine;
using GOAP.Data;

namespace GOAP
{
    public class GOAPAction
    {
        public string name { get; private set; } = ("Base Action");
        private float cost;
        public Dictionary<string, WorldFact> preconditions { get; private set; }
        public Dictionary<string, WorldFact> effects { get; private set; }

        public Action<bool, GOAPAction> executed;

        private GOAPGraphNode graphNode;

        public GOAPAction(Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, string name = "Base Action", float cost = 1, GOAPGraphNode graphNode = null)
        {
            this.preconditions = preconditions;
            this.name = name;
            this.effects = effects;
            this.cost = cost;
            this.graphNode = graphNode;
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
                if (!worldFacts.TryGetValue(preCon.Key, out value) || value != preCon.Value)
                {
                    return false;
                }
            }

            return true;
        }

        public virtual bool BeginExecute(WorldState worldState, GOAPGraphAsset graphAsset)
        {
            PrintPreconditions();

            if (!CheckIfPrconditionsMet(worldState.worldFacts))
            {
                Debug.LogError("Precondtions not met " + $"could not run action {name}");
                executed?.Invoke(false, this);
                return false;
            }

            graphNode.executeFinished += OnGraphNodeExecuteFinished;
            graphNode.OnExecute(graphAsset, worldState);
            return true;
        }

        private void OnGraphNodeExecuteFinished(GOAPGraphAsset graphAsset, WorldState worldState, bool success)
        {
            FinishExecute(graphAsset, worldState, success);
        }

        protected virtual bool FinishExecute(GOAPGraphAsset graphAsset, WorldState worldState, bool success)
        {
            if (success)
            {
                Debug.Log($"Precondtions met : " + " Action executed successfully" + $" Action name : {name}");
                RemovePreconditionsAndAddEffectsToState(worldState);
            }

            executed?.Invoke(success, this);
            return true;
        }

        // If the plan is being executed, you need to start at the current world state and remove all the preconditions of the action from tbhe current world state
        public void RemovePreconditionsAndAddEffectsToState(WorldState currentWorldState)
        {
            foreach (var precondition in preconditions)
            {
                if (currentWorldState.worldFacts.TryGetValue(precondition.Key, out var value) && value == precondition.Value)
                {
                    currentWorldState.TryRemoveFact(precondition.Key);
                }
            }

            foreach (var effect in effects)
            {
                currentWorldState.worldFacts[effect.Key] = effect.Value;
            }
        }

        // If the plan is being planned, you need to start at the goal world state and remove all the effects of the action from the required world state
        public void RemoveEffectsAndAddPreconditionsToState(WorldState requiredWorldState)
        {
            foreach (var effect in effects)
            {
                // TODO Add other conditions such as equal, any, greater smaller
                if (requiredWorldState.worldFacts.TryGetValue(effect.Key, out WorldFact value) && value == effect.Value)
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
    }
}
