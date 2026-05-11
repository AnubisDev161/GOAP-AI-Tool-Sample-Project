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

        public override void OnExecute(GOAPGraphAsset currentGraph, Dictionary<string, bool> worldFacts)
        {
            //currentGraph.goapGraphObject.navMeshAgent.destination = destination;

            //currentGraph.goapGraphObject.destinationReached += OnDestinationReached;
            base.OnExecute(currentGraph, worldFacts);
        }

        private void OnDestinationReached(GOAPGraphAsset currentGraph, Dictionary<string, bool> worldFacts)
        {
            Debug.Log("Move to node executed");
            base.OnExecuteFinished(currentGraph, worldFacts);
        }
    }
}