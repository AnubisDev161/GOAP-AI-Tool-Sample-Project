using GOAP.Core;
using GOAP.GOAPGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExampleProject
{
    [NodeInfo("Wait for seconds", "Example / Wait for seconds", hasInputParams: true, hasOutputParams: true)]
    public class Wait : ActionNode
    {
        [ExposedProperty]
        public float seconds;

        [ExposedProperty]
        public string waitDurationKeyName;

        [ExposedProperty]
        public string optionalGameObject;

        [ExposedProperty]
        public bool triggerInteractIfGameObjectIsSmartObject;

        public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, bool success = true)
        {
            currentGraph.agent.StartCoroutine(WaitForSeconds(currentGraph, worldState, success));
        }

        protected IEnumerator WaitForSeconds(GOAPGraphAsset currentGraph, WorldState worldState, bool success)
        {
            var waitTime = 0.0f;

            if (seconds == 0)
            {
                waitTime = (float)currentGraph.Blackboard.GetKeyWithExpectedType(waitDurationKeyName, GOAP.Core.Agent.GOAPBlackbaord.BlackboardKeyType.Float).value;
            }
            else
            {
                waitTime = seconds;
            }

            yield return new WaitForSeconds(waitTime);
            success = true;

            if (triggerInteractIfGameObjectIsSmartObject)
            {
                var key = currentGraph.Blackboard.GetKeyWithExpectedType(optionalGameObject, GOAP.Core.Agent.GOAPBlackbaord.BlackboardKeyType.GameObject);
                if (key.value != null && (key.value as GameObject).TryGetComponent<SmartObject>(out SmartObject smartObject))
                {
                    smartObject.Interact(currentGraph);
                }
            }

            OnWaitingTimeFinished(currentGraph, worldState, success);
        }

        protected virtual void OnWaitingTimeFinished(GOAPGraphAsset currentGraph, WorldState worldState, bool success)
        {
            OnExecuteFinished(currentGraph, worldState, success);
        }
        public override void OnAbandonCurrentPlan(GOAPGraphAsset currentGraph, WorldState worldState)
        {
            currentGraph.agent.StopAllCoroutines();
        }
    }
}
