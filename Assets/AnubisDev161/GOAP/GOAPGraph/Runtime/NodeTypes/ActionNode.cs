using GOAP.Data;
using GOAPGraph;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GOAPGraph
{
    [NodeInfo("Action", "Action / Action", hasInputParams: true, hasOutputParams: true, paramPortsHaveSingleCapacity: false)]
    public class ActionNode : GOAPGraphNode
    {
        [ExposedProperty]
        public string name;

        [ExposedProperty]
        public float cost = 1.0f;

        // Tells the GOAPAction to remove the preconditions from the curretn worldState ater successful execution
        [ExposedProperty]
        public bool RemovePreconditionsFromWorldState;

        public virtual void OnAbandonCurrentPlan(GOAPGraphAsset currentGraph, WorldState worldState)
        {
            Debug.Log($"<color=orange>Action: {name} abandoned, current world state is {worldState}");
        }
    }
}

