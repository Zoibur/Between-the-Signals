//using System.Diagnostics;
//using System.Runtime.InteropServices.WindowsRuntime;
//using System.Collections.Specialized;
//using System.Threading;
using UnityEngine;


public class DeskNavigation : MonoBehaviour
{
    Camera cam;
    public bool inNavigationMode = true;

    public GameObject[] stationObjects = new GameObject[4];
    
    // Variables for Camera Position and Rotation
    static int pointNum = 5;
    public Vector3[] viewPos = new Vector3[pointNum]; // Position that the camera will look towards // 0 = overall Desk View
    public Vector3[] cameraPos = new Vector3[pointNum]; // Position that the camera will locate // 0 = overall Desk View
    public int currentIndex = 0;


    // Variables for transitions
    public float transitionSpeed = 1.0f;
    float transitionProgress = 0f;
    Vector3 previousPos = new Vector3(0f, 0f, 0f);
    Quaternion previousRot = new Quaternion(0, 0, 0, 0);
    Quaternion nextRot = new Quaternion(0, 0, 0, 0);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        for (int i = 0; i < 4; i++)
        {
            stationObjects[i].GetComponent<Renderer>().material.color = new Color(0, 0, 0);
        }
        EnterDesk();
    }

    // Update is called once per frame
    void Update()
    {
        if(!inNavigationMode)
        {
            return;
        }
        if (transitionProgress > 0f)
        {
            UpdateStationTransition();
            return;
        }
        Control();
    }

    // Method for Keyboard Control
    void Control()
    {
        if(Input.GetKeyDown("a"))
        {
            if(currentIndex == 0)
            {
                currentIndex = pointNum;
            }
            currentIndex--;
            EnterStationTransition();
        }
        if (Input.GetKeyDown("d"))
        {
            currentIndex++;
            if (currentIndex == pointNum)
            {
                currentIndex = 0;
            }
            EnterStationTransition();
        }
    }

    // Method for Initating a transition between 2 camera spots 
    void EnterStationTransition()
    {
        // Start progress
        transitionProgress = Time.deltaTime;

        // Set Position variables needed for the Lerp
        previousPos = cam.transform.position;

        // Set Rotation variables needed for the Lerp
        Vector3 relativePos = viewPos[currentIndex] - cameraPos[currentIndex];
        nextRot = Quaternion.LookRotation(relativePos, Vector3.up);
        previousRot = cam.transform.rotation;

        // Highlight station
        for(int i = 0; i < 4; i++)
        {
            stationObjects[i].GetComponent<Renderer>().material.color = new Color(0, 0, 0);
        }
        if (currentIndex != 0)
        {
            stationObjects[currentIndex-1].GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        }
    }

    // Method for Lerping the transition between 2 camera spots
    void UpdateStationTransition()
    {
        transitionProgress += Time.deltaTime;

        cam.transform.position = Vector3.Lerp(previousPos, cameraPos[currentIndex], transitionProgress);
        cam.transform.rotation = Quaternion.Lerp(previousRot, nextRot, transitionProgress);

        if (transitionProgress > 1f)
        {
            transitionProgress = 0f;
        }
    }

    public void EnterDesk()
    {
        // Enable desk control
        inNavigationMode = true;
        currentIndex = 0;
        EnterStationTransition();

        // Disable player movement control



    }

    public void LeaveDesk()
    {
        // Disable desk control
        inNavigationMode = false;

        // Enable player movement control


    }

  

    void OnDrawGizmos()
    {
        // Draw View Points 
        float radius = 0.1f; 
       
        for(int i = 0; i < pointNum; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(viewPos[i], radius);
            Gizmos.DrawLine(viewPos[i], cameraPos[i]);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(cameraPos[i], radius);
        }
    }
}
