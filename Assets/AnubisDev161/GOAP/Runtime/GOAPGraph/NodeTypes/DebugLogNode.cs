using System.Collections.Generic;
using UnityEngine;
using GOAP.Core;

namespace GOAP.GOAPGraph
{
    [NodeInfo("Debug Log", "Debug/Debug Log Console")]
    public class DebugLogNode : GOAPGraphNode    
    {
        [ExposedProperty] 
        public string logMessage;
        public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState,  Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, bool success = true)
        {
            Debug.Log(logMessage);
            
            base.OnExecute(currentGraph, worldState);
        }
    }
}
