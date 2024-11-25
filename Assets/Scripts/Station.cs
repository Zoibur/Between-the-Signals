using UnityEngine;

public class Station : MonoBehaviour
{
    public enum LerpStage
    {
        None,
        FirstLerp,
        SecondLerp,
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void Activate()
    {

    }
    public virtual void Deactivate()
    {

    }
    public virtual bool IsZoomer()
    {
        return false;
    }

    public virtual bool IsMakingNoise()
    {
        return false;
    }
}
