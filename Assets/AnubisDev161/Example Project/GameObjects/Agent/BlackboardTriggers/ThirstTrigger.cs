using UnityEngine;

public class ThirstTrigger : TimerTrigger
{
    [SerializeField]
    private float thirstIncreasePerInterval;

    private void Start()
    {
        AddSpecifiedWorldFactWithValue("0.0", false);
    }

    public override void OnIntervalFinished()
    {
        var worldFact = GetSpecifiedWorldFact();
        var currentValue = (float)worldFact.Value.GetValue();

        AddSpecifiedWorldFactWithValue((thirstIncreasePerInterval + currentValue).ToString(), false);
    }
}