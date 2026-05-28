using UnityEngine;
using System.Collections.Generic;
using GOAP.GOAPGraph;
using GOAP.Core;
using GOAP.Core.Agent;

namespace ExampleProject
{
    [NodeInfo("Destroy game object", "Example / Destroy game object", hasInputParams: true, hasOutputParams: true)]
    public class DestroyGameObject : ActionNode
    {
        [ExposedProperty]
        public string gameObjectKeyName;

        public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, bool success = true)
        {
            success = false;
            var gameObjectKey = currentGraph.Blackboard.GetKeyWithExpectedType(gameObjectKeyName, GOAPBlackbaord.BlackboardKeyType.GameObject);
            var gameObject = gameObjectKey.value as GameObject;
            if (gameObject != null)
            {
                success = true;
                GameObject.Destroy(gameObject);
            }

            base.OnExecuteFinished(currentGraph, worldState, success);
        }
    }
}