using GOAP.Core;
using UnityEngine;

namespace GOAP.GOAPGraph
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
            base.OnExecuteFinished(currentGraph, worldState, false);
        }

        /// <summary>
        /// Gives you the possibility to add specific validation checks that don't require extra world facts
        /// Returns true by default so that a base Action can always be executed
        /// </summary>
        /// <returns></returns>
        public virtual bool IsAchvievable(GOAPGraphAsset currentGraph)
        {
            return true;
        }
    }
}