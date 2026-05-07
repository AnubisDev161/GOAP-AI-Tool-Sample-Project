using GOAPGraph;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class GOAPAction
    {
        public string name { get; private set; } = ("Base Action");
        private float cost;
        public Dictionary<string, bool> preconditions { get; private set; }
        public Dictionary<string, bool> effects { get; private set; }

        public Action<bool, GOAPAction> executed;

        public GOAPAction(Dictionary<string, bool> preconditions = null, Dictionary<string, bool> effects = null, string name = "Base Action", float cost = 1)
        {
            this.preconditions = preconditions;
            this.name = name;
            this.effects = effects;
            this.cost = cost;
        }
        public void PrintPreconditions()
        {
            foreach (var preCon in preconditions)
            {
                Debug.Log($"Action {name} is evaluating preconditions " + "| Precondition: Name " + preCon.Key.ToString() + " - Value " + preCon.Value + " | ");
            }
        }
        public bool CheckIfPrconditionsMet(Dictionary<string, bool> worldFacts)
        {
            foreach (var preCon in preconditions)
            {
                bool value;
                if (!worldFacts.TryGetValue(preCon.Key, out value) || value != preCon.Value)
                {
                    return false;
                }
            }

            return true;
        }

        public bool Execute(WorldState blackboard)
        {
            PrintPreconditions();

            if (!CheckIfPrconditionsMet(blackboard.worldFacts))
            {
                Debug.LogError("Precondtions not met " + $"could not run action {name}");
                executed?.Invoke(false, this);
                return false;
            }

            Debug.Log($"Precondtions met : " + " Action executed successfully" + $" Action name : {name}");
            RemovePreconditionsAndAddEffectsToState(blackboard);

            executed?.Invoke(true, this);
            return true;
        }

        // If the plan is being executed, you need to start at the current world state and remove all the preconditions of the action from tbhe current world state
        public void RemovePreconditionsAndAddEffectsToState(WorldState currentWorldState)
        {
            foreach (var precondition in preconditions)
            {
                if (currentWorldState.worldFacts.TryGetValue(precondition.Key, out bool value) && value == precondition.Value)
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
                if (requiredWorldState.worldFacts.TryGetValue(effect.Key, out bool value) && value == effect.Value)
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
            
            return "[Name: " + name + " ]";
        }
    }
}
