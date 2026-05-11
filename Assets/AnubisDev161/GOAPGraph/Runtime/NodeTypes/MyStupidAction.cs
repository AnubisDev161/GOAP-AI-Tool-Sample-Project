using System.Collections.Generic;
using UnityEngine;

namespace GOAPGraph
{
    [NodeInfo("My stupid node", "Stupid Node", hasInputParams: true, hasOutputParams: true)]
    public class MyStupidAction : ActionNode
    {

        public override void OnExecute(GOAPGraphAsset currentGraph, Dictionary<string, bool> worldFacts)
        {

            Debug.Log("Write stupid text and then destroy plan");

            worldFacts.Clear();

            Debug.Log("haha, world facts are: " + worldFacts.Count);
            base.OnExecute(currentGraph, worldFacts);
        }
    }
}
