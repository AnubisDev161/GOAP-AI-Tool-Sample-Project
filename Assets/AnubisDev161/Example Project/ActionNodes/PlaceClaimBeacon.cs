using System.Collections.Generic;
using GOAP.GOAPGraph;
using GOAP.Core;

namespace ExampleProject
{
    [NodeInfo("Place claim beacon", "Example / Claim territory", hasInputParams: true, hasOutputParams: true)]
    public class PlaceClaimBeacon : ActionNode
    {
        public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, bool success = true)
        {
            var exampleAgent = currentGraph.agent as ExampleAgent;

            base.OnExecuteFinished(currentGraph, worldState, exampleAgent.PlaceBeacon());
        }
    }
}