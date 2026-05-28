using GOAP.Core;
using GOAP.Core.Agent;
using GOAP.GOAPGraph;
using Mono.Cecil;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputManagerEntry;

[NodeInfo("Find game object", "Example / Find game object", hasInputParams: true, hasOutputParams: true)]
public class FindGameObjectByTag : ActionNode
{
    [ExposedProperty]
    public string gameObjectKeyName;

    [ExposedProperty]
    public string gameObjectTag;

    public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, bool success = true)
    {
        success = false;
        var gameObject = GameObject.FindGameObjectWithTag(gameObjectTag);

        if (gameObject != null)
        {
            var gameObjectKey = currentGraph.Blackboard.GetKeyWithExpectedType(gameObjectKeyName, GOAPBlackbaord.BlackboardKeyType.GameObject);
            gameObjectKey.value = gameObject;
            success = true;
        }

        base.OnExecuteFinished(currentGraph, worldState, success);
    }

    public override bool IsAchvievable(GOAPGraphAsset currentGraph)
    {
        var gameObject = GameObject.FindGameObjectWithTag(gameObjectTag);

        return gameObject != null;
    }
}
