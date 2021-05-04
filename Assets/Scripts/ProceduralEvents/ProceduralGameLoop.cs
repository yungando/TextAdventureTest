using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ProceduralGameLoop : Singleton<ProceduralGameLoop>
{
    [SerializeField] private float _delayUntilFirstEvent = 10.0f;
    [SerializeField] private float _noEventWeight = 10.0f;

    private List<ProceduralEvent> _events = new List<ProceduralEvent>();

    private float timeTillEvent = 100.0f;

    public override void Awake()
    {
        base.Awake();

        foreach (ProceduralEvent e in Resources.LoadAll("", typeof(ProceduralEvent)))
        {
            _events.Add(e);
        }

        foreach (var e in _events)
        {
            e.Init();
        }

        timeTillEvent = _delayUntilFirstEvent;
    }

    void Update()
    {
        timeTillEvent -= Time.deltaTime;

        if (timeTillEvent > 0.0f)
        {
            return;
        }

        var validEvents = GetValidEvents();
        var validEventWeights = new List<float>();

        foreach (var e in validEvents)
        {
            validEventWeights.Add(e.eventWeight);
        }

        validEventWeights.Add(_noEventWeight);

        var eventIndex = GetRandomWeightedIndex(validEventWeights.ToArray());
        
        if (eventIndex == -1 || eventIndex == validEventWeights.Count - 1)
        {
            Debug.Log("Nothing Event Triggered.");

            timeTillEvent = 15.0f;

            return;
        }
            
        var selectedEvent = validEvents[eventIndex];

        if (selectedEvent.isActive == false)
        {
            selectedEvent.TriggerEvent();
            timeTillEvent = Random.Range(selectedEvent.minTimeToNextEvent, selectedEvent.maxTimeToNextEvent);
        }
    }

    public int GetRandomWeightedIndex(float[] weights)
    {
        if (weights == null || weights.Length == 0) return -1;

        float w;
        float t = 0;
        int i;
        for (i = 0; i < weights.Length; i++)
        {
            w = weights[i];

            if (float.IsPositiveInfinity(w))
            {
                return i;
            }
            else if (w >= 0f && !float.IsNaN(w))
            {
                t += weights[i];
            }
        }

        float r = Random.value;
        float s = 0f;

        for (i = 0; i < weights.Length; i++)
        {
            w = weights[i];
            if (float.IsNaN(w) || w <= 0f) continue;

            s += w / t;
            if (s >= r) return i;
        }

        return -1;
    }

    public List<ProceduralEvent> GetValidEvents()
    {
        var tempList = new List<ProceduralEvent>();

        foreach(var e in _events)
        {
            if (e.isActive == true)
                continue;

            if (e.EventRequirementsMet() == true)
            {
                tempList.Add(e);
            }
        }

        return tempList;
    }
}