using UnityEngine;
using TMPro;
public class Book : MonoBehaviour
{
    bool active = false;
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
    public void Activate()
    {
        active = true;
    }
    public void Deactivate()
    {
        active = false;
    }
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
}
