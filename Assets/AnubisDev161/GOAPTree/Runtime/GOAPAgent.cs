using GOAPGraph;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GOAP
{
    public class GOAPAgent : GOAPGraphObject
    {
        private GOAPBrain goapBrain;

        [field: SerializeField]
        public GOAPGraphAsset graphAsset { get; private set; }
        private Queue<GOAPAction> currentPlan;

        private void Start()
        {
            var graphInstance = Instantiate(graphAsset);
    
            goapBrain = new GOAPBrain(this, graphInstance);
            currentPlan =  goapBrain.CreatePLan();

            if (VerifiyCurrentPlan())
            {
                ExecuteCurrentPlan();
            }
        }


        //public GOAPAgent()
        //{
        //    // STATE NAMES
        //    // ---------------------------
        //    string hasWood = "hasWood";
        //    string hasFire = "hasFire";
        //    string hasSpear = "hasSpear";

        //    var worldState = new Dictionary<string, bool>
        //    {
        //        { hasWood, false },
        //        { hasFire, false },
        //        { hasSpear, true },

        //    };
        //    goapBrain = new GOAPBrain(new WorldState(worldState));
        //}

        //// Goal
        //private void Start()
        //{
        //    var goal = goapBrain.goalSelector.GetBestGoal();
        //    currentPlan = goapBrain.CreatePLan(goal);
        //    ExecutePlan(currentPlan, goapBrain.blackboard);
        //}
        private bool VerifiyCurrentPlan()
        {
            var planSize = currentPlan.Count;
            float totalCost = 0;

            Debug.Log($"Started executing plan with {planSize} actions | goal {goapBrain.goalSelector.currentGoal.name}");

            foreach (var action in currentPlan)
            {
                totalCost += action.GetCost();
            }

            if (currentPlan.Count <= 0)
            {
                Debug.LogError("Plan not valid, plan contains no actions!");
                return false;
            }

            return true;
        }

        private void ExecuteCurrentPlan()
        {
            if (currentPlan.Count > 0)
            {
                var action = currentPlan.Dequeue();

                action.executed += OnActionExecuted;
                action.Execute(goapBrain.currentWorldState);
            }

            Debug.Log("Plan executed");
        }

        private void OnActionExecuted(bool success)
        {
            if (success)
            {
                ExecuteCurrentPlan();
            }

            Debug.LogError("Action executed unsuccessfully, terminate plan");
        }



        //   while (plan.Count > 0) 
        //   {
        //        var action = plan.Dequeue();
        //        action.Execute(blackboard);
        //   }

        //    if (WorldStateCompare.IsWorldStateBAchieved(blackboard.worldFacts, goapBrain.goalSelector.currentGoal.desiredConditions))
        //    {
        //        MessageBus.PrintToUnityLog($"Plan with {planSize} actions executed successfully | cost {totalCost}");
        //        MessageBus.PrintToUnityLog($"New world state is ${blackboard.ToString()}");
        //        MessageBus.PrintToUnityLog($"Desired world state is ${goapBrain.goalSelector.currentGoal.ToString()}");
        //    }
        //    else
        //    {
        //        MessageBus.PrintErrorToUnityLog($"Failed to execute plan with {planSize} actions | cost {totalCost}");
        //        MessageBus.PrintErrorToUnityLog($"New world state is {blackboard.ToString()}");
        //        MessageBus.PrintErrorToUnityLog($"Desired world state is {goapBrain.goalSelector.currentGoal.ToString()}");
        //    }
        //}


    }
}