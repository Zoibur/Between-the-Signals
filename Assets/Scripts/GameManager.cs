
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Station[] stations;

    [SerializeField]

    int successScore = 0;
    int failScore = 0;
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
        levelNames[0] = "1956-11-03";
        levelNames[1] = "1956-11-10";
        levelNames[2] = "1956-11-17";
        levelNames[3] = "1956-11-24";
        levelNames[4] = "1956-12-01";

        Instance = this;
        levelTransition.gameObject.SetActive(true);
        currentLevel = PlayerPrefs.GetInt("CurrentLevel");

        currentLevel = Mathf.Clamp(currentLevel, 1, levelAmount);
        dayText.text = "Date: " + levelNames[currentLevel - 1];

        // Add level exclusive objects
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
    
    public void AddScore(int amount)
    {
        if (amount <= 0)
            return;

        successScore += amount;
        score += amount;
    }

    public void DeductScore(int amount)
    {
        if (amount <= 0)
            return;

        failScore += amount;
        score -= amount;
    }

    public bool IsNoiseAboveThreshold(int threshold)
    {
        int noise = 0;
        if (stations != null) {
            for (int i = 0; i < stations.Length; i++) {
                if (stations[i].IsMakingNoise()) {
                    noise++;
                }
            }
        }

        return noise >= threshold;
    }
    
    public IEnumerator LoadNextDay()
    {
        levelTransition.SetTrigger("Start");
        Debug.Log("Loading next day");
      
        yield return new WaitForSeconds(transitionTime);

        // Setup Day values "player, objects, etc"
        currentLevel++;
        if (currentLevel == 6)
        {
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
            yield break;
        }
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        if (currentLevel > PlayerPrefs.GetInt("HighestLevelReached"))
        {
            PlayerPrefs.SetInt("HighestLevelReached", currentLevel);
        }
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    public IEnumerator LoadScene(int index)
    {
        levelTransition.SetTrigger("Start");
        Debug.Log("This Round: Success: " + successScore.ToString() + " | Failures: " + failScore.ToString() + " | Final Score: " + (successScore - failScore).ToString());
        yield return new WaitForSeconds(transitionTime);

        PlayerPrefs.SetInt("PreviousSuccess", successScore);
        PlayerPrefs.SetInt("PreviousFailures", failScore);
        PlayerPrefs.SetInt("PlayerScore", PlayerPrefs.GetInt("PlayerScore") + (successScore - failScore));
        SceneManager.LoadScene(index);
    }

    
}
