using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathManager : MonoBehaviour
{
    public static DeathManager Instance;

    public GameObject door;
    public GameObject guy;
    public CinemachineVirtualCamera POVCamera;
    public AudioClip doorBreakSound;
    //public GameObject screenBlocker;
    public Animator deathScreen;
    public Text deathText;

    public AudioClip punchInitSFX;
    public AudioClip punchCineSFX;


    public void Awake()
    {
        Instance = this;
    }

    private IEnumerator DeathSequence()
    {
        door.transform.rotation = Quaternion.Euler(0, 110.0f, 0);
        AudioManager.instance.PlaySoundFXClip(doorBreakSound, door.transform, 1.0f);
        yield return new WaitForSeconds(0.35f);
        
        guy.SetActive(true);
        POVCamera.gameObject.SetActive(true);
        AudioManager.instance.PlaySoundFXClip(punchInitSFX, guy.transform, 0.1f);
        AudioManager.instance.PlaySoundFXClip(punchCineSFX, guy.transform, 0.1f);

        yield return new WaitForSeconds(0.65f);

        //screenBlocker.SetActive(true);
        deathScreen.SetTrigger("Dead");
        deathText.text = "GAME OVER!";
        
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void PlayDeathCutscene()
    {
        StartCoroutine(DeathSequence());
    }

    void Start()
    {
        //PlayDeathCutscene();
    }
}
