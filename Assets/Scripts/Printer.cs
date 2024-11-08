using UnityEngine;

public class Printer : Station
{
    bool active = false;
    const float FAIL_TIME = 2f;
    const float PRINT_TIME = 6f;
    float timer = 0f;
    bool successPrint = false;
    public Radio radio;

    public GameObject paperPrefab;
    private GameObject holdPaper = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

    public void Print()
    {
        if (!successPrint) 
        {
            FailPrint();
            return;
        }
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
    }
    public void FailPrint()
    {
        // Fail Feedback
        Debug.Log("Failure, Printer Explodes");
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
            Debug.Log("Took Paper");
            // Give paper to decoder
            holdPaper = null;
            Deactivate();
            return;
        }
        active = true;
        successPrint = radio.IsValuesCorrect();
        timer = (successPrint) ? PRINT_TIME : FAIL_TIME;
        GetComponent<Renderer>().material.color = (successPrint) ? Color.yellow : Color.red;
    }

    public override void Deactivate()
    {
        Debug.Log("Deactivate");
        GetComponent<Renderer>().material.color = Color.black;
        active = false;
    }

    public override bool IsZoomer()
    {
        return false;
    }
}
