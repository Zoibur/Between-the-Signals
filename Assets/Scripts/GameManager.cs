
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
    public float transitionTime = 3f;
    public Text dayText;

    public void Awake()
    {
        Instance = this;
        currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        dayText.text = "Day: " + currentLevel.ToString();
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
