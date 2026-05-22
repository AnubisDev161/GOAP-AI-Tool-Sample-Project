using UnityEngine;
using System.Collections.Generic;
using GOAP.GOAPGraph;
using GOAP.Core;

namespace ExampleProject
{
    [NodeInfo("Chop tree", "Example / Chop tree", hasInputParams: true, hasOutputParams: true)]
    public class ChopTree : ActionNode
    {
        [ExposedProperty]
        public string woodSourceKeyName;

        public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, bool success = true)
        {
            var exampleAgent = currentGraph.agent as ExampleAgent;
            var woodSourceKey = currentGraph.Blackboard.GetKey(woodSourceKeyName);
            var woodSource =  woodSourceKey.value as GameObject;
            GameObject.Destroy(woodSource);

            base.OnExecuteFinished(currentGraph, worldState, success);
        }
    }
}