using UnityEngine;

public class Decoder : Station
{

    bool lerping = false;
    float lerpProgress = 0f;
    GameObject holdPaper = null;
    Vector3 originPos;
    Quaternion originRot;
    public Vector3 targetPos;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lerping)
        {
            LerpPaper();
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
    }
    public override void Deactivate()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public override bool IsZoomer()
    {
        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(targetPos, 0.1f);
    }
}
