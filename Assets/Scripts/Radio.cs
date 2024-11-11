using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.TerrainUtils;

public class Radio : Station
{
    public enum KnobMode
    {
        None,
        AmplitudeOn,
        FrequencyOn,
    }

    bool active = false;

    float targetAmp = 0f;
    float targetFreq = 0f;

    float currentAmp = 0f;
    float currentFreq = 0f;

    float errorMargin = 0.1f;
    float changeAmpValueAmount = 0.01f;
    float changeFreqValueAmount = 5f;

    //public GameObject uiButtons;

    [Header("Knobs")]
    Camera _camera;
    LayerMask knobLayer;
    public KnobMode mode = KnobMode.None;
    Vector3 mousePos = Vector3.zero;
    Transform knobTarget;
    float beforeValue = 0f;

    // Wave variables
    [Header("Sinus Wave")]
    public LineRenderer currentLineRenderer;
    public LineRenderer targetLineRenderer;
    public int points = 60;
    public Vector3 xLimits = new Vector3(0f, 1f, 0f);
    public float movementSpeed = 6;

    const float MIN_AMPLITUDE = 0.01f;
    const float MAX_AMPLITUDE = 0.03f;

    const float MIN_FREQUENCY = 10f;
    const float MAX_FREQUENCY = 60f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        xLimits = new Vector3(0f, 0.5f, 0f);

        currentLineRenderer.startColor = Color.yellow;
        currentLineRenderer.endColor = Color.yellow;

        targetLineRenderer.startColor = Color.black;
        targetLineRenderer.endColor = Color.black;

        knobLayer = LayerMask.GetMask("Knob");
        _camera = Camera.main;

        NewRadioValues();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRadioWaves(currentAmp, currentFreq, currentLineRenderer);
        UpdateRadioWaves(targetAmp, targetFreq, targetLineRenderer);

        if (!active)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastToKnob();
        }
        if(mode == KnobMode.AmplitudeOn)
        {
            UpdateKnobValues(ref currentAmp, changeAmpValueAmount, MIN_AMPLITUDE, MAX_AMPLITUDE);
        } else if (mode == KnobMode.FrequencyOn) {
            UpdateKnobValues(ref currentFreq, changeFreqValueAmount, MIN_FREQUENCY, MAX_FREQUENCY);
        }

    }

    // Function for Printer Script to Call when starting to Print paper
    public bool IsValuesCorrect()
    {
        Debug.Log("Current Amp: " + currentAmp.ToString() + " Target Amp: " + targetAmp.ToString() + "Current Freq:" + currentFreq.ToString() + " Target Freq: " + targetFreq.ToString()); 
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

    public override void Activate()
    {
        active = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //NewRadioValues();

    }
    public override void Deactivate()
    {
        active = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if(knobTarget)
        {
            knobTarget.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public override bool IsZoomer()
    {
        return true;
    }

    public void NewRadioValues()
    {
        targetAmp = Random.Range(MIN_AMPLITUDE, MAX_AMPLITUDE);
        targetFreq = Random.Range(MIN_FREQUENCY, MAX_FREQUENCY);

        currentAmp = Random.Range(MIN_AMPLITUDE, MAX_AMPLITUDE);
        currentFreq = Random.Range(MIN_FREQUENCY, MAX_FREQUENCY);
    }



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
            knobTarget = hit.transform;
            mode = (knobTarget.tag == "AmplitudeKnob") ? KnobMode.AmplitudeOn : KnobMode.FrequencyOn;
            mousePos = hit.point;
            mousePos.z = knobTarget.position.z;
            knobTarget.GetComponent<Renderer>().material.color = Color.yellow;
            beforeValue = (mode == KnobMode.AmplitudeOn) ? currentAmp : currentFreq;
        }
    }
    
    void UpdateKnobValues(ref float valueToChange, float changeAmount, float min, float max)
    {
        /*
        if (Input.GetMouseButtonUp(0))
        {
            mode = KnobMode.None;
            knobTarget.GetComponent<Renderer>().material.color = Color.white;
            knobTarget = null;
            return;
        }
        Vector3 testMousePos = Input.mousePosition;
        testMousePos.z = knobTarget.position.z;
        testMousePos = _camera.ScreenToWorldPoint(testMousePos);
        testMousePos.z = knobTarget.position.z;

        float a = Vector3.Distance(knobTarget.transform.position, mousePos); // Dist knob to ori mouse pos
        float b = Vector3.Distance(knobTarget.transform.position, testMousePos);  // Dist knob to current mouse pos
        float c = Vector3.Distance(testMousePos, mousePos); // Dist ori mouse pos to current mouse pos

        float angle = Mathf.Acos((a*a + b*b - c*c) / (2 * a * b));
        float tau = Mathf.PI * 2f;
        float progress = angle / tau;

        valueToChange = beforeValue + Mathf.Lerp(min, max, progress);
        valueToChange = Mathf.Clamp(valueToChange, min, max);
        Debug.Log("Angle: " + angle.ToString() + " | Progress: " + progress.ToString() +" | Adding Value:  " + Mathf.Lerp(min, max, progress).ToString() + " | Current Value: " + valueToChange.ToString());
        */
        if(Input.GetKey("q"))
        {
            valueToChange -= changeAmount * Time.deltaTime; 
        }
        else if(Input.GetKey("e"))
        {
            valueToChange += changeAmount * Time.deltaTime;
        }
        valueToChange = Mathf.Clamp(valueToChange, min, max);
        Debug.Log("Current Amp: " + currentAmp + " | Target Amp: " + targetAmp + " | Current Freq: " + currentFreq + " | Target Freq: " + targetFreq );
    }
    void UpdateRadioWaves(float amplitude, float frequency, LineRenderer lR)
    {
        float xStart = xLimits.x;
        float tau = 2 * Mathf.PI;
        float xFinish = xLimits.y;

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
        if(mode == KnobMode.None || !knobTarget)
        {
            return;
        }
        Vector3 testMousePos = Input.mousePosition;
        testMousePos.z = knobTarget.position.z;
        testMousePos = _camera.ScreenToWorldPoint(testMousePos);
        testMousePos.z = knobTarget.position.z;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(mousePos, testMousePos);
        Gizmos.DrawLine(knobTarget.position, testMousePos);
        Gizmos.DrawLine(knobTarget.position, mousePos);
    }
}

/*
 * 
 * 
    Circle slider thingy
    where player starts holding on slider becomes 0
    moving the slider will increment/decrement from the values


    Button that when press will trigger an activation
    Save mouse position compared to button center
    While button is held down, calculate the angle between original mouse pos and current mouse pos
    currentValue = original value + lerp(minValue, maxValue, angle / 360)
    Releasing button will save values
 */