using UnityEngine;
using TMPro;
public class Book : MonoBehaviour
{

    /*
    public static int pagesAmount = 9;
    string[] pages = new string[pagesAmount];
    public TextMeshPro currentPage;
    int currentIndex = 0;

    private void Start()
    {
       // Debug.Log("Book Start");
       // currentPage = GetComponent<TextMeshPro>();
       SetupPages();
        currentPage.text = pages[currentIndex];
    }

    void SetupPages()
    {
        pages[0] = "Morse Code Translations [A-Z]\n\nA = .-\nB = -...";
        pages[1] = "C = -.-.\nD = -..\nE = .\nF = ..-.";
        pages[2] = "G = --.\nH = ....\nI = ..\nJ = .---";
        pages[3] = "K = -.-\nL = .-..\nM = --\nN = -.";
        pages[4] = "O = ---\nP = .--.\nQ = --.-\nR = .-.";
        pages[5] = "S = ...\nT = -\nU = ..-\nV = ...-";
        pages[6] = "W = .--\nX = -..-\nY = -.--\nZ = --..";
        pages[7] = "1 = .----\n2 = ..---\n3 = ...--\n4 = ....-\n5 = .....";
        pages[8] = "6 = -....\n7 = --...\n8 = ---..\n9 = ----.\n0 = -----";
    }
    private void Update()
    {
        if(!active)
        {
            return;
        }
        if (Input.GetKeyDown("q"))
        {
            PreviousPage();
        }
        if (Input.GetKeyDown("e"))
        {
            NextPage();
        }
    }
    */
    public bool IsActive()
    {
        return transform.GetChild(1).gameObject.activeSelf;
    }
    public void Toggle()
    {
        transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
        transform.GetChild(1).gameObject.SetActive(!transform.GetChild(1).gameObject.activeSelf);
    }
    /*
    public void Activate()
    {
       transform.GetChild(0).gameObject.SetActive(false);
       transform.GetChild(1).gameObject.SetActive(true);
    }
    public void Deactivate()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
    }
    */
    /*
    public void NextPage()
    {
        if(currentIndex >= pagesAmount-1)
        {
            return;
        }

        currentIndex++;

        currentPage.text = pages[currentIndex];
    }
    public void PreviousPage()
    {
        if (currentIndex <= 0)
        {
            return;
        }

        currentIndex--;
        currentPage.text = pages[currentIndex];
    }
    */
}
