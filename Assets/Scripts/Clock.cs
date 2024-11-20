using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public TextMeshPro textGUI;

    private void OnEnable()
    {
        TimeManager.OnClockChange += OnClockChange;
    }

    private void OnDisable()
    {
        TimeManager.OnClockChange -= OnClockChange;
    }

    private void OnClockChange(int hour, int minute)
    {
        textGUI.text = hour.ToString("00") + ":" + minute.ToString("00");
    }
}
