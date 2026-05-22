using GOAP.Core.Agent;
using UnityEngine;

/// <summary>
/// Base class for timer related triggers. OnIntervalFinished is called every time when the currentInterval is >= interval
/// </summary>
public class TimerTrigger : GOAPBlackboardTrigger
{
    [SerializeField]
    private float interval;
    private float currentInterval;

    private void Update()
    {
        currentInterval += Time.deltaTime;

        if (currentInterval >= interval)
        {
            OnIntervalFinished();
            currentInterval = 0;
        }
    }

    public virtual void OnIntervalFinished()
    {

    }
}
