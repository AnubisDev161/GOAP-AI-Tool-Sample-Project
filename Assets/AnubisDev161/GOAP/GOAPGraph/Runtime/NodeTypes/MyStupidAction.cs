using GOAP.Data;
using System.Collections.Generic;
using UnityEngine;

namespace GOAPGraph
{
    [NodeInfo("My stupid node", "Stupid / Node", hasInputParams: true, hasOutputParams: true)]
    public class MyStupidAction : ActionNode
    {
        public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, bool success = true)
        {

            Debug.Log("Write stupid text and then destroy plan");

          //  worldState.Clear();

            Debug.Log("haha, world facts are: " + worldState.worldFacts.Count);
            base.OnExecute(currentGraph, worldState);
        }
    }
}
