using GOAPGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP.Data;
using System;
using UnityEditor;

namespace GOAPGraph
{
    [NodeInfo("Move To", "AI / Move To", hasInputParams: true, hasOutputParams: true)]
    public class MoveTo : ActionNode
    {
        [ExposedProperty]
        public Destination destination;
        public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, bool success = true)
        {
            var targetDestination = destination.position;

            if (targetDestination == Vector3.zero)
            {
                var targetPosKey = currentGraph.Blackboard.GetKey("TargetPos");
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

            currentGraph.goapGraphObject.navigation.desinationReached += OnDestinationReached;
            currentGraph.goapGraphObject.navigation.SetDestination(targetDestination);
        }

        private void OnDestinationReached(GOAPGraphAsset currentGraph, WorldState worldState)
        {
            Debug.Log("Move to node executed");
            currentGraph.goapGraphObject.navigation.desinationReached -= OnDestinationReached;
            base.OnExecuteFinished(currentGraph, worldState, true);
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