using ExampleProject;
using GOAP.Core;
using GOAP.GOAPGraph;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

[NodeInfo("Construct campfire", "Example / Construct campfire", hasInputParams: true, hasOutputParams: true)]
public class ConstructCampfire : ActionNode
{
    [ExposedProperty]
    public GameObject campfirePrefab;

    [ExposedProperty]
    public float heightOffset;

    [ExposedProperty]
    public string campfirePositionKeyName;
    public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, bool success = true)
    {
        success = false;
        var spawnPos = new Vector3(currentGraph.agent.transform.position.x, currentGraph.agent.transform.position.y + heightOffset, currentGraph.agent.transform.position.z);
        var campfire = GameObject.Instantiate(campfirePrefab, spawnPos, Quaternion.identity);

        var campfirePosKey = currentGraph.Blackboard.GetKeyWithExpectedType(campfirePositionKeyName, GOAP.Core.Agent.GOAPBlackbaord.BlackboardKeyType.Vector3);
        campfirePosKey.value = spawnPos;
        currentGraph.Blackboard.SetKey(campfirePositionKeyName, campfirePosKey);
        
        if (campfire != null) success = true;

        base.OnExecuteFinished(currentGraph, worldState, success);
    }
}