using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GOAPGraph
{
    [NodeInfo("Start State", "Start/Start", hasFlowInput: false, hasFlowOutput: true, paramPortsHaveSingleCapacity: false, hasOutputParams: true)]
    public class StartWorldStateNode : GOAPGraphNode
    {
        public override void OnExecute(GOAPGraphAsset currentGraph, Dictionary<string, bool> worldFacts)
        {
            Debug.Log("InitialNode node processed");

            base.OnExecute(currentGraph, worldFacts);
        }
    }
}
