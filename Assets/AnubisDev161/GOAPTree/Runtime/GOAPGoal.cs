using GOAP.Data;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class GOAPGoal
    {
        private float priority;
        public Dictionary<string, WorldFact> desiredConditions {  get; private set; }
        public string name { get; private set; }

        public GOAPGoal(float priority, Dictionary<string, WorldFact> desiredConditions, string goalName = "baseGoal")
        {
            this.priority = priority;
            this.desiredConditions = desiredConditions;
            this.name = goalName;
        }

        public virtual bool IsValid()
        {
            return true;
        }

        public virtual float GetPriority()
        {
            return priority;
        }

        public override string ToString()
        {
            string allFacts = "";

            foreach (var state in desiredConditions)
            {
                allFacts += state.ToString() + " | ";
            }

            return allFacts;
        }

    }
}