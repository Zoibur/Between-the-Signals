//using System.Diagnostics;
//using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


public class DeskNavigation : MonoBehaviour
{
    Camera cam;
    public GameObject[] stationObjects = new GameObject[4];

    static int pointNum = 5;
    public Vector3[] viewPos = new Vector3[pointNum]; // Position that the camera will look towards // 0 = overall Desk View
    public Vector3[] cameraPos = new Vector3[pointNum]; // Position that the camera will locate // 0 = overall Desk View

    public bool inNavigationMode = true;
    public int currentIndex = 0;

    public float transitionSpeed = 2.0f;



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
        Control();
    }

    void Control()
    {
        if(Input.GetKeyDown("a"))
        {
            if(currentIndex == 0)
            {
                currentIndex = pointNum;
            }
            currentIndex--;
            SwitchStation();
        }
        if (Input.GetKeyDown("d"))
        {
            currentIndex++;
            if (currentIndex == pointNum)
            {
                currentIndex = 0;
            }
            SwitchStation();
        }
    }

    void SwitchStation()
    {
        // Set Camera to new pos
        cam.transform.position = cameraPos[currentIndex];

        // Set Rotation on camera to look towards new pos
        Vector3 relativePos = viewPos[currentIndex] - cameraPos[currentIndex];
        cam.transform.rotation = Quaternion.LookRotation(relativePos, Vector3.up);

        for(int i = 0; i < 4; i++)
        {
            stationObjects[i].GetComponent<Renderer>().material.color = new Color(0, 0, 0);
        }
        if (currentIndex != 0)
        {
            stationObjects[currentIndex-1].GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        }
    }

    public void EnterDesk()
    {
        // Enable desk control
        // Disable player movement control
        inNavigationMode = true;
        currentIndex = 0;
        SwitchStation();
    }

    public void LeaveDesk()
    {
        inNavigationMode = false;
        // Disable desk control
        // Enable player movement control
    }

  

    void OnDrawGizmos()
    {
        // Draw View Pos 
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
