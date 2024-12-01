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

    public TextMeshProUGUI outcomeText;

    public AudioClip gulp;
    public AudioClip thump;
    public AudioClip knock;

    bool hasSkipped = false;


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


    Vector3 camLookDoor = new Vector3(0f, 269f, 0f);

    //float timePerDev = 3f;
  
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

        int maxScore = 15;
        int playerScore = PlayerPrefs.GetInt("PlayerScore");

        int deadPeople = PlayerPrefs.GetInt("TotalFailures") * 500;
        int alivePeople = PlayerPrefs.GetInt("TotalSuccess") * 500;

        ending = (playerScore < maxScore) ? 1 : 2;

        Debug.Log("PlayerScore: " + playerScore.ToString() + " | Ending: " + ending.ToString());

      
        switch(ending)
        {
            case 1:
                newsPaper.GetComponentInChildren<TextMeshPro>().text = "The War is Won!\nThe Goverment is now cleansing the captial from any hiding spies.";
                outcomeText.text = "Bad Ending\n\nDespite your effort, you managed to save: "
                    + alivePeople.ToString() + " Civilians.\n"+ deadPeople + " Civilians died because of you.";//outcomeText.text = "As the war ended, you had no other choice but to kill yourself before the goverment could catch you.";
                break;
            case 2:
                newsPaper.GetComponentInChildren<TextMeshPro>().text = "The War is Over!\nOur Great Nation has been Annexed.";
                outcomeText.text = "As the war ended, a rescue squad enters the city to escort you home to your loving wife and child. You were celebrated as a hero.\n\nDuring your mission, you managed to save: " 
                    + alivePeople.ToString() + " Civilians.\n" + deadPeople + " Civilians died because of you.";
                break;
        }

        StartCoroutine(StartCutscene());
    }


    IEnumerator StartCutscene()
    {
        yield return new WaitForSeconds(8f);
        state = EndingState.Cutscene;
        if (ending == 2)
        {
            AudioManager.instance.PlaySoundFXClip(knock, transform, 0.8f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case EndingState.PreCutscene:
                break;
            case EndingState.Cutscene:
                UpdateCutscene();
                break;
            case EndingState.Credits:
                UpdateCredit();
                break;
        }
    }
    void UpdateCredit()
    {
       
        if (!Input.anyKeyDown)
        {
            return;
        }
        if (skipText.gameObject.activeSelf)
        {
            if(hasSkipped)
            {
                SceneManager.LoadScene(0);
            }
            else
            {

            credits.SetTrigger("Skip");
                hasSkipped = true;
            }
        }
        skipText.gameObject.SetActive(true);

    }
    void UpdateCutscene()
    {
       
        switch (ending)
        {
            case 1:
                UpdateEndingOne();
                break;
            case 2:
                UpdateEndingTwo();
                break;
        }
        if (Input.anyKeyDown)
        {
            if (!skipText.gameObject.activeSelf)
            {
                skipText.gameObject.SetActive(true);
                return;
            }
            switch (ending)
            {
                case 1:
                    StartCoroutine(EndCutsceneOne());
                    break;
                case 2:
                    StartCoroutine(EndCutsceneTwo());
                    break;
            }
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
                StartCoroutine(EndCutsceneOne());
                break;
        }
    }

    void UpdateEndingTwo()
    {
        stageProgress += Time.deltaTime * progressSpeed;
        if (stageProgress > 1f)
        {
            stageProgress = 0f;
            currentStage++;
           
           
        }
        switch(currentStage)
        {
            case 1:
                newsPaper.transform.position = Vector3.Lerp(paperOriPos, paperDstPos, stageProgress);
                newsPaper.transform.rotation = Quaternion.Lerp(Quaternion.Euler(paperOriRot), Quaternion.Euler(paperDstRot), stageProgress);
                break;
            case 2:
                cam.transform.rotation = Quaternion.Lerp(Quaternion.Euler(camOriRot), Quaternion.Euler(camLookDoor), stageProgress);
                break;
            case 3:
                StartCoroutine(EndCutsceneTwo());
                break;

        }
     
    }

   
    IEnumerator EndCutsceneOne()
    {
        state = EndingState.Credits;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(2f);
        AudioManager.instance.PlaySoundFXClip(gulp, transform, 1f);
        yield return new WaitForSeconds(3f);
        AudioManager.instance.PlaySoundFXClip(thump, transform, 0.4f);
        yield return new WaitForSeconds(1f);
        StartCoroutine(ActivateCredits());
    }
    IEnumerator EndCutsceneTwo()
    {
        state = EndingState.Credits;
        transition.SetTrigger("Start");
      //  yield return new WaitForSeconds(1f);
        
        yield return new WaitForSeconds(2f);
        StartCoroutine(ActivateCredits());
    }

    IEnumerator ActivateCredits()
    {
        credits.transform.gameObject.SetActive(true);
        credits.SetTrigger("Start");
        //credits.StartPlayback();
        yield return new WaitForSeconds(33f);
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
