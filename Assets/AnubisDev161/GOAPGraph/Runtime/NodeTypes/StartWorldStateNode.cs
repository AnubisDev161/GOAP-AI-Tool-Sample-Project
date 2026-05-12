using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using GOAP.Data;

namespace GOAPGraph
{
    [NodeInfo("Start State", "Start/Start", hasFlowInput: false, hasFlowOutput: true, paramPortsHaveSingleCapacity: false, hasOutputParams: true)]
    public class StartWorldStateNode : GOAPGraphNode
    {
        public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, bool success)
        {
            Debug.Log("InitialNode node processed");

            base.OnExecute(currentGraph, worldState);
        }
    }
}
