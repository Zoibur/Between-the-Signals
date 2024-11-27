using System;
using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public static event Action<int, int> OnClockChange;
    
    public int increment = 15;
    public float secondsPerIncrement = 7.5f;
    
    private int hour = 0;
    private int minute = 0;
    
    private float time = 0.0f;

    public int realMinutesPerDay = 5;
    public int startHour = 8;
    public int endHour = 23;
    
    private void Start()
    {
        hour = startHour;
        minute = 0;
        OnClockChange?.Invoke(hour, minute);

       // int minutesPerDay = hoursPerDay * 60;
        int hoursPerDay = endHour - startHour;
        int incrementsPerDay = (60 / increment) * hoursPerDay;
        int incrementsPerRealMin = incrementsPerDay / realMinutesPerDay;
        secondsPerIncrement = 60 / incrementsPerRealMin;


        StartCoroutine(ClockLoop());
    }

    private IEnumerator ClockLoop()
    {
        while (true) {
            yield return new WaitForSeconds(secondsPerIncrement);
            IncrementClock();
            OnClockChange?.Invoke(hour, minute);
        }
    }

  
    
    private void IncrementClock()
    {
        minute += increment;
        if (minute >= 60) {
            hour++;
            if (hour >= 23) {
                //hour = 0;
                StartCoroutine(GameManager.Instance.LoadNextDay());
            }
                
            /* we'll modulo since if we use an increment like 25, we'll need to set the minutes to non-zero;
               for example, 0 -> 25 -> 50 -> 75 | add hour | set minute to 75 % 60 or 15. */
            minute = minute % 60;
        }
        time -= secondsPerIncrement;
    }
    
    private void Update()
    {
        time += Time.deltaTime;
        //if (time >= secondsPerIncrement) {
        //    IncrementClock();
        //    OnClockChange?.Invoke(hour, minute);
        //}
    }
}
