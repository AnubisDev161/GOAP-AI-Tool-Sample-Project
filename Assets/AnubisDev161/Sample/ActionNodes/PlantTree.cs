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

        [ExposedProperty]

        public float heightOffset;
        public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, bool success = true)
        {
            var spawnPos = new Vector3(currentGraph.agent.transform.position.x, currentGraph.agent.transform.position.y + heightOffset, currentGraph.agent.transform.position.z);
            var tree = GameObject.Instantiate(beaconPrefab, spawnPos, Quaternion.identity);
            if (tree == null) success = false;

            base.OnExecuteFinished(currentGraph, worldState, success);
        }
    }
}