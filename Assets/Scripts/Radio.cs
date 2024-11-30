//using System;
//using System.Runtime.CompilerServices;
//using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using UnityEngine;
//using UnityEngine.TerrainUtils;

public class Radio : Station
{
    public enum KnobMode
    {
        None,
        AmplitudeOn,
        FrequencyOn,
    }

    public AudioSource noiseSFX;
    public AudioSource messageSFX;
    public AudioSource buttonSFX;

    public AudioClip morseClip;
    public AudioClip mumblingClip;

    bool isRadioOn = false;

    bool active = false;

    float targetAmp = 0f;
    float targetFreq = 0f;

    float currentAmp = 0f;
    float currentFreq = 0f;

    float errorMargin = 5f;

    float changeAmount = 10f;

    bool isMorse = false;

    public GameObject[] interactUI = new GameObject[3];
    //float changeAmpValueAmount = 5f;
    //float changeFreqValueAmount = 5f;

    GameObject focusTarget;

    public GameObject[] lightButtons = new GameObject[2];

    //public GameObject uiButtons;

    [Header("Knobs")]
    Camera _camera;
    LayerMask knobLayer;
    public KnobMode mode = KnobMode.None;
    //Vector3 mousePos = Vector3.zero;
    Transform knobTarget;
    float beforeValue = 0f;

    // Wave variables
    [Header("Sinus Wave")]
    public LineRenderer currentLineRenderer;
    //public LineRenderer targetLineRenderer;
    public int points = 60;
    public Vector3 xLimits = new Vector3(0f, 1f, 0f);
    public float waveEndX = 0.8f;
    public float movementSpeed = 6;

    //const float MIN_AMPLITUDE = 0.01f;
    const float MAX_AMPLITUDE = 0.03f;

    //const float MIN_FREQUENCY = 10f;
    //const float MAX_FREQUENCY = 60f;

    const float MAX_VALUE = 60f;
    const float MIN_VALUE = 10f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLineRenderer.enabled = true;
        xLimits = new Vector3(0f, 0.5f, 0f);

        currentLineRenderer.startColor = Color.yellow;
        currentLineRenderer.endColor = Color.yellow;
        float lineThickness = 0.005f;
        currentLineRenderer.SetWidth(lineThickness, lineThickness);
        currentLineRenderer.enabled = false;
        //targetLineRenderer.startColor = Color.black;
        //targetLineRenderer.endColor = Color.black;

        knobLayer = LayerMask.GetMask("Knob");
        _camera = Camera.main;

