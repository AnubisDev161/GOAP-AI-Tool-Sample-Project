using GOAP.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP.GOAPGraph
{
    [NodeInfo("Generate random destination", "Generate random / Destination", hasInputParams: true, hasOutputParams: true)]
    public class GenerateRandomDestination : ActionNode
    {
        [ExposedProperty]
        public float range;

        [ExposedProperty]
        public string targetPosKeyName;

        public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, bool success = true)
        {
            var blackbaordKey = currentGraph.Blackboard.GetKeyWithExpectedType(targetPosKeyName, Core.Agent.GOAPBlackbaord.BlackboardKeyType.Vector3);

            if (blackbaordKey == null)
            {
                base.OnExecuteFinished(currentGraph, worldState, false);
                return;
            }

            var randomPointInsideUnitSphere = Random.insideUnitSphere;
            var randomPos = (randomPointInsideUnitSphere * range) + currentGraph.agent.gameObject.transform.position;

            NavMeshHit hit;

            NavMesh.SamplePosition(randomPos, out hit, range, 1);

            Vector3 finalPos = hit.position;
            blackbaordKey.value = finalPos; 

            base.OnExecute(currentGraph, worldState);
        }
    }
}
