using System.Collections.Generic;
using UnityEngine;
using GOAP.Data;

namespace GOAPGraph
{
    [NodeInfo("Debug Log", "Debug/Debug Log Console")]
    public class DebugLogNode : GOAPGraphNode    
    {
        [ExposedProperty] 
        public string logMessage;
        public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, bool success)
        {
            Debug.Log(logMessage);
            
            base.OnExecute(currentGraph, worldState);
        }
    }
}
