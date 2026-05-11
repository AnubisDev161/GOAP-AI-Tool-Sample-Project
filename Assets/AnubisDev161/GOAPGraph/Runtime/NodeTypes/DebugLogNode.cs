using System.Collections.Generic;
using UnityEngine;

namespace GOAPGraph
{
    [NodeInfo("Debug Log", "Debug/Debug Log Console")]
    public class DebugLogNode : GOAPGraphNode    
    {
        [ExposedProperty] 
        public string logMessage;
        public override void OnExecute(GOAPGraphAsset currentGraph, Dictionary<string, bool> worldFacts)
        {
            Debug.Log(logMessage);
            
            base.OnExecute(currentGraph, worldFacts);
        }
    }
}
