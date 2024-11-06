using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Camera Settings")]
    public Vector3 focusFrom;
    public Vector3 focusAt;
    public float focusFOV;

    public Vector3 GetFocusPosition()
    {
        return transform.TransformPoint(focusFrom);
    }

    public Quaternion GetFocusRotation()
    {
        Vector3 origin = transform.TransformPoint(focusFrom);
        Vector3 center = transform.TransformPoint(focusAt);
        Vector3 direction = (center - origin).normalized;
        return Quaternion.LookRotation(direction);
    }
    
    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.TransformPoint(focusAt), 0.01f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.TransformPoint(focusFrom), 0.01f);

        Matrix4x4 original = Gizmos.matrix;

        Vector3 origin = transform.TransformPoint(focusFrom);
        Vector3 center = transform.TransformPoint(focusAt);
        
        Vector3 direction = (center - origin).normalized;
        Gizmos.matrix = Matrix4x4.TRS(origin, Quaternion.LookRotation(direction), Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, focusFOV, Vector3.Distance(origin, center), 0.01f, Camera.main.aspect);
        
        Gizmos.matrix = original;
#endif
    }
}
