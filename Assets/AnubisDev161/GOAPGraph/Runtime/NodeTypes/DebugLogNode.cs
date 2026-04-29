

using UnityEngine;

namespace GOAPGraph
{
    [NodeInfo("Debug Log", "Debug/Debug Log Console")]
    public class DebugLogNode : GOAPGraphNode    
    {
        [ExposedProperty] 
        public string logMessage;
        public override void OnProcess(GOAPGraphAsset currentGraph, DebugInfo debugInfo)
        {
            debugInfo.success = true;
            Debug.Log(logMessage);
            
            base.OnProcess(currentGraph, debugInfo);
        }
    }
}
