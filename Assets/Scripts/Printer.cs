using UnityEngine;

public class Printer : Station
{
    public AudioSource printingSFX;
    public AudioSource failPrintSFX;

    LerpStage lerpStage = LerpStage.None;

    bool active = false;
    const float FAIL_TIME = 2f;
    const float PRINT_TIME = 6f;
    float timer = 0f;
    bool successPrint = false;
    public Radio radio;
    public Decoder decoder;

    public GameObject paperPrefab;
    private GameObject holdPaper = null;

    //bool lerping = false;
    float lerpProgress = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lerpStage != LerpStage.None)
        {
            if(lerpStage == LerpStage.FirstLerp)
            {
                LerpPaperFirstHalf();
            }
            else
            {
                LerpPaperSecondHalf();
            }
        }
        if(!active)
        {
            return;
        }
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            Print();
        }
    }

    void LerpPaperFirstHalf()
    {
        lerpProgress += Time.deltaTime;
        holdPaper.transform.position = Vector3.Lerp(transform.position, (-transform.forward * (holdPaper.transform.localScale.x / 2f)) + transform.position, lerpProgress);

        if(lerpProgress > 1f)
        {
            lerpStage = LerpStage.None;
            //lerping = false;
        }
    }

    void LerpPaperSecondHalf()
    {
        lerpProgress += Time.deltaTime;
        holdPaper.transform.position = Vector3.Lerp((-transform.forward * (holdPaper.transform.localScale.x / 2f)) + transform.position, (-transform.forward * (holdPaper.transform.localScale.x)) + transform.position, lerpProgress);

        if (lerpProgress > 1f)
        {
            Debug.Log("Took Paper");
            decoder.SetPaper(holdPaper);
            holdPaper = null;
            lerpStage = LerpStage.None;
            Deactivate();
        }
    }

    public void Print()
    {
        if (!successPrint) 
        {
            FailPrint();
            return;
        }
        printingSFX.Stop();
        Debug.Log("Success, Bogos Binted");
        // Instantiate Paper
        holdPaper = Instantiate(paperPrefab);
        holdPaper.transform.position = transform.position;
        holdPaper.transform.rotation = transform.rotation;
        holdPaper.transform.Rotate(0f, 90f, 0f);

        float yChange = holdPaper.transform.localScale.y;
        holdPaper.transform.position = new Vector3(holdPaper.transform.position.x, holdPaper.transform.position.y + yChange, holdPaper.transform.position.z);

        GetComponent<Renderer>().material.color = Color.green;

        active = false;
        lerpStage = LerpStage.FirstLerp;
        lerpProgress = 0f;

        radio.NewRadioValues();
    }
    public void FailPrint()
    {
        // Fail Feedback
        Debug.Log("Failure, Printer Explodes");
        failPrintSFX.Play();
        Deactivate();
    }

    public override void Activate()
    {
        if (active)
        {
            return;
        }
        if (holdPaper)
        {
            lerpStage = LerpStage.SecondLerp;
            lerpProgress = 0f;
            /*
            Debug.Log("Took Paper");
            decoder.SetPaper(holdPaper);
            holdPaper = null;
            lerpStage = LerpStage.None;
            Deactivate();
            */
            return;
        }
        active = true;
        //successPrint = radio.IsValuesCorrect();
        successPrint = (radio.IsAmplitudeInRange() && radio.IsFrequencyInRange()) ? true : false;
        timer = (successPrint) ? PRINT_TIME : FAIL_TIME;
        GetComponent<Renderer>().material.color = (successPrint) ? Color.yellow : Color.red;
        if (successPrint)
        {
            printingSFX.Play();
        }
    }

    public override void Deactivate()
    {
        //Debug.Log("Deactivate");
        GetComponent<Renderer>().material.color = Color.black;
        active = false;
    }

    public override bool IsZoomer()
    {
        return false;
    }
}