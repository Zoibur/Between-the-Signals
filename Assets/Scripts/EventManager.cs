using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EventManager : MonoBehaviour
{
    public static EventManager instance { get; private set; }
    
    public float timeBetweenEventsMin;
    public float timeBetweenEventsMax;

    public event Action<string> OnEventRaised;
    
    public string[] eventIDs;
    
    private float timeSinceLastEvent = 0.0f;
    private float timeEventShouldHappen = 0.0f;
    
    private void ResetEventTimer()
    {
        timeEventShouldHappen = Random.Range(timeBetweenEventsMin, timeBetweenEventsMax);
        timeSinceLastEvent = 0.0f;
    }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ResetEventTimer();
    }

    void Update()
    {
        timeSinceLastEvent += Time.deltaTime;
        if (timeSinceLastEvent >= timeEventShouldHappen) {
            OnEventRaised?.Invoke(eventIDs[Random.Range(0, eventIDs.Length)]);
            ResetEventTimer();
        }
    }
}
