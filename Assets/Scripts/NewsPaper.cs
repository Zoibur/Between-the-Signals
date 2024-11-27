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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int currentDay = PlayerPrefs.GetInt("CurrentLevel");
        if(currentDay == 1)
        {
            transform.position = dayOnePosition;
            //gameObject.SetActive(false);
            return;
        }
        int playerScore = PlayerPrefs.GetInt("PreviousScore");
        int maxScore = PlayerPrefs.GetInt("PreviousMaxScore");
        int missingPoints = maxScore - playerScore;
        int m = 100;
        originPos = transform.position;

        string todaysNews = "";
        string todaysMission = "";

        switch(currentDay)
        {
            case 2:
              
                
            case 3:
               
            case 4:
                
            case 5:
                todaysNews = (m * missingPoints).ToString() + " Civilians Dead in attack.\n" + (m * playerScore).ToString() + " Civilians were saved";
                todaysMission = "Todays Mission\n" +
                    "Find and Decypher 3 Enemy Messages and then Send them to us.\n" +
                    "Intel suggest that apartments in your area will have check ups from Soldiers\nMake sure to not make sounds when Soldiers are near.\n" +
                    "Let no soldier find your equipment.";
                break;
        }

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
        if(Input.GetMouseButtonDown(1))
        {
            zoomed = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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
        zoomed = true;
        noteButton.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
