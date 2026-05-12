using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using GOAP.Data;

namespace GOAPGraph
{
    [NodeInfo("Blackbaord Key", "Blackbaord/World Fact", hasFlowInput: false, hasFlowOutput: false, hasInputParams: true, hasOutputParams: true, paramPortsHaveSingleCapacity: false)]
    public class BlackbaordKeyNode : GOAPGraphNode
    {
        [ExposedWorldFactProperty]
        public WorldFact worldFact;

        public WorldFact GetData()
        {
            return worldFact;
        }
    }
}
