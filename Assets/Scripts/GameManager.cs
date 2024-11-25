
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private int score = 0;

    private int currentLevel = 1;

    public Animator levelTransition;
    //public Animator sceneTransition;
    float transitionTime = 3f;
    public Text dayText;

    static int levelAmount = 5;
    string[] levelNames = new string[levelAmount];

    public void Awake()
    {
        levelNames[0] = "1956-11-23";
        levelNames[1] = "1957-01-04";
        levelNames[2] = "1957-03-23";
        levelNames[3] = "1957-04-07";
        levelNames[4] = "1957-06-19";

        Instance = this;
        levelTransition.gameObject.SetActive(true);
        currentLevel = PlayerPrefs.GetInt("CurrentLevel");

        currentLevel = Mathf.Clamp(currentLevel, 1, levelAmount);
        dayText.text = "Date: " + levelNames[currentLevel - 1];

        // Add level exclusive objects
    }


    public void AddScore(int amount)
    {
        if (amount <= 0)
            return;

        score += amount;
    }

    public void DeductScore(int amount)
    {
        if (amount <= 0)
            return;

        score -= amount;
    }

    public IEnumerator LoadNextDay()
    {
        levelTransition.SetTrigger("Start");
      
        yield return new WaitForSeconds(transitionTime);

        // Setup Day values "player, objects, etc"
        currentLevel++;
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        if (currentLevel > PlayerPrefs.GetInt("HighestLevelReached"))
        {
            PlayerPrefs.SetInt("HighestLevelReached", currentLevel);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator LoadScene()
    {
        //sceneTransition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
    }
}
