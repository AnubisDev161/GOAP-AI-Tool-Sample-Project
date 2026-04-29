using System;
using UnityEngine;

namespace GOAPGraph
{
    [NodeInfo("Goal State", "Goal/Goal", hasFlowInput: true, hasFlowOutput: false, hasInputParams: true, hasOutputParams: false)]
    public class GoalWorldStateNode : GOAPGraphNode
    {
        public override void OnProcess(GOAPGraphAsset currentGraph, DebugInfo debugInfo)
        {
            debugInfo.success = true;

            base.OnProcess(currentGraph, debugInfo);
        }
    }
}
