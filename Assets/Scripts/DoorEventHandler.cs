using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEventHandler : MonoBehaviour
{
    public AudioClip openSound;
    public AudioClip slamSound;
    
    private AudioSource audioSource;
    private Coroutine coroutine = null;

    private float rotationClosed = 0.0f;
    private float rotationOpen = 20.0f;
    private bool isOpen = false;

    private float GetOpenness()
    {
        return (transform.rotation.eulerAngles.y + rotationClosed) / rotationOpen;
    }
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) {
            if (isOpen) {
                SlamDoor();
            }
            else {
                OpenDoor();
            }
        }
    }

    private void OnEvent(EventManager.EventID eventID)
    {
        switch (eventID) {
            case EventManager.EventID.DoorOpen:
                OpenDoor();
                break;
            case EventManager.EventID.DoorSlam:
                SlamDoor();
                break;
        }
    }

    private void OpenDoor()
    {
        if (isOpen)
            return;
        
        if (coroutine != null) {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(RotateDoor(20.0f, 1.5f));
        if (audioSource.isPlaying) {
            audioSource.Stop();
        }
        
        audioSource.PlayOneShot(openSound);
        isOpen = true;
    }

    private void SlamDoor()
    {
        if (!isOpen)
            return;
        
        if (coroutine != null) {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(RotateDoor(0.0f, 0.175f * GetOpenness()));
        if (audioSource.isPlaying) {
            audioSource.Stop();
        }
        
        audioSource.PlayOneShot(slamSound);
        isOpen = false;
    }

    private IEnumerator RotateDoor(float rotation, float duration)
    {
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, rotation, 0);

        float elapsed = 0.0f;
        while (elapsed < duration) {
            float t = elapsed / duration;
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.rotation = targetRotation;
    }
}
