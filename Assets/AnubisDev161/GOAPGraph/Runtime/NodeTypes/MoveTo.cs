using GOAPGraph;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP.Data;

namespace GOAPGraph
{
    [NodeInfo("Move To", "AI / Move To", hasInputParams: true, hasOutputParams: true)]
    public class MoveTo : ActionNode
    {
        [ExposedProperty]
        public Vector3 destination;

        public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, bool success)
        {
            var targetDestination = destination;

            if (destination == Vector3.zero)
            {
                targetDestination = GetTargetDestinationFromWorldFacts(worldState.worldFacts);
            }

            if (targetDestination == Vector3.zero)
            {
                success = false;
                base.OnExecuteFinished(currentGraph, worldState, success);
                return;
            }

            currentGraph.goapGraphObject.navigation.desinationReached += OnDestinationReached;
            currentGraph.goapGraphObject.navigation.SetDestination(targetDestination);
        }

        private Vector3 GetTargetDestinationFromWorldFacts(Dictionary<string, WorldFact> worldFacts)
        {
            foreach (var worldFact in worldFacts)
            {
                if (worldFact.Value.valueType == GOAP.Data.ValueType.Vector3)
                {
                    return WorldFact.ConvertValueToVector3(worldFact.Value.value);
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
}