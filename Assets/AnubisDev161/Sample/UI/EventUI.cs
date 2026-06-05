using ExampleProject;
using GOAP.Core;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class EventUI : MonoBehaviour
{
    [SerializeField]
    private string hungerKeyName;

    [SerializeField]
    private string thirstKeyName;

    [SerializeField]
    private string scaredKeyName;

    [SerializeField]
    private string stockpiledWoodKeyName;

    [SerializeField]
    private string stockpiledStoneKeyName;

    [SerializeField]
    private List<Bar> bars = new List<Bar>();

    [SerializeField]
    private List<TMP_Text> counters = new List<TMP_Text>();

    [SerializeField]
    private ExampleAgent selectedAgent;

    public void OnBeingNightButtonClicked()
    {
        var allAgents = GameObject.FindObjectsByType<ExampleAgent>(FindObjectsSortMode.InstanceID);

        var isNightWorldFact = new WorldFact();
        isNightWorldFact.value = "True";
        isNightWorldFact.name = "IsNight";
        isNightWorldFact.valueType = WorldFactType.Bool;

        foreach (var agent in allAgents)
        {
            agent.goapBrain.currentWorldState.TryAddFact(isNightWorldFact, true);
        }

        var dayNightcycle = GameObject.FindAnyObjectByType<DayNightCycle>();

        if (dayNightcycle != null)
        {
            dayNightcycle.StartNight();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }

        if (selectedAgent != null)
        {
            UpdateBars(selectedAgent);
        }
    }

    private void HandleClick()
    {
        Ray ray;
        RaycastHit hitInfo;

        var mousePos = Input.mousePosition;

        ray = Camera.main.ScreenPointToRay(mousePos);
        Physics.Raycast(ray, out hitInfo, Mathf.Infinity);

        UnityEngine.Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        if (hitInfo.collider != null && hitInfo.collider.TryGetComponent<ExampleAgent>(out ExampleAgent agent))
        {
            UnityEngine.Debug.Log("<color=white>New agent selected!");
            selectedAgent = agent;
            UpdateBars(selectedAgent);
        }
    }

    private void UpdateBars(ExampleAgent agent)
    {
        if (agent.goapBrain.currentWorldState.worldFacts.TryGetValue(hungerKeyName, out var worldFact) && worldFact.name == hungerKeyName)
        {
            bars[0].UpdateValue(worldFact);
        }

        if (agent.goapBrain.currentWorldState.worldFacts.TryGetValue(thirstKeyName, out worldFact) && worldFact.name == thirstKeyName)
        {
            bars[1].UpdateValue(worldFact);
        }

        if (agent.goapBrain.currentWorldState.worldFacts.TryGetValue(scaredKeyName, out worldFact) && worldFact.name == scaredKeyName)
        {
            bars[2].UpdateValue(worldFact);
        }

        if (agent.goapBrain.currentWorldState.worldFacts.TryGetValue(stockpiledWoodKeyName, out worldFact) && worldFact.name == stockpiledWoodKeyName)
        {
            counters[0].text = $"Stockpiled wood: {worldFact.value}";
        }

        if (agent.goapBrain.currentWorldState.worldFacts.TryGetValue(stockpiledStoneKeyName, out worldFact) && worldFact.name == stockpiledStoneKeyName)
        {
            counters[1].text = $"Stockpiled stone: {worldFact.value}";
        }
    }
}
