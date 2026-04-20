using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace GOAP
{
    public class GOAPAgent : MonoBehaviour
    {
        private GOAPBrain goapBrain;
        private Queue<GOAPAction> currentPlan;
        public GOAPAgent()
        {
            string hasKettle = "hasKettle";
            string hasWater = "hasWater";
            string hasTea = "hasTea";
            string KettleBoiled = "KettleBoiled";

            Dictionary<string, bool> currentWorldState = new Dictionary<string, bool>();

            currentWorldState.Add(hasKettle, true);
            currentWorldState.Add(hasWater, false);
            currentWorldState.Add(hasTea, false);
            currentWorldState.Add(KettleBoiled, false);

            var blackboard = new GOAPBlackboard(currentWorldState);

            goapBrain = new GOAPBrain(blackboard);

        }

        // Goal
        private void Start()
        {
            string hasTea = "hasTea";
            Dictionary<string, bool> desiredWorldState = new Dictionary<string, bool>();
            desiredWorldState.Add(hasTea, true);


            var goal = new GOAPGoal(100, desiredWorldState);

            currentPlan = goapBrain.CreatePLan(goal);
            ExecutePlan(currentPlan, goapBrain.blackboard);
        }
        private void ExecutePlan(Queue<GOAPAction> plan, GOAPBlackboard blackboard)
        {
            var planSize = plan.Count;
            float totalCost = 0;
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

            MessageBus.PrintToUnityLog($"Plan with {planSize} actions executed successfully | cost {totalCost}");
        }
    }
}