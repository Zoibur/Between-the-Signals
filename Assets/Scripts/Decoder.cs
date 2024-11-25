using UnityEngine;
using TMPro;
using static Radio;
using UnityEngine.UI;

public class Decoder : Station
{
    public TMP_InputField inputField;
    public GameObject morseBook;
    LayerMask decodeToolLayer;
    Camera _camera;
    bool active = false;
    Vector3 bookOriginPos;
    public float itemDist = 0.3f;
    GameObject holdTool;
    Vector3 toolOriginPos; 

    // Paper Lerping
    bool lerping = false;
    float lerpProgress = 0f;
    GameObject holdPaper = null;
    Vector3 originPos;
    Quaternion originRot;
    public Vector3 targetPos;
    Quaternion targetRot;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        decodeToolLayer = LayerMask.GetMask("DecodeTool");
        _camera = Camera.main;
        bookOriginPos = morseBook.transform.position;
        targetRot = Quaternion.Euler(90f, 90f, 0f);
  
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
            if (holdTool)
            {
                if(holdTool == holdPaper)
                {
                    //holdPaper.GetComponent<Paper>().SetInputMessage(inputField.text.ToString());
                    inputField.DeactivateInputField();
                    inputField.gameObject.SetActive(false);
                }
                else if (holdTool == morseBook)
                {
                    morseBook.GetComponent<Book>().Deactivate();
                }
                holdTool.transform.position = toolOriginPos;
                holdTool = null;   
            }
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
        holdPaper.transform.rotation = Quaternion.Lerp(originRot, targetRot, lerpProgress);//(lerpStage == LerpStage.FirstLerp) ? Quaternion.Lerp(originRot, transform.rotation, lerpProgress) : Quaternion.Lerp(originRot, transform.rotation, lerpProgress);
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
        if (holdTool)
        {
            holdTool.transform.position = toolOriginPos;
            holdTool = null;
        }
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
            holdTool = hit.transform.gameObject;
            toolOriginPos = holdTool.transform.position;
            holdTool.transform.position = _camera.transform.position + (_camera.transform.forward * 0.5f);
            if (holdTool == holdPaper)
            {
                Debug.Log("Tool is Paper");
                inputField.gameObject.SetActive(true);
                inputField.ActivateInputField();
            }
            else if (holdTool == morseBook)
            {
                morseBook.GetComponent<Book>().Activate();
            }
            /*
            if(hit.transform == morseBook.transform)
            {
                morseBook.GetComponent<Renderer>().material.color = Color.red;
                Debug.DrawRay(_camera.transform.position, testMousePos - _camera.transform.position, Color.green);
                morseBook.transform.position = _camera.transform.position + (_camera.transform.forward * itemDist);//_camera.transform.forward * itemDist;
                //morseBook.transform.LookAt(_camera.transform.position); 
                holdingTool = true;
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
                holdingTool = true;
            }
            else
            {
                Debug.DrawRay(_camera.transform.position, testMousePos - _camera.transform.position, Color.red);
            }
            */
        }
    }
    public void UpdateInputText()
    {
        if (Input.GetKeyDown("space"))
        {
            return;
        }
        if(Input.GetKeyDown("backspace"))
        {
            holdPaper.GetComponent<Paper>().RemoveFromInputMessage();
            return;
        }
        // int indexChange = (Input.GetKeyDown("backspace")) ? -1 : 1;
        string input = inputField.text;
        //Debug.Log("Full Input: " + input);
        holdPaper.GetComponent<Paper>().AddToInputMessage(input[input.Length -1]);
        //Debug.Log("Paper Function Ended");
      
        //Debug.Log("Ending Input Function");
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(targetPos, 0.1f);
    }
}
