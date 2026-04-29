using GOAPGraph;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAPGraph
{
    [NodeInfo("Move To", "AI / Move To", hasInputParams: true, hasOutputParams: false)]
    public class MoveTo : GOAPGraphNode
    {
        [ExposedProperty]
        public Vector3 destination;
        public override void OnProcess(GOAPGraphAsset currentGraph, DebugInfo debugInfo)
        {
            currentGraph.goapGraphObject.navMeshAgent.destination = destination;

            currentGraph.goapGraphObject.destinationReached += OnDestinationReached;

        }

        private void OnDestinationReached(GOAPGraphAsset currentGraph)
        {
            Debug.Log("Move to node executed");
            base.OnProcess(currentGraph, new DebugInfo(true, TerminationReason.None));
        }
    }
}