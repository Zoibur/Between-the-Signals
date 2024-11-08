using UnityEngine;

public class FaxMachine : Station
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Activate()
    {
        GetComponent<Renderer>().material.color = Color.yellow;
    }

    public override void Deactivate()
    {
        GetComponent<Renderer>().material.color = Color.black;
    }

    public override bool IsZoomer()
    {
        return false;
    }
}
