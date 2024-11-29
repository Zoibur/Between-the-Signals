using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
public class EndingManager : MonoBehaviour
{
    enum EndingState
    {
        PreCutscene,
        Cutscene,
        Credits,
    }
    EndingState state = EndingState.PreCutscene;
    int ending = 0;
    public Animator transition;
    public Text epilogueText;
    public GameObject drawer;

    public GameObject newsPaper;

    public GameObject skipText;

    // Credits
    public Animator credits;
  

    // Cutscene
    int currentStage = 0;
    float stageProgress = 0f;
    float progressSpeed = 0.8f;
    Camera cam;

    Vector3 oriPos;
   // public Vector3 dstPos;

    Vector3 camOriRot;
    public Vector3 camDstRot;

    Vector3 paperOriPos;
    public Vector3 paperDstPos;
    Vector3 paperOriRot;
    Vector3 paperDstRot;

    float timePerDev = 3f;
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transition.gameObject.SetActive(true);
        cam = Camera.main;
        oriPos = drawer.transform.localPosition;
        camOriRot = cam.transform.rotation.eulerAngles;
        epilogueText.text = "Epilogue";

        paperOriPos = newsPaper.transform.position;
        paperOriRot = newsPaper.transform .rotation.eulerAngles;
        paperDstRot = new Vector3(0f, 0f, -90f);

        int maxScore = 5;
        int playerScore = PlayerPrefs.GetInt("PlayerScore");

        ending = (playerScore < maxScore) ? 1 : 2;

        Debug.Log("PlayerScore: " + playerScore.ToString() + " | Ending: " + ending.ToString());

      
        switch(ending)
        {
            case 1:
                newsPaper.GetComponentInChildren<TextMeshPro>().text = "The War is Won!\nThe Goverment is now cleansing the captial from any hiding spies.";
                break;
            case 2:
                newsPaper.GetComponentInChildren<TextMeshPro>().text = "The War is Over!\nOur Great Nation has been Annexed.";
                break;
        }

        StartCoroutine(StartCutscene());
    }


    IEnumerator StartCutscene()
    {
        yield return new WaitForSeconds(4f);
        state = EndingState.Cutscene;
    }
    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case EndingState.PreCutscene:
                break;
            case EndingState.Cutscene:
                switch (ending)
                {
                    case 1:
                        UpdateEndingOne();
                        break;
                    case 2:
                        UpdateEndingTwo();
                        break;
                }
                break;
            case EndingState.Credits:
                if (Input.anyKeyDown)
                {
                    if (skipText.gameObject.activeSelf)
                    {
                        SceneManager.LoadScene(0);
                    }
                    skipText.gameObject.SetActive(true);
                }
                break;
        }
    }
    
    void UpdateEndingOne()
    {
        stageProgress += Time.deltaTime * progressSpeed;
        if (stageProgress > 1f)
        {
            stageProgress = 0f;
            currentStage++;
        }
        switch (currentStage)
        {
            case 1:
                // Lerp newspaper to table
                newsPaper.transform.position = Vector3.Lerp(paperOriPos,paperDstPos,stageProgress);
                newsPaper.transform.rotation = Quaternion.Lerp(Quaternion.Euler(paperOriRot), Quaternion.Euler(paperDstRot), stageProgress);
                break;
            case 2:
                // Lerp rotation to drawer
                cam.transform.rotation = Quaternion.Lerp(Quaternion.Euler(camOriRot), Quaternion.Euler(camDstRot), stageProgress);
                break;
            case 3:
                // Lerp drawer position
                drawer.transform.localPosition = new Vector3(Mathf.Lerp(oriPos.x, oriPos.x - 0.5f, stageProgress), drawer.transform.position.y, 0f);//Vector3.Lerp(oriPos, dstPos, stageProgress);
                break;
            case 4:
                // Zoom in on pills
                cam.fieldOfView = Mathf.Lerp(45, 35, stageProgress);
                break;
            case 5:
                // Load Credits
                //StartCoroutine(LoadCredits());
                StartCoroutine(EndCutscene());
                break;
        }
    }

    void UpdateEndingTwo()
    {
        
    }

   
    IEnumerator EndCutscene()
    {
        state = EndingState.Credits;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(2f);
        StartCoroutine(ActivateCredits());
    }

    IEnumerator ActivateCredits()
    {
        credits.transform.gameObject.SetActive(true);
        credits.SetTrigger("Start");
        yield return new WaitForSeconds(20f);
        SceneManager.LoadScene(0);
    }
    /*
    public IEnumerator LoadCredits()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
    */
}
