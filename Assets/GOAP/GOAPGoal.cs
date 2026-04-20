using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class GOAPGoal
    {
        private float priority;
        public Dictionary<string, bool> desiredConditions {  get; private set; }

        public GOAPGoal(float priority, Dictionary<string, bool> desiredConditions)
        {
            this.priority = priority;
            this.desiredConditions = desiredConditions;
        }

        public bool IsValid()
        {
            return true;
        }

        public float GetPriority()
        {
            return priority;
        }
    }
}