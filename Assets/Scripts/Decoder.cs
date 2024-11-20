using UnityEngine;
using static Radio;

public class Decoder : Station
{
    public GameObject morseBook;
    LayerMask decodeToolLayer;
    Camera _camera;
    bool active = false;

    // Paper Lerping
    bool lerping = false;
    float lerpProgress = 0f;
    GameObject holdPaper = null;
    Vector3 originPos;
    Quaternion originRot;
    public Vector3 targetPos;

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        decodeToolLayer = LayerMask.GetMask("DecodeTool");
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (lerping)
        {
            LerpPaper();
        }
        if(!active)
        {
            return;
        }
        if(Input.GetMouseButtonDown(0))
        {
            RaycastToTool();
        }
    }

    public void SetPaper(GameObject paper)
    {
        holdPaper = paper;
        lerping = true;
        lerpProgress = 0f;
        originPos = paper.transform.position;
        originRot = paper.transform.rotation;
    }

    public GameObject TakePaper()
    {
        GameObject temp = holdPaper;
        holdPaper = null;
        return temp;
    }

    void LerpPaper()
    {
        lerpProgress += Time.deltaTime;
        holdPaper.transform.position = Vector3.Lerp(originPos, targetPos, lerpProgress);//(lerpStage == LerpStage.FirstLerp) ? Vector3.Lerp(originPos, targetPos, lerpProgress) : Vector3.Lerp(originPos, targetPos, lerpProgress);
        holdPaper.transform.rotation = Quaternion.Lerp(originRot, transform.rotation, lerpProgress);//(lerpStage == LerpStage.FirstLerp) ? Quaternion.Lerp(originRot, transform.rotation, lerpProgress) : Quaternion.Lerp(originRot, transform.rotation, lerpProgress);
        if (lerpProgress > 1f)
        {
            lerping = false;
            /*
            lerpStage = (lerpStage == LerpStage.FirstLerp) ? LerpStage.SecondLerp : LerpStage.Done;
            if (lerpStage == LerpStage.SecondLerp)
            {
                lerpProgress = 0f;
            }
            else
            {
               
            }
            */
        }
    }

    public override void Activate()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        active = true;
    }
    public override void Deactivate()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        active = false;
    }
    public override bool IsZoomer()
    {
        return true;
    }

    void RaycastToTool()
    {
        
        Vector3 testMousePos = Input.mousePosition;
        testMousePos.z = 100f;
        testMousePos = _camera.ScreenToWorldPoint(testMousePos);

        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, testMousePos - _camera.transform.position, out hit, 5f, decodeToolLayer))
        {
            if(hit.transform == morseBook.transform)
            {
                morseBook.GetComponent<Renderer>().material.color = Color.red;
                Debug.DrawRay(_camera.transform.position, testMousePos - _camera.transform.position, Color.green);
            } 
            if(!holdPaper)
            {
                Debug.DrawRay(_camera.transform.position, testMousePos - _camera.transform.position, Color.red);
                return;
            }
            else if (hit.transform == holdPaper.transform)
            {
                holdPaper.GetComponent<Renderer>().material.color = Color.red;
                Debug.DrawRay(_camera.transform.position, testMousePos - _camera.transform.position, Color.green);
            }
            else
            {
                Debug.DrawRay(_camera.transform.position, testMousePos - _camera.transform.position, Color.red);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(targetPos, 0.1f);
    }
}
