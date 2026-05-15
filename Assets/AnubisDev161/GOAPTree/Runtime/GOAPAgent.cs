using GOAP.Data;
using GOAPGraph;
using System;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;

using UnityEngine;
using static GOAP.GOAPBlackbaord;

namespace GOAP
{
    public class GOAPAgent : GOAPGraphObject
    {
        public GOAPBrain goapBrain {  get; private set; }
        [field: SerializeField]
        public GOAPGraphAsset graphAsset { get; private set; }
        private Queue<GOAPAction> currentPlan;
        private PlanDebugInfo planDebugInfo;
       
        private void Start()
        {
            if (graphAsset == null)
            {
                Debug.LogError("Agent has no graph asset!");
                return;
            }

            var graphInstance = Instantiate(graphAsset);
        
            goapBrain = new GOAPBrain(this, graphInstance);



            currentPlan =  goapBrain.CreatePLan();


            if (currentPlan != null && VerifiyCurrentPlan())
            {
                ExecuteCurrentPlan();
            }
            else
            {
                Debug.LogError("Agent stopped planning due to an invalid plan");
            }
        }

        private bool VerifiyCurrentPlan()
        {
            var planSize = currentPlan.Count;
            float totalCost = 0;

            Debug.Log($"Started executing plan with {planSize} actions | goal {goapBrain.goalSelector.currentGoal.name}");
            planDebugInfo.planSize = planSize;
           

            foreach (var action in currentPlan)
            {
                totalCost += action.GetCost();
                planDebugInfo.totalCost = totalCost;
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
                action.BeginExecute(goapBrain.currentWorldState, goapBrain.graphInstance);
                return;
            }

            if (WorldStateCompare.IsWorldStateBAchieved(goapBrain.currentWorldState.worldFacts, goapBrain.goalSelector.currentGoal.desiredConditions))
            {
                Debug.Log( "<color=green>" + $"Plan with {planDebugInfo.planSize} + actions executed successfully | cost {planDebugInfo.totalCost}");
                Debug.Log($"New world state is ${goapBrain.currentWorldState.ToString()}");
                Debug.Log($"Desired world state is ${goapBrain.goalSelector.currentGoal.ToString()}");
            }
            else
            {
                Debug.LogError($"Failed to execute plan with {planDebugInfo.planSize} actions | cost {planDebugInfo.totalCost}");
                Debug.LogError($"New world state is {goapBrain.currentWorldState.ToString()}");
                Debug.LogError($"Desired world state is {goapBrain.goalSelector.currentGoal.ToString()}");
                return;
            }
            


            Debug.LogError("Agent stopped planning due to no other exisitng plan found");
        }

        private void OnActionExecuted(bool success, GOAPAction lastAction)
        {
            lastAction.executed -= OnActionExecuted;

            if (success)
            {
                ExecuteCurrentPlan();
            }
            else
            {
                Debug.LogError("Action executed unsuccessfully, execution stopped with last action: " + lastAction);
            }
        }
    }

    public struct PlanDebugInfo
    {
        public int planSize;
        public float totalCost;

    }
}