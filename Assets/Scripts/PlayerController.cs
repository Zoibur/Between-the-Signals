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
    
    [Header("Movement")]
    public float speed;
    
    private Vector2 rotation = Vector2.zero;
    private Rigidbody rb = null;
    private LayerMask interactableLayerMask;
    private GameObject focusTarget;
    
    public event Action<PlayerController> OnPlayerStateChanged;

    void Awake()
    {
        interactableLayerMask = LayerMask.GetMask("Interactable");
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
        if (Input.GetKeyDown(KeyCode.E)) {
            if (state == PlayerState.Moving) {
                state = PlayerState.AtDesk;
            } else if (state == PlayerState.AtDesk || state == PlayerState.Focus) {
                state = PlayerState.Moving;
            }
            
            OnPlayerStateChanged?.Invoke(this);
        }

        if (state == PlayerState.AtDesk) {
            if (Input.GetMouseButtonDown(1) && focusTarget) {
                state = PlayerState.Focus;
                OnPlayerStateChanged?.Invoke(this);
            }
        } else if (state == PlayerState.Focus) {
            if (Input.GetMouseButtonDown(1)) {
                state = PlayerState.AtDesk;
                OnPlayerStateChanged?.Invoke(this);
            }
        }
    }
    
    void FixedUpdate()
    {
        if (state == PlayerState.Moving) {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            float angle = _camera.transform.eulerAngles.y * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle));
            
            Vector3 forward = direction;
            Vector3 right = Vector3.Cross(Vector3.up, direction);

            Vector3 movement = right * input.x + forward * input.y;
            rb.AddForce(movement * speed, ForceMode.Force);
        } else if (state == PlayerState.AtDesk) {
            RaycastHit hit;
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, 5f, interactableLayerMask)) {
                Debug.DrawRay(_camera.transform.position, _camera.transform.forward * hit.distance, Color.green);
                focusTarget = hit.collider.gameObject;
            } else {
                Debug.DrawRay(_camera.transform.position, _camera.transform.forward * 5f, Color.red);
                focusTarget = null;
            }
        }
    }
}
