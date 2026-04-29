using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace GOAPGraph
{
    [NodeInfo("InitialNode", "Test/InitialNode", hasFlowInput: false, hasFlowOutput: true)]
    public class StartNode : GOAPGraphNode
    {
        public override void OnProcess(GOAPGraphAsset currentGraph, DebugInfo debugInfo)
        {
            debugInfo.success = true;
            Debug.Log("InitialNode node processed");


            base.OnProcess(currentGraph, debugInfo);
        }
    }
}
