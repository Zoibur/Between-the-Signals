using UnityEngine;

public class Printer : MonoBehaviour
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        GetComponent<Renderer>().material.color = Color.yellow; 
    }
}
