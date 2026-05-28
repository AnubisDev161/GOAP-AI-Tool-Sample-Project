using UnityEngine;
using System.Collections.Generic;
using GOAP.GOAPGraph;
using GOAP.Core;
using GOAP.Core.Agent;

namespace ExampleProject
{
    [NodeInfo("Chop tree", "Example / Chop tree", hasInputParams: true, hasOutputParams: true)]
    public class ChopTree : ActionNode
    {
        [ExposedProperty]
        public string woodSourceKeyName;

        public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, bool success = true)
        {
            success = false;
            var woodSourceKey = currentGraph.Blackboard.GetKeyWithExpectedType(woodSourceKeyName, GOAPBlackbaord.BlackboardKeyType.GameObject);
            var woodSource =  woodSourceKey.value as GameObject;
            if (woodSource != null)
            {
                success = true;
                GameObject.Destroy(woodSource);
            }
           
            base.OnExecuteFinished(currentGraph, worldState, success);
        }
    }
}