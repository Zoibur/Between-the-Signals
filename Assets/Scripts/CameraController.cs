using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera movingCamera;
    public CinemachineVirtualCamera atDeskCamera;
    public CinemachineVirtualCamera focusCamera;

    public float atDeskRotationVertical = 90f;
    public float atDeskRotationHorizontal = 36f;
    
    private void SetMouseLocked(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

    private void ResetAtDeskCamera()
    {
        Quaternion rotation = Quaternion.Euler(atDeskRotationHorizontal, atDeskRotationVertical, 0);
        atDeskCamera.ForceCameraPosition(atDeskCamera.transform.position, rotation);
    }
    
    void Start()
    {
        ResetAtDeskCamera();
        GetComponent<PlayerController>().OnPlayerStateChanged += (PlayerController controller) =>
        {
            if (controller.state == PlayerController.PlayerState.Moving) {
                movingCamera.gameObject.SetActive(true);
                atDeskCamera.gameObject.SetActive(false);
                focusCamera.gameObject.SetActive(false);
                ResetAtDeskCamera();
            } else if (controller.state == PlayerController.PlayerState.AtDesk) {
                movingCamera.gameObject.SetActive(false);
                atDeskCamera.gameObject.SetActive(true);
                focusCamera.gameObject.SetActive(false);
            } else if (controller.state == PlayerController.PlayerState.Focus) {
                Interactable interactable = controller.GetFocusInteractable();
                focusCamera.transform.SetPositionAndRotation(interactable.GetFocusPosition(), interactable.GetFocusRotation());
                focusCamera.m_Lens.FieldOfView = interactable.focusFOV;
                
                movingCamera.gameObject.SetActive(false);
                atDeskCamera.gameObject.SetActive(false);
                focusCamera.gameObject.SetActive(true);
            }
        };
    }
}
