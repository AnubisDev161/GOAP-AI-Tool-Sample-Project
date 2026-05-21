using GOAP;
using GOAP.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static GOAP.GOAPBlackbaord;

namespace GOAPGraph
{
    [NodeInfo("Generate random destination", "Generate random / Destination", hasInputParams: true, hasOutputParams: true)]
    public class GenerateRandomDestination : ActionNode
    {
        [ExposedProperty]
        public float range;
        public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, bool success = true)
        {
            var blackbaordKey = currentGraph.Blackboard.GetKey("TargetPos");

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
