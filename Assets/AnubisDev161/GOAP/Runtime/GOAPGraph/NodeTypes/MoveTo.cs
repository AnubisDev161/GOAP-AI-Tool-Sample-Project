using System.Collections.Generic;
using UnityEngine;
using GOAP.Core;
using System;

namespace GOAP.GOAPGraph
{
    [NodeInfo("Move To", "AI / Move To", hasInputParams: true, hasOutputParams: true)]
    public class MoveTo : ActionNode
    {
        [ExposedProperty]
        public Destination destination;

        [ExposedProperty]
        public string targetPosKeyName;

        public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, bool success = true)
        {
            var targetDestination = destination.position;

            if (targetDestination == Vector3.zero)
            {
                var targetPosKey = currentGraph.Blackboard.GetKey(targetPosKeyName);
                if (targetPosKey != null && targetPosKey.value != null)
                {
                    targetDestination = (Vector3)targetPosKey.value;
                }
            } 

            if (targetDestination == Vector3.zero)
            {
                Debug.LogError("Given vector is null or vector.zero!");
                success = false;
                base.OnExecuteFinished(currentGraph, worldState, success);
                return;
            }

            currentGraph.agent.navigation.desinationReached += OnDestinationReached;
            currentGraph.agent.navigation.SetDestination(targetDestination);
        }

        private void OnDestinationReached(GOAPGraphAsset currentGraph, WorldState worldState)
        {
            Debug.Log("Move to node executed");
            currentGraph.agent.navigation.desinationReached -= OnDestinationReached;
            base.OnExecuteFinished(currentGraph, worldState, true);
        }

        public override void OnAbandonCurrentPlan(GOAPGraphAsset currentGraph, WorldState worldState)
        {
            base.OnAbandonCurrentPlan(currentGraph, worldState);
            currentGraph.agent.navigation.SetDestination(currentGraph.agent.transform.position);
            Debug.Log("Move to node abandoned");
            currentGraph.agent.navigation.desinationReached -= OnDestinationReached;
            base.OnExecuteFinished(currentGraph, worldState, false);
        }
    }

    // Create a wrapper class you can draw with a custom property drawer
    [Serializable]
    public class Destination
    {
        [SerializeField]
        public Vector3 position;

        [SerializeField]
        public int sceneID;
    }
}