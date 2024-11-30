using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        StartPatrol,
        LightFlicker,
    };
    
    public static event Action<EventID> OnEventRaised;

    [Serializable]
    public class EventDay
    {
        [Serializable]
        public class EventWeight
        {
            public EventID eventID;
            [Range(0, 100)]
            public int weight;
        }

        public float intervalMin;
        public float intervalMax;
        public EventWeight[] weights;
    }
    
    public EventDay[] days = new EventDay[5];
    
    void Start()
    {
        StartCoroutine(EventLoop());
    }
    
    private int BiasedRandom(IEnumerable<int> weights)
    {
        int random = UnityEngine.Random.Range(0, weights.Sum());
        int sum = 0;
        
        int index = 0;
        foreach (int weight in weights) {
            sum += weight;
            if (sum >= random) {
                break;
            }

            index++;
        }

        return index;
    }
    
    private EventID? ChooseEvent(int level)
    {
        EventDay day = days[level];
        if (day.weights == null || day.weights.Length == 0) {
            Debug.Log("No events found for level " + GameManager.Instance.GetCurrentLevel());
            return null;
        }
        
        Dictionary<int, EventID> table = new Dictionary<int, EventID>();
        List<int> weights = new List<int>();
        for (int i = 0; i < day.weights.Length; i++) {
            table.Add(i, day.weights[i].eventID);
            weights.Add(day.weights[i].weight);
        }

        return table[BiasedRandom(weights)];
    }
    
    private IEnumerator EventLoop()
    {
        while (true) {
            EventDay day = days[GameManager.Instance.GetCurrentLevel() - 1];
            yield return new WaitForSeconds(UnityEngine.Random.Range(day.intervalMin, day.intervalMax));
            EventID? eventID = ChooseEvent(GameManager.Instance.GetCurrentLevel() - 1);
            if (eventID.HasValue) {
                RaiseEvent(eventID.Value);
            }
        }
    }
    
    private void RaiseEvent(EventID eventID)
    {
        Debug.Log("Event raised: " + eventID.ToString());
        OnEventRaised?.Invoke(eventID);
    }
}
