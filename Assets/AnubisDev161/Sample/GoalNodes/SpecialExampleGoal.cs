using GOAP.Core;
using GOAP.GOAPGraph;
using UnityEngine;

[NodeInfo("Special example goal", "Goal / Special example goal", hasFlowInput: true, hasFlowOutput: false, hasInputParams: true, hasOutputParams: false, paramPortsHaveSingleCapacity: false)]
public class SpecialExampleGoal : GoalWorldStateNode
{
    [ExposedProperty]
    public bool callGameWonOnAchieved;

    public override void OnAchieved()
    {
        base.OnAchieved();
    }
}