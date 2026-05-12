using GOAPGraph;
using System;
using System.Collections.Generic;
using UnityEngine;
using GOAP.Data;

namespace GOAPGraph
{
    [NodeInfo("Action", "Action / Action", hasInputParams: true, hasOutputParams: true, paramPortsHaveSingleCapacity: false)]
    public class ActionNode : GOAPGraphNode
    {
        [ExposedProperty]
        public string name;

        [ExposedProperty]
        public float cost = 1.0f;
    }
}

