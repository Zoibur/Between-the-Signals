using System;
using System.Collections;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public AnimationCurve lightIntensity;
    
    private bool isFlickering = false;
    public Light light1;
    public Light light2;

    public AudioClip audio;

    private IEnumerator Flicker()
    {
        float originalIntensity1 = light1.intensity;
        float originalIntensity2 = light2.intensity;
        isFlickering = true;

        float progress = 0.0f;
        while (progress < 1.0f) {
            light1.intensity = lightIntensity.Evaluate(progress);
            light2.intensity = lightIntensity.Evaluate(progress) * 8;
            progress += Time.deltaTime;
            yield return null;
        }
        
        isFlickering = false;
        light1.intensity = originalIntensity1;
        light2.intensity = originalIntensity2;
    }
    
    private void OnEvent(EventManager.EventID eventID)
    {
        if (eventID == EventManager.EventID.LightFlicker && !isFlickering) {
            AudioManager.instance.PlaySoundFXClip(audio, transform.GetChild(0).transform, 0.8f);
            StartCoroutine(Flicker());
        }
    }

    private void OnEnable()
    {
        EventManager.OnEventRaised += OnEvent;
    }

    private void OnDisable()
    {
        EventManager.OnEventRaised -= OnEvent;
    }
}
