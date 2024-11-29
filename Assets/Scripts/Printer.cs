using UnityEngine;

public class Printer : Station
{
    MessageList messageList = new MessageList();

    public AudioSource successPrintSFX;
    public AudioSource workingSFX;
    public AudioSource failPrintSFX;
    public AudioSource buttonSFX;

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

    public Vector3 printerCenter;

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
        holdPaper.transform.position = Vector3.Lerp(printerCenter, (transform.forward * (holdPaper.transform.localScale.y / 2f)) + printerCenter, lerpProgress);

        if(lerpProgress > 1f)
        {
            lerpStage = LerpStage.None;
            successPrintSFX.Stop();
            //lerping = false;
        }
    }

    void LerpPaperSecondHalf()
    {
        lerpProgress += Time.deltaTime;
        holdPaper.transform.position = Vector3.Lerp((transform.forward * (holdPaper.transform.localScale.y / 2f)) + printerCenter, (transform.forward * (holdPaper.transform.localScale.y)) + printerCenter, lerpProgress);

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
        workingSFX.Stop();
        successPrintSFX.Play();
        Debug.Log("Success, Bogos Binted");
        // Instantiate Paper
        holdPaper = Instantiate(paperPrefab);
        holdPaper.transform.position = transform.position;
        holdPaper.transform.rotation = transform.rotation;
        holdPaper.transform.Rotate(90f, 0f, 0f);

        if(radio.GetIsMorse())
        {
            CreateMorseCodePaper();
        }
        else
        {
            CreateSecretCodePaper();
        }
        /*
        int randNum = Random.Range(0, 2);
        switch(randNum)
        {
            case 0:
                CreateMorseCodePaper();
                break;
            case 1:
                CreateSecretCodePaper();
                break;
        }*/
       
        //GetComponent<Renderer>().material.color = Color.green;

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
        workingSFX.Stop();
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
            if(decoder.HasPaper())
            {
                return;
            }
            lerpStage = LerpStage.SecondLerp;
            lerpProgress = 0f;
            successPrintSFX.Stop();
            /*
            Debug.Log("Took Paper");
            decoder.SetPaper(holdPaper);
            holdPaper = null;
            lerpStage = LerpStage.None;
            Deactivate();
            */
            return;
        }
        buttonSFX.Play();
        active = true;
        //successPrint = radio.IsValuesCorrect();
        successPrint = (radio.IsAmplitudeInRange() && radio.IsFrequencyInRange());
        //successPrint = true;
       // Debug.Log("Amplitude In Range: " + radio.IsAmplitudeInRange() + " | Frequency In Range: " + radio.IsFrequencyInRange());
        timer = (successPrint) ? PRINT_TIME : FAIL_TIME;
        //GetComponent<Renderer>().material.color = (successPrint) ? Color.yellow : Color.red;
      
        workingSFX.Play();
        
    }

    public override void Deactivate()
    {
        //Debug.Log("Deactivate");
        //GetComponent<Renderer>().material.color = Color.black;
        active = false;
    }

    public override bool IsZoomer()
    {
        return false;
    }
    
    public override bool IsMakingNoise()
    {
        return active;
    }

    void CreateMorseCodePaper()
    {
        // Paper Texts
        Paper paperScript = holdPaper.GetComponent<Paper>();
        string secretMessage = messageList.GetRandomMorseCodeMessage();
        paperScript.SetTargetMessage(secretMessage);
        paperScript.SetInputMessage(messageList.GenerateBlank(secretMessage));
        secretMessage = messageList.GenerateMorseCode(secretMessage);
        paperScript.SetCodeMessage(secretMessage);

        float yChange = holdPaper.transform.localScale.y;
        holdPaper.transform.position = new Vector3(holdPaper.transform.position.x, holdPaper.transform.position.y + yChange, holdPaper.transform.position.z);

    }
    void CreateSecretCodePaper()
    {
        // Paper Texts
        Paper paperScript = holdPaper.GetComponent<Paper>();
        string secretMessage = messageList.GetRandomSecretMessage();
        paperScript.SetTargetMessage(secretMessage);
        paperScript.SetInputMessage(messageList.GenerateBlank(secretMessage));
        secretMessage = messageList.GenerateSecretMessage(secretMessage);
        paperScript.SetCodeMessage(secretMessage);

        float yChange = holdPaper.transform.localScale.y;
        holdPaper.transform.position = new Vector3(holdPaper.transform.position.x, holdPaper.transform.position.y + yChange, holdPaper.transform.position.z);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(printerCenter, 0.1f);
    }
}
