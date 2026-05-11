using System;
using UnityEngine;

namespace GOAPGraph
{
    [NodeInfo("Goal State", "Goal/Goal", hasFlowInput: true, hasFlowOutput: false, hasInputParams: true, hasOutputParams: false, paramPortsHaveSingleCapacity: false)]
    public class GoalWorldStateNode : GOAPGraphNode
    {
        [ExposedProperty]
        public string name;

        [ExposedProperty]
        public float priority = 1.0f;
        public override void OnProcess(GOAPGraphAsset currentGraph, DebugInfo debugInfo)
        {
            debugInfo.success = true;

            base.OnProcess(currentGraph, debugInfo);
        }
    }
}
