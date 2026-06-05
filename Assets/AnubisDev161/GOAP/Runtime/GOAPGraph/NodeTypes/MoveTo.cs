using System.Collections.Generic;
using UnityEngine;
using GOAP.Core;
using System;
using GOAP.Core.Agent;

namespace GOAP.GOAPGraph
{
    [NodeInfo("Move To", "AI / Move To", hasInputParams: true, hasOutputParams: true)]
    public class MoveTo : ActionNode
    {
        [ExposedProperty]
        public Vector3 targetDestination;

        [ExposedProperty]
        public string targetPosKeyName;

        public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, bool success = true)
        {
            success = false;
            var targetDestination = this.targetDestination;

            if (targetDestination == Vector3.zero)
            {
                targetDestination = GetVectorFromBlackboardKey(currentGraph);
            } 

            if (targetDestination == Vector3.zero)
            {
                Debug.LogError("Given vector is vector.zero!");
                base.OnExecuteFinished(currentGraph, worldState, success);
                return;
            }

            if (!currentGraph.agent.navigation.SetDestination(targetDestination))
            {
                base.OnExecuteFinished(currentGraph, worldState, success);
                return;
            }

            currentGraph.agent.navigation.desinationReached += OnDestinationReached;
        }

        private void OnDestinationReached(GOAPGraphAsset currentGraph, WorldState worldState)
        {
            Debug.Log("Move to node executed");
            currentGraph.agent.navigation.desinationReached -= OnDestinationReached;
            base.OnExecuteFinished(currentGraph, worldState, true);
        }

        public override void OnAbandonCurrentPlan(GOAPGraphAsset currentGraph, WorldState worldState)
        {
            currentGraph.agent.navigation.desinationReached -= OnDestinationReached;
            currentGraph.agent.navigation.SetDestination(currentGraph.agent.transform.position);
            base.OnAbandonCurrentPlan(currentGraph, worldState);
        }

        private Vector3 GetVectorFromBlackboardKey(GOAPGraphAsset currentGraph)
        {
            var targetPosKey = currentGraph.Blackboard.GetKeyWithExpectedType(targetPosKeyName, GOAPBlackbaord.BlackboardKeyType.Vector3);

            if (targetPosKey != null && targetPosKey.value != null)
            {
                return (Vector3)targetPosKey.value;
            }

            targetPosKey = currentGraph.Blackboard.GetKeyWithExpectedType(targetPosKeyName, GOAPBlackbaord.BlackboardKeyType.GameObject);

            if (targetPosKey != null && targetPosKey.value != null)
            {
                return ((GameObject)targetPosKey.value).transform.position;
            }

            return Vector3.zero;
        }
    }
}