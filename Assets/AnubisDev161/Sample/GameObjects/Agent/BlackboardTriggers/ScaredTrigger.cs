using GOAP.Core;
using GOAP.Core.Agent;
using UnityEngine;

public class ScaredTrigger : TimerTrigger
{
    [SerializeField]
    private float scaredIncreasePerInterval;

    private GOAPAgent agent;
    private void Start()
    {
        AddSpecifiedWorldFactWithValue("0.0f", false);
        agent = GetComponent<GOAPAgent>();
    }

    public override void OnIntervalFinished()
    {
        agent.goapBrain.currentWorldState.worldFacts.TryGetValue("IsInDanger", out var inDangerWorldFact);
      
        if (inDangerWorldFact != null && inDangerWorldFact.value != null && ((bool)inDangerWorldFact.GetValue()) == true)
        {
            var worldFact = GetSpecifiedWorldFact();
            var currentValue = (float)worldFact.Value.GetValue();
            AddSpecifiedWorldFactWithValue((scaredIncreasePerInterval + currentValue).ToString() + "f", false);
        }
    }
}
