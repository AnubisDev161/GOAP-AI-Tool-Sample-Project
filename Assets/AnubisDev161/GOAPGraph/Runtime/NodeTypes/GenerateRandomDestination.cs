using GOAP.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace GOAPGraph
{
    [NodeInfo("Generate random destination", "Generate random / Destination", hasInputParams: true, hasOutputParams: true)]
    public class GenerateRandomDestination : ActionNode
    {
        [ExposedProperty]
        public float range;
        public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, bool success)
        {
            var newWordlFact = new WorldFact();
            newWordlFact.name = "New destination";
            newWordlFact.valueType = ValueType.Vector3;
            
            var randomPointInsideUnitSphere = Random.insideUnitSphere;
            var randomPos = (randomPointInsideUnitSphere * range) + currentGraph.goapGraphObject.gameObject.transform.position;

            NavMeshHit hit;

            NavMesh.SamplePosition(randomPos, out hit, range, 1);

            Vector3 finalPos = hit.position;
            newWordlFact.value = finalPos.ToString();

            worldState.TryAddFact(newWordlFact);
            
            base.OnExecute(currentGraph, worldState);
        }
    }
}
