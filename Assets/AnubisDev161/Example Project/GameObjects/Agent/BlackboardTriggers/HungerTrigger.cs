using GOAP.Core.Agent;
using UnityEngine;

/// <summary>
/// This trigger increases the world fact Hungry over time by hungerIncreasePerInterval
/// </summary>
public class HungerTrigger : GOAPBlackboardTrigger
{
    [SerializeField]
    private float increaseHungerInterval;

    [SerializeField]
    private float hungerIncreasePerInterval;

    private float currentInterval;

    private void Start()
    {
        AddSpecifiedWorldFactWithValue("0.0", false);
    }

    private void Update()
    {
        currentInterval += Time.deltaTime;

        if (currentInterval >= increaseHungerInterval)
        {
            var worldFact = GetSpecifiedWorldFact();
            var currentValue = (float)worldFact.Value.GetValue();

            AddSpecifiedWorldFactWithValue((hungerIncreasePerInterval + currentValue).ToString(), false);
            currentInterval = 0;
        }
    }
}
