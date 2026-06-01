using ExampleProject;
using GOAP.Core;
using GOAP.GOAPGraph;
using System.Collections.Generic;
using UnityEngine;

[NodeInfo("Sleep at improvised camp", "Example / Sleep at improvised camp", hasInputParams: true, hasOutputParams: true)]
public class SleepAtImprovisedCamp : Wait
{
    public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, bool success = true)
    {
        currentGraph.agent.StartCoroutine(WaitForSeconds(currentGraph, worldState, success));
    }

    protected override void OnWaitingTimeFinished(GOAPGraphAsset currentGraph, WorldState worldState, bool success)
    {
        var dayNightcycle = GameObject.FindAnyObjectByType<DayNightCycle>();

        if (dayNightcycle != null)
        {
            dayNightcycle.EndNight();
        }

        base.OnWaitingTimeFinished(currentGraph, worldState, success);
    }
}
