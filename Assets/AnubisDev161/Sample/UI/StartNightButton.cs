using ExampleProject;
using TMPro;
using UnityEngine;

public class StartNightButton : MonoBehaviour
{
    void Start()
    {
        var dayNightcycle = GameObject.FindAnyObjectByType<DayNightCycle>();

        if (dayNightcycle != null)
        {
            dayNightcycle.OnNightEnded += OnNightEnded;
        }
    }

    public void OnStartNightButtonClicked()
    {
        var text = GetComponentInChildren<TextMeshProUGUI>();
        if (!text)
        {
            return;
        }

        text.text = "Night...";
    }

    private void OnDestroy()
    {
        var dayNightcycle = GameObject.FindAnyObjectByType<DayNightCycle>();

        if (dayNightcycle != null)
        {
            dayNightcycle.OnNightEnded -= OnNightEnded;

        }
    }

    private void OnNightEnded()
    {
        var text = GetComponentInChildren<TextMeshProUGUI>();
        if (!text)
        {
            return;
        }

        text.text = "Start Night";
    }
}