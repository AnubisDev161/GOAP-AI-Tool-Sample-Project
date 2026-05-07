using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace GOAPGraph
{
    [NodeInfo("Start State", "Start/Start", hasFlowInput: false, hasFlowOutput: true, paramPortsHaveSingleCapacity: false)]
    public class StartWorldStateNode : GOAPGraphNode
    {
        public override void OnProcess(GOAPGraphAsset currentGraph, DebugInfo debugInfo)
        {
            debugInfo.success = true;
            Debug.Log("InitialNode node processed");


            base.OnProcess(currentGraph, debugInfo);
        }
    }
}
