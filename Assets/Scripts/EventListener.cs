using System;
using UnityEngine;

public class EventListener : MonoBehaviour
{
    private void OnEvent(string id)
    {
        Debug.Log("Event: " + id);
    }
    private void Start()
    {
        EventManager.instance.OnEventRaised += OnEvent;
    }
}
