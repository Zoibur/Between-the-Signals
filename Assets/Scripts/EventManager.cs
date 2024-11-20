using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [Header("Testing")]
    public bool TestFireEvent;
    public EventID TestFireEventID;

    private void OnValidate()
    {
        if (TestFireEvent) {
            RaiseEvent(TestFireEventID);
            TestFireEvent = false;
        }
    }
    
    public enum EventID
    {
        DoorOpen,
        DoorSlam,
        WindowTap,
        ToiletFlush,
    };

    public float minEventInterval;
    public float maxEventInterval;
    
    public static event Action<EventID> OnEventRaised;

    void Start()
    {
        StartCoroutine(EventLoop());
    }

    private IEnumerator EventLoop()
    {
        while (true) {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minEventInterval, maxEventInterval));
            RaiseEvent((EventID)UnityEngine.Random.Range(0, Enum.GetValues(typeof(EventID)).Length));
        }
    }
    
    private void RaiseEvent(EventID eventID)
    {
        Debug.Log("Event raised: " + eventID.ToString());
        OnEventRaised?.Invoke(eventID);
    }
}
