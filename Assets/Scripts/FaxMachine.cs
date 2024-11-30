using UnityEngine;
using System.Collections;

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

    public Vector3 faxCenter;
    public bool isMakingNoise = false;

    Material[] materials;
    public GameObject[] lights = new GameObject[2];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        materials = GetComponent<Renderer>().sharedMaterials;
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
        holdPaper.transform.position = Vector3.Lerp(originPos, (-transform.up * (holdPaper.transform.localScale.y)) + faxCenter, lerpProgress); // Need to update Lerp Positions
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
        holdPaper.transform.position = Vector3.Lerp((-transform.up * (holdPaper.transform.localScale.y)) + faxCenter, faxCenter, lerpProgress); // Need to update Lerp Positions
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
            GameManager.Instance.AddScore(1);
            Debug.Log("Values Match | Success");
            successSFX.Play();
            StartCoroutine(PlayLight(2, 0.5f));
        }
        else
        {
            // Else 
            // Red light
            GameManager.Instance.DeductScore(1);
            Debug.Log("Values Doesnt Match | Failure");
            failSFX.Play();
            StartCoroutine(PlayLight(1, 0.3f));
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
            StartCoroutine(PlayLight(1, 0.3f));
            return;
        }
        originPos = holdPaper.transform.position;
        lerpProgress = 0f;
        lerpStage = LerpStage.FirstLerp;
        //GetComponent<Renderer>().material.color = Color.yellow;
        isMakingNoise = true;
    }

    public override void Deactivate()
    {
        isMakingNoise = false;
        //GetComponent<Renderer>().material.color = Color.black;
    }

    public override bool IsZoomer()
    {
        return false;
    }
    
    public override bool IsMakingNoise()
    {
        return isMakingNoise;
    }

    IEnumerator PlayLight(int lightSource, float time)
    {
        materials[lightSource].EnableKeyword("_EMISSION");
        lights[lightSource - 1].SetActive(true);
        yield return new WaitForSeconds(time);
        materials[lightSource].DisableKeyword("_EMISSION");
        lights[lightSource - 1].SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(faxCenter, 0.1f);
    }
}
