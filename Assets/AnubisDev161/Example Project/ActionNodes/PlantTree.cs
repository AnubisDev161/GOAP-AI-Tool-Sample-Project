using System.Collections.Generic;
using GOAP.GOAPGraph;
using GOAP.Core;
using UnityEngine;

namespace ExampleProject
{
    [NodeInfo("Plant tree", "Example / Plant tree", hasInputParams: true, hasOutputParams: true)]
    public class PlantTree : ActionNode
    {
        [ExposedProperty]
        public GameObject beaconPrefab;
        public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, bool success = true)
        {
            var beacon = GameObject.Instantiate(beaconPrefab, currentGraph.agent.transform.position, Quaternion.identity);
            if (beacon == null) success = false;

            base.OnExecuteFinished(currentGraph, worldState, success);
        }
    }
}