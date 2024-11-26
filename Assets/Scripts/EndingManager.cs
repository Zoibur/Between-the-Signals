using UnityEngine;
using UnityEngine.UI;
public class EndingManager : MonoBehaviour
{
    int ending = 0;
    public Animator transition;
    public Text epilogueText;
    public GameObject drawer;

    int currentStage = 0;
    float stageProgress = 0f;
    float progressSpeed = 0.5f;
    Camera cam;

    Vector3 oriPos;
   // public Vector3 dstPos;

    Vector3 oriRot;
    public Vector3 dstRot;

    bool playCutscene = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transition.gameObject.SetActive(true);
        cam = Camera.main;
        oriPos = drawer.transform.localPosition;
        oriRot = cam.transform.rotation.eulerAngles;
        epilogueText.text = "Epilogue";

        int maxScore = 10;
        int playerScore = PlayerPrefs.GetInt("PlayerScore");

        ending = (playerScore < maxScore) ? 1 : 2;

        Debug.Log("PlayerScore: " + playerScore.ToString() + " | Ending: " + ending.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            playCutscene = true;
        }
        if (!playCutscene)
        {
            return;
        }
        switch (ending)
        {
            case 1:
                UpdateBadEnding();
                break;
            case 2:
                break;
        }
    }

    void UpdateBadEnding()
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
                // Stand  still
                break;
            case 2:
                // Lerp rotation to drawer
                cam.transform.rotation = Quaternion.Lerp(Quaternion.Euler(oriRot), Quaternion.Euler(dstRot), stageProgress);
                break;
            case 3:
                // Lerp drawer position
                drawer.transform.localPosition = new Vector3(Mathf.Lerp(oriPos.x, oriPos.x - 0.5f, stageProgress), drawer.transform.position.y, 0f);//Vector3.Lerp(oriPos, dstPos, stageProgress);
                break;
            case 4:
                // Zoom in on pills
                cam.fieldOfView = Mathf.Lerp(45, 25, stageProgress);
                break;
            case 5:
                // Load Credits
                break;
        }
    }
}
