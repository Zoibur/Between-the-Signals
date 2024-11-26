using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class NewsPaper : MonoBehaviour
{
    Vector3 originPos;
    public Vector3 finalPos;

    public Vector3 dayOnePosition;

    public GameObject uiPaper;
    GameObject uiHiddenLetter;
    GameObject uiNewspaper;

    public TextMeshPro modelText;

    float lerpProgress = 0f;
    bool isLerping = false;

    public AudioClip knockDoorAudio;
    public AudioClip slideAudio;

    float timeTillKnock = 8f;
    float timeKnockToSlide = 2f;

    bool uiActive = false;

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

        uiNewspaper = uiPaper.transform.GetChild(1).gameObject;
        uiHiddenLetter = uiPaper.transform.GetChild(0).gameObject;

        modelText.text = todaysNews;
        uiNewspaper.GetComponentInChildren<TextMeshProUGUI>().text = todaysNews;
        uiHiddenLetter.GetComponentInChildren<TextMeshProUGUI>().text = todaysMission;

        StartCoroutine(KnockDoor());
    }

    // Update is called once per frame
    void Update()
    {
        if(isLerping)
        {
            Lerping();
        }
        if (uiActive)
        {
            if (Input.GetKeyDown("e"))
            {
                uiPaper.SetActive(false);
                uiActive = false;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
            }
        }
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

    void Lerping()
    {
        lerpProgress += Time.deltaTime;
        transform.position = Vector3.Lerp(originPos, finalPos, lerpProgress);
        if (lerpProgress >= 1f)
        {
            isLerping = false;
        }
    }

    public void Inspect()
    {
        uiPaper.SetActive(true);
        uiActive = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ShowHiddenLetter()
    {
        uiHiddenLetter.transform.SetSiblingIndex(1);
        Debug.Log("Show Hidden Letter");
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(finalPos, 0.1f);
    }
}
