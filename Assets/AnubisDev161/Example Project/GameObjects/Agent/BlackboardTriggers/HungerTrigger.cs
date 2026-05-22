using UnityEngine;

public class HungerTrigger : TimerTrigger
{
    [SerializeField]
    private float hungerIncreasePerInterval;

    private void Start()
    {
        AddSpecifiedWorldFactWithValue("0.0", false);
    }

    public override void OnIntervalFinished()
    {
        var worldFact = GetSpecifiedWorldFact();

        var currentValue = (float)worldFact.Value.GetValue();
        AddSpecifiedWorldFactWithValue((hungerIncreasePerInterval + currentValue).ToString(), false);
    }
}
