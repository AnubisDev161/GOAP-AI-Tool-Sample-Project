using GOAP.Core;
using System.Collections.Generic;

namespace GOAP.GOAPGraph
{
    [NodeInfo("Delete random destination", "Generate random / Delete destination", hasInputParams: true, hasOutputParams: true)]
    public class DeletetargetDuringExecution : ActionNode
    {
        [ExposedProperty]
        public float range;
        public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, bool success = true)
        {
            var blackbaordKey = currentGraph.Blackboard.GetKey("TargetPos");

            if (blackbaordKey == null)
            {
                base.OnExecuteFinished(currentGraph, worldState, false);
                return;
            }

            blackbaordKey.value = null; 
            base.OnExecute(currentGraph, worldState);
        }
    }
}
