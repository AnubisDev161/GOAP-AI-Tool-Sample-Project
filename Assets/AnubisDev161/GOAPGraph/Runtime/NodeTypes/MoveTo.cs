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
            var targetDestination = currentGraph.blackboard.GetKey("TargetPos");

            // TODO Add logic to get a destination from a blackboard 
            if (destination == null)
            {
                Debug.LogError("Given vector world fact is vector.zero!");
                success = false;
                base.OnExecuteFinished(currentGraph, worldState, success);
                return;
            }

            currentGraph.goapGraphObject.navigation.desinationReached += OnDestinationReached;
            currentGraph.goapGraphObject.navigation.SetDestination((Vector3)targetDestination.key);
        }

        private Vector3 GetTargetDestinationFromPreconditions(Dictionary<string, WorldFact> preconditions)
        {
            if (preconditions != null)
            {
                foreach (var precon in preconditions)
                {
                    if (precon.Value.valueType == GOAP.Data.ValueType.Vector3)
                    {
                        return WorldFact.ConvertValueToVector3(precon.Value.value);
                    }
                }
            }

            Debug.LogError("No vector3 found in given worldFacts, couldn't perform move to action!");
            return Vector3.zero;
        }

        private void OnDestinationReached(GOAPGraphAsset currentGraph, WorldState worldState)
        {
            Debug.Log("Move to node executed");
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