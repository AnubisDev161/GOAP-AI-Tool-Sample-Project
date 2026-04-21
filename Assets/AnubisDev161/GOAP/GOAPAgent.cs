using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class GOAPAgent : MonoBehaviour
    {
        private GOAPBrain goapBrain;
        private Queue<GOAPAction> currentPlan;
        public GOAPAgent()
        {
            // STATE NAMES
            // ---------------------------
            string hasWood = "hasWood";
            string hasFire = "hasFire";



            var worldState = new Dictionary<string, bool>
            {
                { hasWood, false },
                { hasFire, false },
            
            };
            goapBrain = new GOAPBrain(new WorldState(worldState));
        }

        // Goal
        private void Start()
        {
            var goal = goapBrain.goalSelector.GetBestGoal();
            currentPlan = goapBrain.CreatePLan(goal);
            ExecutePlan(currentPlan, goapBrain.blackboard);
        }
        private void ExecutePlan(Queue<GOAPAction> plan, WorldState blackboard)
        {
            var planSize = plan.Count;
            float totalCost = 0;

            MessageBus.PrintToUnityLog($"Started executing plan with {planSize} actions | cost {totalCost} | goal {goapBrain.goalSelector.currentGoal.name}");

            foreach (var action in plan)
            {
                totalCost += action.GetCost();
            }

            if (plan.Count <= 0)
            {
                MessageBus.PrintErrorToUnityLog("Plan not valid, plan contains no actions!");
                return;
            }

           while (plan.Count > 0) 
           {
                var action = plan.Dequeue();
                action.Execute(blackboard);
           }

            if (WorldStateCompare.IsWorldStateBAchieved(blackboard.worldFacts, goapBrain.goalSelector.currentGoal.desiredConditions))
            {
                MessageBus.PrintToUnityLog($"Plan with {planSize} actions executed successfully | cost {totalCost}");
            }
            else
            {
                MessageBus.PrintErrorToUnityLog($"Failed to execute plan with {planSize} actions | cost {totalCost}");
            }
        }
    }
}