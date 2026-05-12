using System;
using UnityEngine;
using GOAP.Data;

namespace GOAPGraph
{
    [NodeInfo("Goal State", "Goal/Goal", hasFlowInput: true, hasFlowOutput: false, hasInputParams: true, hasOutputParams: false, paramPortsHaveSingleCapacity: false)]
    public class GoalWorldStateNode : GOAPGraphNode
    {
        [ExposedProperty]
        public string name;

        [ExposedProperty]
        public float priority = 1.0f;
    }
}
