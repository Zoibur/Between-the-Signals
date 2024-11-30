using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject quitMenu;
    public GameObject settingsMenu;
    public GameObject tutorialMenu;
    public GameObject crossHair;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }
    void TogglePauseMenu()
    {
        if(menu.activeSelf)
        {
            UnPause();
        }
        else
        {
            Pause();
        }
    }

    public void UnPause()
    {
        menu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        crossHair.SetActive(true);
    }

    public void Pause()
    {
        menu.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        crossHair.SetActive(false);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenQuitMenu()
    {
        quitMenu.SetActive(true);
    }
    public void CloseQuitMenu()
    {
        quitMenu.SetActive(false);
    }
    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
        //menu.SetActive(false);
    }

    public void OpenTutorial()
    {
        tutorialMenu.SetActive(true);
        //menu.SetActive(false);
    }

    public void GoBack()
    {
        //menu.SetActive(true);
        tutorialMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }
}