        NewRadioValues();
    }

    // Update is called once per frame
    void Update()
    {
        if(isRadioOn)
        {
            UpdateRadioWaves(currentAmp, currentFreq, currentLineRenderer);
            CheckLightButtons();
        }
        //UpdateRadioWaves(targetAmp, targetFreq, targetLineRenderer);

        if (!active)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            SelectRaycastedObject();
           // RaycastToKnob();
        }
        if(mode == KnobMode.AmplitudeOn)
        {
            UpdateKnobValues(ref currentAmp);
            UpdateSoundValues();
        } else if (mode == KnobMode.FrequencyOn) {
            UpdateKnobValues(ref currentFreq);
            UpdateSoundValues();
        }
       
       
    }

    /*
    // Function for Printer Script to Call when starting to Print paper
    public bool IsValuesCorrect()
    {
        //Debug.Log("Current Amp: " + currentAmp.ToString() + " Target Amp: " + targetAmp.ToString() + "Current Freq:" + currentFreq.ToString() + " Target Freq: " + targetFreq.ToString()); 
        if (Mathf.Abs(currentAmp - targetAmp) > errorMargin)
        {
            Debug.Log("Print Will Fail");
            return false;
        }
        if (Mathf.Abs(currentFreq - targetFreq) > errorMargin)
        {
            Debug.Log("Print Will Fail");
            return false;
        }
        Debug.Log("Print Will Success");
        return true;
    }
    */
    public bool IsAmplitudeInRange()
    {
       // Debug.Log("Amplitude Values | Current Amp: " + currentAmp.ToString() + " | Target Amp: " + targetAmp.ToString());
        if (Mathf.Abs(currentAmp - targetAmp) > errorMargin)
        {
            return false;
        }
        return true;
    }

    public bool IsFrequencyInRange()
    {
        //Debug.Log("Frequency Values | Current Freq: " + currentFreq.ToString() + " | Target Freq: " + targetFreq.ToString());
        if (Mathf.Abs(currentFreq - targetFreq) > errorMargin)
        {
            return false;
        }
        return true;
    }
    public override void Activate()
    {
        active = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public override void Deactivate()
    {
        active = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if(knobTarget)
        {
            knobTarget.GetComponent<Renderer>().material.color = Color.white;
            interactUI[1].SetActive(false);
        }
        interactUI[0].SetActive(false);
        interactUI[1].SetActive(false);
        interactUI[2].SetActive(false);
    }

    public override bool IsZoomer()
    {
        return true;
    }

    public override bool IsMakingNoise()
    {
        return isRadioOn;
    }

    void CheckLightButtons()
    {
        if (IsAmplitudeInRange())
        {
            lightButtons[0].SetActive(true);
        }
        else { 
            lightButtons[0].SetActive(false); 
        }
        if (IsFrequencyInRange())
        {
            lightButtons[1].SetActive(true);
        }
        else { 
            lightButtons[1].SetActive(false); 
        }
    }

    public void NewRadioValues()
    {
        //targetAmp = Random.Range(MIN_AMPLITUDE, MAX_AMPLITUDE);
        //targetFreq = Random.Range(MIN_FREQUENCY, MAX_FREQUENCY);

        //currentAmp = Random.Range(MIN_AMPLITUDE, MAX_AMPLITUDE);
        //currentFreq = Random.Range(MIN_FREQUENCY, MAX_FREQUENCY);
        do
        {
            targetAmp = Random.Range(MIN_VALUE, MAX_VALUE);
            targetFreq = Random.Range(MIN_VALUE, MAX_VALUE);

            currentAmp = Random.Range(MIN_VALUE, MAX_VALUE);
            currentFreq = Random.Range(MIN_VALUE, MAX_VALUE);

        } while (IsAmplitudeInRange() || IsFrequencyInRange());

        int randNum = Random.Range(1, 101);
        isMorse = (randNum > 65);

        messageSFX.clip = (isMorse) ? morseClip : mumblingClip;

        UpdateSoundValues();
    }

    public bool GetIsMorse()
    {
        return isMorse;
    }
    void ToggleRadio()
    {
        buttonSFX.Play();

        //AudioManager.instance.PlaySoundFXClip(buttonSFX, transform, 1f);
      
        isRadioOn = !isRadioOn;
        if (isRadioOn) {
            UpdateSoundValues();
            currentLineRenderer.enabled = true;
        }
        else
        {
            lightButtons[0].SetActive(false);
            lightButtons[1].SetActive(false);
            noiseSFX.Stop();
            currentLineRenderer.enabled = false;

            messageSFX.Stop();
        }
    }


    void SelectRaycastedObject()
    {
        if (knobTarget)
        {

            knobTarget.GetComponent<Renderer>().material.color = Color.white;
            interactUI[1].SetActive(false);
            interactUI[2].SetActive(false);
            knobTarget = null;
            mode = KnobMode.None;
            
        }
        if(!focusTarget)
        {
            return;
        }
        if (focusTarget.transform.tag == "Button")
        {
            ToggleRadio();
            return;
        }
        knobTarget = focusTarget.transform;
        mode = (knobTarget.tag == "AmplitudeKnob") ? KnobMode.AmplitudeOn : KnobMode.FrequencyOn;
        knobTarget.GetComponent<Renderer>().material.color = Color.red;
        
        interactUI[0].SetActive(false);
        //beforeValue = (mode == KnobMode.AmplitudeOn) ? currentAmp : currentFreq;
        if(mode == KnobMode.AmplitudeOn)
        {
            interactUI[1].SetActive(true);
            beforeValue = currentAmp;
        }
        else
        {
            interactUI[2].SetActive(true);
            beforeValue = currentFreq;
        }
        // Other stuff
    }
    private void FixedUpdate()
    {
        if (!active)
        {
            return;
        }
        // Raycast
        // gameobject for focused Target

        Vector3 testMousePos = Input.mousePosition;
        testMousePos.z = 100f;
        testMousePos = _camera.ScreenToWorldPoint(testMousePos);

        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, testMousePos - _camera.transform.position, out hit, 5f, knobLayer))
        {
            focusTarget = hit.transform.gameObject;
            interactUI[0].SetActive(true);
        }
        else
        {
            focusTarget = null;
            interactUI[0].SetActive(false);
        }
    }

    /*
    void RaycastToKnob()
    {
        if(knobTarget)
        {
           
            knobTarget.GetComponent<Renderer>().material.color = Color.white;
            knobTarget = null;
            mode = KnobMode.None;
        }
        Vector3 testMousePos = Input.mousePosition;
        testMousePos.z = 100f;
        testMousePos = _camera.ScreenToWorldPoint(testMousePos);

        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, testMousePos - _camera.transform.position, out hit, 5f, knobLayer))
        {
            if(hit.transform.tag == "Button")
            {
                ToggleRadio();
                return;
            }
            knobTarget = hit.transform;
            mode = (knobTarget.tag == "AmplitudeKnob") ? KnobMode.AmplitudeOn : KnobMode.FrequencyOn;
            //mousePos = hit.point;
            //mousePos.z = knobTarget.position.z;
            knobTarget.GetComponent<Renderer>().material.color = Color.red;
            beforeValue = (mode == KnobMode.AmplitudeOn) ? currentAmp : currentFreq;
        }
    }
    */
    
    void UpdateKnobValues(ref float valueToChange)
    {
        // Convert to procent
        //valueToChange *= 100f / max;
        if(Input.GetKey("q"))
        {
            valueToChange -= changeAmount * Time.deltaTime;

            if (valueToChange > MIN_VALUE + 1f)
            {
                knobTarget.RotateAroundLocal(knobTarget.forward, -Time.deltaTime);
            }
            
        }
        else if(Input.GetKey("e"))
        {
            valueToChange += changeAmount * Time.deltaTime;
            if(valueToChange < MAX_VALUE - 1f)
            {

              knobTarget.RotateAroundLocal(knobTarget.forward, Time.deltaTime);
            }
            
        }
        // Convert back
        //valueToChange *= max / 100f;
        valueToChange = Mathf.Clamp(valueToChange, MIN_VALUE, MAX_VALUE);
        Debug.Log("Current Amp: " + currentAmp + " | Target Amp: " + targetAmp + " | Current Freq: " + currentFreq + " | Target Freq: " + targetFreq );
    }

    private void UpdateSoundValues()
    {
        if(!isRadioOn)
        {
            return;
        }
        if (!messageSFX.isPlaying && IsAmplitudeInRange())
        {
            // start message
            messageSFX.Play();
        }
        else if (messageSFX.isPlaying && !IsAmplitudeInRange())
        {
            // end message
            messageSFX.Stop();
        }

        if (noiseSFX.isPlaying && IsFrequencyInRange() && IsAmplitudeInRange())
        {
            // end noise
            noiseSFX.Stop();
        } else if (!noiseSFX.isPlaying && !IsFrequencyInRange())
        {
            // start noise
            noiseSFX.Play();
        }
       


    }
    void UpdateRadioWaves(float amplitude, float frequency, LineRenderer lR)
    {
        // convert amplitude
        amplitude *= MAX_AMPLITUDE / 100f;
        float xStart = 0f;
        float tau = 2 * Mathf.PI;
        float xFinish = waveEndX;

        lR.positionCount = points;
        for (int i = 0; i < points; i++)
        {
            float progress = (float)i / (points - 1);
            float x = Mathf.Lerp(xStart, xFinish, progress);
            float y = amplitude * Mathf.Sin((tau * frequency * x) + (Time.timeSinceLevelLoad * movementSpeed));
            lR.SetPosition(i, new Vector3(lR.transform.position.x + x, lR.transform.position.y + y, lR.transform.position.z));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 waveEnd = transform.position;
        waveEnd.x += waveEndX;
        Gizmos.DrawSphere(waveEnd, 0.05f);
        /*
        if(mode == KnobMode.None || !knobTarget)
        {
            return;
        }
        Vector3 testMousePos = Input.mousePosition;
        testMousePos.z = knobTarget.position.z;
        testMousePos = _camera.ScreenToWorldPoint(testMousePos);
        testMousePos.z = knobTarget.position.z;

       
        Gizmos.DrawLine(mousePos, testMousePos);
        Gizmos.DrawLine(knobTarget.position, testMousePos);
        Gizmos.DrawLine(knobTarget.position, mousePos);
        */
    }
}

