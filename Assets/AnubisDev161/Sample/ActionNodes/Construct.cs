using GOAP.Core;
using GOAP.Core.Agent;
using GOAP.GOAPGraph;
using System.Collections.Generic;
using UnityEngine;

namespace ExampleProject
{
    [NodeInfo("Construct ", "Example / construct ", hasInputParams: true, hasOutputParams: true)]
    public class Construct : ActionNode
    {
        [ExposedProperty]
        public GameObject prefab;

        [ExposedProperty]
        public float heightOffset;

        [ExposedProperty]
        public string SaveObjectInKeyWithName;

        [ExposedProperty]
        public Vector3 optionalSpecificPosition;

        [ExposedProperty]
        public string optionalSpecificPositionKeyName;

        public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, bool success = true)
        {
            success = false;
            Vector3 spawnPos;
            if (optionalSpecificPosition == Vector3.zero)
            {
                spawnPos = new Vector3(currentGraph.agent.transform.position.x, currentGraph.agent.transform.position.y + heightOffset, currentGraph.agent.transform.position.z);
            }
            else
            {
                spawnPos = optionalSpecificPosition;
            }
         
            var instantiatedGameObject = GameObject.Instantiate(prefab, spawnPos, prefab.transform.rotation);

            var key = currentGraph.Blackboard.GetKeyWithExpectedType(SaveObjectInKeyWithName, GOAPBlackbaord.BlackboardKeyType.GameObject);
            key.value = instantiatedGameObject;
            currentGraph.Blackboard.SetKey(SaveObjectInKeyWithName, key);
            if (instantiatedGameObject != null) success = true;
            base.OnExecuteFinished(currentGraph, worldState, success);
        }
    }
}