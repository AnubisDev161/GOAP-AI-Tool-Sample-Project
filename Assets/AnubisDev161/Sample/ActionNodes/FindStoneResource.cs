using GOAP.Core;
using GOAP.Core.Agent;
using GOAP.GOAPGraph;
using Mono.Cecil;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputManagerEntry;

[NodeInfo("Find stone source", "Example / Find stone source", hasInputParams: true, hasOutputParams: true)]
public class FindStoneResource : ActionNode
{
    [ExposedProperty]
    public string stoneSourceKeyName;

    [ExposedProperty]
    public string stoneTag;

    public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, bool success = true)
    {
        success = false;
        var stoneDeposit = GameObject.FindGameObjectWithTag(stoneTag);

        if (stoneDeposit != null)
        {
            var stoneSourceKey = currentGraph.Blackboard.GetKeyWithExpectedType(stoneSourceKeyName, GOAPBlackbaord.BlackboardKeyType.GameObject);
            stoneSourceKey.value = stoneDeposit;
            success = true;
        }

        base.OnExecuteFinished(currentGraph, worldState, success);
    }

    public override bool IsAchvievable(GOAPGraphAsset currentGraph)
    {
        var stoneDeposit = GameObject.FindGameObjectWithTag(stoneTag);

        return stoneDeposit != null;
    }
}
