using UnityEngine;

public class Radio : Station
{
    float correctAmp = 0f;
    float correctFreq = 0f;

    float currentAmp = 0f;
    float currentFreq = 0f;

    float errorMargin = 0.1f;
    float changeValueAmount = 0.1f;

    public GameObject uiButtons;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiButtons.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Function for Printer Script to Call when starting to Print paper
    public bool IsValuesCorrect()
    {
        Debug.Log("Current Amp: " + currentAmp.ToString() + " Target Amp: " + correctAmp.ToString() + "Current Freq:" + currentFreq.ToString() + " Target Freq: " + correctFreq.ToString()); 
        if (Mathf.Abs(currentAmp - correctAmp) > errorMargin)
        {
            Debug.Log("Print Will Fail");
            return false;
        }
        if (Mathf.Abs(currentFreq - correctFreq) > errorMargin)
        {
            Debug.Log("Print Will Fail");
            return false;
        }
        Debug.Log("Print Will Success");
        return true;
    }

    public override void Activate()
    {
        uiButtons.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        NewRadioValues();

    }
    public override void Deactivate()
    {
        uiButtons.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override bool IsZoomer()
    {
        return true;
    }

    public void NewRadioValues()
    {
        correctAmp = Random.Range(0f, 1f);
        correctFreq = Random.Range(0f, 1f);
    }

    public void ChangeAmplitude(bool raise)
    {
        currentAmp += (raise) ? changeValueAmount : -changeValueAmount;
        currentAmp = Mathf.Clamp(currentAmp, 0f, 1f);
        Debug.Log("Current Amp: " + currentAmp.ToString() + " Target Amp: " + correctAmp.ToString());
    }

    public void ChangeFrequency(bool raise)
    {
        currentFreq += (raise) ? changeValueAmount : -changeValueAmount;
        currentFreq = Mathf.Clamp(currentFreq, 0f, 1f);
        Debug.Log("Current Freq: " + currentFreq.ToString() + " Target Freq: " + correctFreq.ToString());
    }

   

}
