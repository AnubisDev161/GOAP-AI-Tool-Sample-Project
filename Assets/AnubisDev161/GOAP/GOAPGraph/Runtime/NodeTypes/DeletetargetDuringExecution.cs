using GOAP;
using GOAP.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace GOAPGraph
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
