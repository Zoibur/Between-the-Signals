using System;
using UnityEditor.Rendering;
using UnityEngine;
using CameraEditorUtils = UnityEditor.CameraEditorUtils;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Moving,
        AtDesk,
        Focus,
    }
    
    public Camera _camera;
    public PlayerState state = PlayerState.Moving;
    private PlayerState prevState = PlayerState.Moving;

    [Header("Movement")]
    public float speed;
    
    private Vector2 rotation = Vector2.zero;
    private Rigidbody rb = null;
    private LayerMask interactableLayerMask;
    private LayerMask stationLayerMask;
    private GameObject focusTarget;
    private GameObject currentFocusedTarget;

    const string chairTag = "Chair";
    const string bedTag = "Bed";
    const string newsTag = "Newspaper";

    public event Action<PlayerController> OnPlayerStateChanged;

    void Awake()
    {
        interactableLayerMask = LayerMask.GetMask("Interactable");
        stationLayerMask = LayerMask.GetMask("Station");
    }
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public Interactable GetFocusInteractable()
    {
        return focusTarget.GetComponent<Interactable>();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            InteractInputCheck();
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            if (state == PlayerState.Moving && focusTarget) {
                /*
                if (focusTarget.tag == chairTag)
                {
                    state = PlayerState.AtDesk;
                    OnPlayerStateChanged?.Invoke(this);
                }
                */
                switch(focusTarget.tag)
                {
                    case chairTag:
                        state = PlayerState.AtDesk;
                        OnPlayerStateChanged?.Invoke(this);
                        break;

                    case bedTag:
                        StartCoroutine(GameManager.Instance.LoadNextDay());
                        break;

                    case newsTag:
                        focusTarget.GetComponent<NewsPaper>().Inspect();
                        state = PlayerState.Focus;
                        OnPlayerStateChanged?.Invoke(this);
                        break;
                }

            } else if (state == PlayerState.AtDesk) {
                state = PlayerState.Moving;
                OnPlayerStateChanged?.Invoke(this);

                if(currentFocusedTarget)
                {
                    DeactivateTarget();
                }
            }
            
            
        }

       
    }
    
    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, 5f, interactableLayerMask))
        {
            Debug.DrawRay(_camera.transform.position, _camera.transform.forward * hit.distance, Color.green);
            focusTarget = hit.collider.gameObject;
        }
        else if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, 5f, stationLayerMask) && state == PlayerState.AtDesk)
        {
            Debug.DrawRay(_camera.transform.position, _camera.transform.forward * hit.distance, Color.green);
            focusTarget = hit.collider.gameObject;
        }
        else
        {
            Debug.DrawRay(_camera.transform.position, _camera.transform.forward * 5f, Color.red);
            focusTarget = null;
        }

        if (state == PlayerState.Moving) {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            float angle = _camera.transform.eulerAngles.y * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle));
            
            Vector3 forward = direction;
            Vector3 right = Vector3.Cross(Vector3.up, direction);

            Vector3 movement = right * input.x + forward * input.y;
            rb.AddForce(movement * speed, ForceMode.Force);
        } else if (state == PlayerState.AtDesk) {
           
        }
    }

    private void InteractInputCheck()
    {

        if (state == PlayerState.Focus)
        {

            DeactivateTarget();
            state = prevState;
            OnPlayerStateChanged?.Invoke(this);

            return;

        }

        if (!focusTarget)
        {
            return;
        }
        if (focusTarget.tag == chairTag)
        {
            return;
        }
        currentFocusedTarget = focusTarget;
        if (focusTarget.GetComponent<Station>() != null)
        {
            focusTarget.GetComponent<Station>().Activate();
            if (!focusTarget.GetComponent<Station>().IsZoomer())
            {
                return;
            }
        }
        prevState = state;
        state = PlayerState.Focus;
        OnPlayerStateChanged?.Invoke(this);

    }

    void DeactivateTarget()
    {
        if (currentFocusedTarget.GetComponent<Station>() != null)
        {
            currentFocusedTarget.GetComponent<Station>().Deactivate();

        }
        currentFocusedTarget = null;
    }
}
