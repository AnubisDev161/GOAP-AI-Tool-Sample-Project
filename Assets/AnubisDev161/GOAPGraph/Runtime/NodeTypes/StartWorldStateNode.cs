using System;
using UnityEngine;

namespace GOAPGraph
{
    [NodeInfo("Start State", "Start/Start", hasFlowInput: false, hasFlowOutput: true, hasInputParams: false, hasOutputParams: false)]
    public class StartWorldStateNode : GOAPGraphNode
    {
        public override void OnProcess(GOAPGraphAsset currentGraph, DebugInfo debugInfo)
        {
            debugInfo.success = true;

            base.OnProcess(currentGraph, debugInfo);
        }
    }
}
