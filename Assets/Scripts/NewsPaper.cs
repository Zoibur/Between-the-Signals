using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class NewsPaper : MonoBehaviour
{
    Vector3 originPos;
    public Vector3 finalPos;

    public Vector3 dayOnePosition;

    //public GameObject uiPaper;
    //GameObject uiHiddenLetter;
    public GameObject secretNote;

    public TextMeshPro modelText;

    float lerpProgress = 0f;
    bool isLerping = false;

    public AudioClip knockDoorAudio;
    public AudioClip slideAudio;

    float timeTillKnock = 8f;
    float timeKnockToSlide = 2f;

    //bool uiActive = false;

    Vector3 secretNotePos1;
    Vector3 secretNotePos2;
    Vector3 secretNotePos3;
    bool halfway = false;
    bool lerpNote = false;

    public GameObject noteButton;
    bool zoomed = false;

    bool inspectSkip = false;
    bool skipNote = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int currentDay = PlayerPrefs.GetInt("CurrentLevel");
        if(currentDay == 1)
        {
            skipNote = false;
           // modelText.text = "Day 1";
            transform.position = dayOnePosition;
            transform.Rotate(new Vector3(0f, 180f, 0f));
            //gameObject.SetActive(false);
            return;
        }
        int playerSuccess = PlayerPrefs.GetInt("PreviousSuccess");
        int playerFails = PlayerPrefs.GetInt("PreviousFailures");
        //int maxScore = PlayerPrefs.GetInt("PreviousMaxScore");
        //int missingPoints = maxScore - playerScore;
        int m = 500;
        string todaysNews = "";
        if (playerSuccess > 0)
        {
            todaysNews += (playerSuccess * m).ToString() + " Civilians Saved in Attack!\n";
        }
        if (playerFails == 0 && playerSuccess == 0)
        {
            playerFails = 2;
        }
        if (playerFails > 0)
        {
            todaysNews += (playerFails * m).ToString() + " Civilians Dead in Attack!";
        }
        //int result = playerScore * 500;
        originPos = transform.position;

       
        string todaysMission = "";

        Debug.Log("Day: " + currentDay.ToString());
        secretNote.SetActive(false);



        //todaysNews += (playerScore > 0) ? " Civilians Survived Attack!" : " Civilians Dead In Attack!";
        //todaysNews = (m * missingPoints).ToString() + " Civilians Dead in attack.\n" + (m * playerScore).ToString() + " Civilians were saved";

        /*
        switch (currentDay)
        {
            case 2:
                todaysNews = "Day 2";
                break;
            case 3:
                todaysNews = "Day 3";
                break;
            case 4:
                todaysNews = "Day 4";
                break;
            case 5:
                todaysNews = (m * missingPoints).ToString() + " Civilians Dead in attack.\n" + (m * playerScore).ToString() + " Civilians were saved";
                todaysMission = "Todays Mission\n" +
                    "Find and Decypher 3 Enemy Messages and then Send them to us.\n" +
                    "Intel suggest that apartments in your area will have check ups from Soldiers\nMake sure to not make sounds when Soldiers are near.\n" +
                    "Let no soldier find your equipment.";
                
                break;
        }
        */

        //uiNewspaper = uiPaper.transform.GetChild(1).gameObject;
        //uiHiddenLetter = uiPaper.transform.GetChild(0).gameObject;

        modelText.text = todaysNews;
        //uiNewspaper.GetComponentInChildren<TextMeshProUGUI>().text = todaysNews;
        //uiHiddenLetter.GetComponentInChildren<TextMeshProUGUI>().text = todaysMission;

        StartCoroutine(KnockDoor());
    }

    // Update is called once per frame
    void Update()
    {
        if (isLerping)
        {
            LerpNewspaper();
        }
        if (!zoomed)
        {
            return;
        }
        
        if (lerpNote)
        {
            LerpSecretNote();
        }
        if (inspectSkip)
        {
            inspectSkip = false;
            return;
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Stop Inspecting Paper");
            zoomed = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            noteButton.SetActive(false);
        }
        /*
        if (uiActive)
        {
            if (Input.GetKeyDown("e"))
            {
                //uiPaper.SetActive(false);
                uiActive = false;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
            }
        }
        */
    }
    public IEnumerator KnockDoor()
    {
        yield return new WaitForSeconds(timeTillKnock);
        StartCoroutine(SlideNewsPaper());
    }

    public IEnumerator SlideNewsPaper()
    {
        AudioManager.instance.PlaySoundFXClip(knockDoorAudio, transform, 1f);
        yield return new WaitForSeconds(timeKnockToSlide);
        isLerping =true;
        lerpProgress = 0f;
        AudioManager.instance.PlaySoundFXClip(slideAudio, transform, 1f);
    }

    void LerpNewspaper()
    {
        lerpProgress += Time.deltaTime;
        transform.position = Vector3.Lerp(originPos, finalPos, lerpProgress);
        if (lerpProgress >= 1f)
        {
            isLerping = false;
        }
    }

    void LerpSecretNote()
    {
        lerpProgress += Time.deltaTime;
        secretNote.transform.position = Vector3.Lerp(secretNotePos1, secretNotePos2, lerpProgress);
        if (!halfway && lerpProgress >= 0.5f)
        {
            halfway = true;
            secretNotePos1 = secretNotePos2;
            secretNotePos2 = secretNotePos3;
        }
        if (lerpProgress >= 1f)
        {
            lerpNote = false;
        }
    }
    public void Inspect()
    {
        // Show button
        //uiPaper.SetActive(true);
        //uiActive = true;

        inspectSkip = true;
        Debug.Log("Inspect Paper");
        zoomed = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (!skipNote)
        {
            noteButton.SetActive(true);
        }
      
    }
    public void ShowHiddenLetter()
    {
        if(halfway)
        {
            return;
        }
        lerpNote = true;
        halfway = false;
        secretNotePos1 = secretNote.transform.position;
        secretNotePos2 = secretNotePos1 + (secretNote.transform.up * 0.5f);
        secretNotePos3 = secretNotePos1 + (secretNote.transform.forward * 0.02f);
        lerpProgress = 0f;
        noteButton.SetActive(false);
        // Lerp paper
        //uiHiddenLetter.transform.SetSiblingIndex(1);
        Debug.Log("Show Hidden Letter");
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(finalPos, 0.1f);
        Gizmos.DrawSphere(dayOnePosition, 0.1f);
    }
}
