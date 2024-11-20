using UnityEngine;

public class AmbienceEventHandler : MonoBehaviour
{
    public EventManager.EventID handleEventID;
    public AudioClip sound;
    
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
        if (handleEventID == eventID && !audioSource.isPlaying) {
            audioSource.PlayOneShot(sound);
        }
    }
}
