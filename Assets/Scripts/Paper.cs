using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Paper : MonoBehaviour 
{
    
    public TextMeshPro tmp;
    void Start()
    {
        tmp = GetComponentInChildren<TextMeshPro>();
         tmp.text = "Script Text";
    }

    void Update()
    {

    }
}


