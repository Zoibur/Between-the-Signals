using UnityEngine;

public class FaxMachine : Station
{
    public AudioSource successSFX;
    public AudioSource workingSFX;
    public AudioSource failSFX;

    GameObject holdPaper;
    LerpStage lerpStage = LerpStage.None;
    float lerpProgress = 0f;
    Vector3 originPos;
    public Decoder decoder;
    const float CALCULATE_TIME = 3f;
    float calculateTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (calculateTimer > 0f)
        {
            calculateTimer -= Time.deltaTime;
            if (calculateTimer <= 0f)
            {
                CalculatePaperResults();   
            }
        }
        if(lerpStage == LerpStage.FirstLerp)
        {
            LerpPaperFirstHalf();
        }
        else if (lerpStage == LerpStage.SecondLerp)
        {
            LerpPaperSecondHalf();
        }
    }

    void LerpPaperFirstHalf()
    {
        lerpProgress += Time.deltaTime;
        holdPaper.transform.position = Vector3.Lerp(originPos, (-transform.forward * (holdPaper.transform.localScale.y)) + transform.position, lerpProgress); // Need to update Lerp Positions
        Vector3 startRot = new Vector3(90f, 90f, 0f);
        Vector3 endRot = new Vector3(90f, 90f, 0f);
        holdPaper.transform.rotation = Quaternion.Lerp(Quaternion.Euler(startRot), Quaternion.Euler(endRot), lerpProgress);
        if (lerpProgress > 1f)
        {
            lerpProgress = 0f;
            lerpStage = LerpStage.SecondLerp;
        }
    }
    void LerpPaperSecondHalf()
    {
        lerpProgress += Time.deltaTime;
        holdPaper.transform.position = Vector3.Lerp((-transform.forward * (holdPaper.transform.localScale.y)) + transform.position, transform.position, lerpProgress); // Need to update Lerp Positions
        //holdPaper.transform.rotation = Quaternion.Lerp(originRot, originRot, lerpProgress);
        if (lerpProgress > 1f)
        {           
            calculateTimer = CALCULATE_TIME;
            lerpStage = LerpStage.None;
            workingSFX.Play();
        }
    }
    void CalculatePaperResults()
    {
        workingSFX.Stop();

        string targetMessage = holdPaper.GetComponent<Paper>().GetTargetMessage();
        string inputMessage = holdPaper.GetComponent<Paper>().GetInputMessage();

        if(targetMessage == inputMessage)
        {
            // If values are correct
            // Green light
            Debug.Log("Values Match | Success");
            successSFX.Play();
        }
        else
        {
            // Else 
            // Red light
            Debug.Log("Values Doesnt Match | Failure");
            failSFX.Play();
        }
        Debug.Log("The Correct Message: " + targetMessage + " | Player Input: " + inputMessage);

        Destroy(holdPaper);
        Deactivate();
    }
    public override void Activate()
    {
        if(holdPaper)
        {
            return;
        }
        holdPaper = decoder.TakePaper();
        if (!holdPaper)
        {
            failSFX.Play();
            return;
        }
        originPos = holdPaper.transform.position;
        lerpProgress = 0f;
        lerpStage = LerpStage.FirstLerp;
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
