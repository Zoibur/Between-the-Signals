using System;
using System.Collections;
//using Unity.VisualScripting;
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
        private bool deaf;

        private Coroutine footstepCoroutine;

        public Patrolling(float progress = 0.0f, bool deaf = false)
        {
            this.progress = progress;
            this.deaf = deaf;
        }
        
        private IEnumerator FootstepLoop(HallwayGuy self)
        {
            while (true) {
                self.audioSource.Play();
                yield return new WaitForSeconds(UnityEngine.Random.Range(1.0f, 1.1f));
            }
        }
        
        public void OnEnter(HallwayGuy self)
        {
            footstepCoroutine = self.StartCoroutine(FootstepLoop(self));
        }

        public void OnUpdate(HallwayGuy self)
        {
            progress += Time.deltaTime * (self.patrolSpeed / Vector3.Distance(self.patrolStart, self.patrolEnd));
            self.transform.position = Vector3.Lerp(self.patrolStart, self.patrolEnd, progress);

            if (progress >= 1.0f) {
                self.ChangeState(new Idle());
            }

            bool heard = !deaf && Vector3.Distance(self.transform.position, self.outsideDoorPosition) < self.hearingRadius;
            if (heard && GameManager.Instance.IsNoiseAboveThreshold(2)) {
                self.ChangeState(new Alert(progress));
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

        private float speed = 1.5f;
        
        private Vector3 start;
        private Coroutine footstepCoroutine;
        private IEnumerator FootstepLoop(HallwayGuy self, float waitMin, float waitMax)
        {
            while (true) {
                self.audioSource.Play();
                yield return new WaitForSeconds(UnityEngine.Random.Range(waitMin, waitMax));
            }
        }

        private IEnumerator Sequence(HallwayGuy self)
        {
            Debug.Log("Sequence: Heard something, stopping to listen...");
            yield return new WaitForSeconds(1.0f);
            
            Debug.Log("Sequence: Moving towards door...");
            Coroutine footsteps = self.StartCoroutine(FootstepLoop(self, 1.3f, 1.4f));
            
            float progress = 0.0f;
            while (progress < 1.0f) {
                progress += Time.deltaTime * (speed / Vector3.Distance(start, self.outsideDoorPosition));
                self.transform.position = Vector3.Lerp(start, self.outsideDoorPosition, progress);
                yield return null;
            }

            self.StopCoroutine(footsteps);
            Debug.Log("Sequence: Waiting for a bit outside door...");
            yield return new WaitForSeconds(0.75f);
            
            Debug.Log("Sequence: Knock on door...");
            AudioManager.instance.PlaySoundFXClip(self.knockingSound, self.transform, 1.0f);
            yield return new WaitForSeconds(5.0f);
            
            Debug.Log("Sequence: Check if making any noise...");
            float elapsed = 0.0f;
            while (elapsed < 4.0f) {
                if (GameManager.Instance.IsNoiseAboveThreshold(1)) {
                    Debug.Log("You died!");
                    DeathManager.Instance.PlayDeathCutscene();
                    yield break;
                }
                elapsed += Time.deltaTime;
                yield return null;
            }

            Debug.Log("Huh, must have been my imagination...");
            self.ChangeState(new Patrolling(0.55f, true));
            
            Debug.Log("Sequence: DONE");
        }

        public Alert(float patrolProgress)
        {
        }
        
        public void OnEnter(HallwayGuy self)
        {
            start = self.transform.position;
            self.StartCoroutine(Sequence(self));
        }

        public void OnUpdate(HallwayGuy self)
        {

        }

        public void OnExit(HallwayGuy self)
        {
        }
    }

    public Vector3 patrolStart;
    public Vector3 patrolEnd;
    public float patrolSpeed;
    public Vector3 outsideDoorPosition;
    public float hearingRadius = 4.5f;
    
    public AudioClip knockingSound;
    
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
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(outsideDoorPosition, hearingRadius);
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
        if (state != null) {
            state.OnUpdate(this);
        }
    }
}
