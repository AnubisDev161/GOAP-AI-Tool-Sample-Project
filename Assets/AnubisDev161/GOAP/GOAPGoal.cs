using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class GOAPGoal
    {
        private float priority;
        public Dictionary<string, bool> desiredConditions {  get; private set; }
        public string name { get; private set; }

        public GOAPGoal(float priority, Dictionary<string, bool> desiredConditions, string goalName = "baseGoal")
        {
            this.priority = priority;
            this.desiredConditions = desiredConditions;
            this.name = goalName;
        }

        public bool IsValid()
        {
            return true;
        }

        public float GetPriority()
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