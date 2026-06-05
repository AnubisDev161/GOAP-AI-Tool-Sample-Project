using GOAP.Core;
using GOAP.Core.Agent;
using GOAP.GOAPGraph;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[NodeInfo("Find campfire", "Example / Find campfire", hasInputParams: true, hasOutputParams: true)]
public class FindCampfire : ActionNode
{
    [ExposedProperty]
    public string campfirePositionKeyName;

    [ExposedProperty]
    public string campfireTag;

    public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, bool success = true)
    {
        success = false;
        var campfire = GameObject.FindGameObjectWithTag(campfireTag);

        if (campfire != null)
        {
            var campfirePositionKey = currentGraph.Blackboard.GetKeyWithExpectedType(campfirePositionKeyName, GOAPBlackbaord.BlackboardKeyType.Vector3);
            campfirePositionKey.value = campfire.transform.position;
            success = true;
        }

        base.OnExecuteFinished(currentGraph,worldState, success);
    }

    public override bool IsAchvievable(GOAPGraphAsset currentGraph)
    {
        var campfire = GameObject.FindGameObjectWithTag(campfireTag);

        return campfire != null;
    }
}
