using GOAPGraph;
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
        //private void ExecutePlan(Queue<GOAPAction> plan, WorldState blackboard)
        //{
        //    var planSize = plan.Count;
        //    float totalCost = 0;

        //    MessageBus.PrintToUnityLog($"Started executing plan with {planSize} actions | goal {goapBrain.goalSelector.currentGoal.name}");

        //    foreach (var action in plan)
        //    {
        //        totalCost += action.GetCost();
        //    }

        //    if (plan.Count <= 0)
        //    {
        //        MessageBus.PrintErrorToUnityLog("Plan not valid, plan contains no actions!");
        //        return;
        //    }



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