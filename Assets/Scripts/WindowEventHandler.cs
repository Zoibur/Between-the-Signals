using UnityEngine;

public class WindowEventHandler : MonoBehaviour
{
    public AudioClip tapSound;
    
    private AudioSource audioSource;
    
    private void OnEnable()
    {
        EventManager.OnEventRaised += OnEvent;
    }
    
    private void OnDisable()
    {
        EventManager.OnEventRaised -= OnEvent;
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEvent(EventManager.EventID eventID)
    {
        switch (eventID) {
            case EventManager.EventID.WindowTap:
                if (audioSource.isPlaying)
                    break;
                
                audioSource.PlayOneShot(tapSound);
                break;
        }
    }
}
