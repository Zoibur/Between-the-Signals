using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainOverlay;
    public GameObject levelsOverlay;
    public GameObject settingsOverlay;
    public GameObject creditsOverlay;
    public GameObject tutorialOverlay;
    public Button loadLevelsButton;

    public GameObject lampObject;
    

    static int levelAmount = 5;
    public Button[] levelButtons = new Button[levelAmount];

    int highestLevelReached = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPrefs.SetInt("PlayerScore", 0);
        highestLevelReached = PlayerPrefs.GetInt("HighestLevelReached");
        if(highestLevelReached == 0 )
        {
            loadLevelsButton.interactable = false;
        }
        for(int i = 0; i < levelAmount; i++)
        {
            levelButtons[i].interactable = (i <= highestLevelReached);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float halfRange = (95f - 85f) / 2f;
        float rotateValue = 85f + halfRange + Mathf.Sin(Time.timeSinceLevelLoad) * halfRange;
        lampObject.transform.rotation = Quaternion.Euler(rotateValue, 0f, 0f);

        if(Input.GetKeyDown("r"))
        {
            PlayerPrefs.SetInt("HighestLevelReached", 0);
            loadLevelsButton.interactable = false;
        }
    }

    public void ShowLoadLevels()
    {
        // Hide buttons in main menu
        // Show buttons for each completed level
        // Also show "go back" button
        levelsOverlay.SetActive(true);
        mainOverlay.SetActive(false);

       
    }

    public void GoToMainOverlay()
    {
        settingsOverlay.SetActive(false);
        creditsOverlay.SetActive(false);
        levelsOverlay.SetActive(false);
        tutorialOverlay.SetActive(false);
        mainOverlay.SetActive(true);

    }
  
    public void OpenTutorial()
    {
        tutorialOverlay.SetActive(true);
        mainOverlay.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartLevel(int level)
    {
        PlayerPrefs.SetInt("CurrentLevel", level);
        SceneManager.LoadScene(1);
    }

    public void OpenSettings()
    {
        settingsOverlay.SetActive(true);
        mainOverlay.SetActive(false);
    }

    public void OpenCredits()
    {
        creditsOverlay.SetActive(true);
        mainOverlay.SetActive(false);
    }
}
