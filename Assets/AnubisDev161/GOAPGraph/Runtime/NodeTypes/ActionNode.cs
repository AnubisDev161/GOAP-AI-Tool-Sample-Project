using GOAPGraph;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOAPGraph
{
    [NodeInfo("Action", "Action / Action", hasInputParams: true, hasOutputParams: true)]
    public class ActionNode : GOAPGraphNode
    {
        [ExposedProperty]
        public string name;

        [ExposedProperty]
        public float cost = 1.0f;
    }
}

