using System.Collections.Generic;
using UnityEngine;
using GOAP.Core;

namespace GOAP.GOAPGraph
{
    [NodeInfo("Start State", "Start/Start", hasFlowInput: false, hasFlowOutput: true, paramPortsHaveSingleCapacity: false, hasOutputParams: true)]
    public class StartWorldStateNode : GOAPGraphNode
    {
        public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, bool success = true)
        {
            Debug.Log("InitialNode node processed");

            base.OnExecute(currentGraph, worldState);
        }
    }
}
