using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HallwayGuy : MonoBehaviour
{
    private interface IState
    {
        string ID { get; }
        void OnEnter(HallwayGuy self) { }
        void OnEvent(HallwayGuy self, EventManager.EventID eventID) { }
        void OnUpdate(HallwayGuy self) { }
        void OnExit(HallwayGuy self) { }
    }

    private class Idle : IState
    {
        public string ID => "Idle";

        public void OnEvent(HallwayGuy self, EventManager.EventID eventID)
        {
            switch (eventID) {
                case EventManager.EventID.StartPatrol:
                    self.ChangeState(new Patrolling());
                    break;
            }
        }
    }

    private class Patrolling : IState
    {
        public string ID => "Patrolling";
        
        private float progress;

        private Coroutine footstepCoroutine;
        
        private IEnumerator FootstepLoop(HallwayGuy self)
        {
            while (true) {
                yield return new WaitForSeconds(UnityEngine.Random.Range(1.0f, 1.1f));
                self.audioSource.Play();
            }
        }
        
        public void OnEnter(HallwayGuy self)
        {
            progress = 0.0f;
            footstepCoroutine = self.StartCoroutine(FootstepLoop(self));
        }

        public void OnUpdate(HallwayGuy self)
        {
            progress += Time.deltaTime * (self.patrolSpeed / Vector3.Distance(self.patrolStart, self.patrolEnd));
            self.transform.position = Vector3.Lerp(self.patrolStart, self.patrolEnd, progress);

            if (progress >= 1.0f) {
                self.ChangeState(new Idle());
            }
        }

        public void OnExit(HallwayGuy self)
        {
            self.StopCoroutine(footstepCoroutine);
        }
    }
    
    private class Alert : IState
    {
        public string ID => "Alert";

        public const float Speed = 1.5f;
        private float progress;
        
        private Vector3 startPosition;
        private Coroutine footstepCoroutine;
        private IEnumerator FootstepLoop(HallwayGuy self)
        {
            while (true) {
                yield return new WaitForSeconds(UnityEngine.Random.Range(1.0f, 1.1f));
                self.audioSource.Play();
            }
        }
        
        public void OnEnter(HallwayGuy self)
        {
            startPosition = self.transform.position;
            progress = 0.0f;
            footstepCoroutine = self.StartCoroutine(FootstepLoop(self));
        }

        public void OnUpdate(HallwayGuy self)
        {
            progress += Time.deltaTime * (self.patrolSpeed / Vector3.Distance(self.patrolStart, self.patrolEnd));
            self.transform.position = Vector3.Lerp(startPosition, self.patrolEnd, progress);

            if (progress >= 1.0f) {
                self.ChangeState(new Idle());
            }
        }

        public void OnExit(HallwayGuy self)
        {
            self.StopCoroutine(footstepCoroutine);
        }
    }

    public Vector3 patrolStart;
    public Vector3 patrolEnd;
    public float patrolSpeed;
    public Vector3 outsideDoorPosition;
    
    private IState state;
    
    private AudioSource audioSource;
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(outsideDoorPosition, 0.1f);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(patrolStart, 0.1f);
        Gizmos.DrawSphere(patrolEnd, 0.1f);
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(patrolStart, patrolEnd);
    }

    private void ChangeState(IState newState)
    {
        if (state != null) {
            state.OnExit(this);
        }

        state = newState;
        state.OnEnter(this);
    }
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ChangeState(new Idle());
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            ChangeState(new Patrolling());
        }

        if (state != null) {
            state.OnUpdate(this);
        }
    }
}
