using GOAP.GOAPGraph;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GOAP.Core.Agent
{
    public class GOAPAgent : MonoBehaviour
    {
        public GOAPNavigation navigation { get; private set; }

        public Action<GOAPGraphAsset, WorldState> abandonCurrentPlan;
        public GOAPBrain goapBrain {  get; private set; }
        [field: SerializeField]
        public GOAPGraphAsset graphAsset { get; private set; }
        private Queue<GOAPAction> currentPlan;
        private PlanDebugInfo planDebugInfo;

        [SerializeField]
        private float planningInterval = 5;

        /// <summary>
        /// Init is called in the Start method to ensure that the GOAPNavigation Component has already been loaded
        /// </summary>
        private void Start()
        {
            Init();
        }

        protected virtual void Init()
        {
            InitNavigation();
            InitBrain();
            StartNewActionPlan();
        }

        private void InitBrain()
        {
            if (graphAsset == null)
            {
                Debug.LogError("Agent has no graph asset!");
                return;
            }

            var graphInstance = Instantiate(graphAsset);
            goapBrain = new GOAPBrain(this, graphInstance);
        }

        private void InitNavigation()
        {
            var navigation = GetComponent<GOAPNavigation>();

            if (navigation == null)
            {
                Debug.LogError("No GOAPNaviagtion component found!");
                return;
            }

            this.navigation = navigation;
        }

        private bool VerifiyCurrentPlan()
        {
            var planSize = currentPlan.Count;
            float totalCost = 0;

            Debug.Log($"<color=lightBlue>Started executing plan with {planSize} actions | goal {goapBrain.goalSelector.currentGoal.name} | {this}");
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

        public virtual void OnCurrentWorldStateChangedByTrigger()
        {
            AbandonCurrentPlan();
            StartNewActionPlan();
        }

        protected virtual void AbandonCurrentPlan()
        {
            StopAllCoroutines();
            currentPlan.Clear();
            abandonCurrentPlan?.Invoke(goapBrain.graphInstance, goapBrain.currentWorldState);
            Debug.Log($"<color=orange> Abandoned current plan with goal: {goapBrain.goalSelector.currentGoal}, current world state is: {goapBrain.currentWorldState}");
        }

        protected void StartNewActionPlan()
        {
            currentPlan = goapBrain.CreatePLan();

            if (currentPlan != null && VerifiyCurrentPlan())
            {
                ExecuteCurrentPlan();
            }
            else
            {
                Debug.LogError("Agent stopped planning due to an invalid plan");
            }
        }
        
        private void ExecuteCurrentPlan()
        {
            if (currentPlan.Count > 0)
            {
                var action = currentPlan.Dequeue();
                abandonCurrentPlan += action.OnAbandonCurrentPlan;

                action.executed += OnActionExecuted;

                action.BeginExecute(goapBrain.currentWorldState, goapBrain.graphInstance);
                return;
            }

            if (WorldStateCompare.IsWorldStateBAchieved(goapBrain.currentWorldState.worldFacts, goapBrain.goalSelector.currentGoal.desiredConditions))
            {
                Debug.Log( "<color=green>" + $"Plan with {planDebugInfo.planSize} + actions executed successfully | cost {planDebugInfo.totalCost} | goal {goapBrain.goalSelector.currentGoal} | {this}");
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
            
            StartCoroutine(CreateNewPlanAfterDelay(planningInterval));
        }

        private void OnActionExecuted(bool success, GOAPAction lastAction)
        {
            lastAction.executed -= OnActionExecuted;
            abandonCurrentPlan -= lastAction.OnAbandonCurrentPlan;

            if (success)
            {
                ExecuteCurrentPlan();
            }
            else
            {
                Debug.LogError($"Action executed unsuccessfully, execution stopped with last action: {lastAction} | {this}");
                AbandonCurrentPlan();
                StartCoroutine(CreateNewPlanAfterDelay(planningInterval));
            }
        }

        private IEnumerator CreateNewPlanAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            StartNewActionPlan();
        }

        public override string ToString()
        {
            return $"agent: {name}";
        }
    }

    public struct PlanDebugInfo
    {
        public int planSize;
        public float totalCost;
    }
}