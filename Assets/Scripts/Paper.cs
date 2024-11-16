//using System.Globalization;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;
using System;

public class Paper : MonoBehaviour 
{
    //MessageList messages = new MessageList();
    public TextMeshPro tmpMorse;
    public TextMeshPro tmpInput;
    //public TextMeshPro tmpBlank;
    string targetMessage = string.Empty;
    string morseCodeMessage = string.Empty;
    string currentMessage { set; get; } = string.Empty; 
    //string blankMessage = string.Empty;

    int currentIndex = 0;
    void Start()
    {
        // tmp = GetComponentInChildren<TextMeshPro>();
       // targetMessage = messages.GetRandomMessage();
       // tmp.text = targetMessage;
        //Debug.Log("Letter Content: " + tmp.text);
    }

    void Update()
    {

    }
    public void SetTargetMessage(string newValue) { 
        targetMessage = newValue; 
    }
    public string GetTargetMessage() { return targetMessage; }

    public void SetMorseCodeMessage(string newValue)
    {
        morseCodeMessage = newValue;
        tmpMorse.text = morseCodeMessage;
    }

    public void RemoveFromInputMessage()
    {
        if (currentIndex <= 0)
        {
            return;
        }

        currentIndex--;
        if (currentMessage[currentIndex] == ' ')
        {
            currentIndex--;
        }

        string buffer = "";
        for (int i = 0; i < currentMessage.Length; i++)
        {
            if (i == currentIndex)
            {
                buffer += "-";
                continue;
            }
            buffer += currentMessage[i];
        }
        currentMessage = buffer;
        tmpInput.text = currentMessage;

        /*
        if (currentIndex <= 0)
        {
            return;
        }
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = 0;
            return;
        }
        if (currentMessage[currentIndex] == ' ')
        {
            currentIndex--;
        }
        Debug.Log("Current Index Before Remove: " + currentIndex);
        string buffer = "";
        for (int i = 0; i < currentMessage.Length; i++)
        {
            if (i == currentIndex)
            {
                buffer += '-';
                continue;
            }
            buffer += currentMessage[i];
        }
        currentMessage = buffer;
        tmpInput.text = currentMessage;

      

        Debug.Log("Current Index After Remove: " + currentIndex);
        */
    }
    public void AddToInputMessage(char newchar)
    {
        if(currentIndex >= currentMessage.Length)
        {
            return;
        }

        string buffer = "";
        for (int i = 0; i < currentMessage.Length; i++)
        {
            if (i == currentIndex)
            {
                buffer += char.ToUpper(newchar);
                continue;
            }
            buffer += currentMessage[i];
        }
        currentMessage = buffer;
        tmpInput.text = currentMessage;

        currentIndex++;
        if(currentIndex >= currentMessage.Length)
        {
            return;
        }
        if (currentMessage[currentIndex] == ' ')
        {
            currentIndex++;
        }
        /*
       if(currentIndex > targetMessage.Length - 1)
        {
            return;
        }
        Debug.Log("Current Index: " + currentIndex + " | Length: " + currentMessage.Length);
        string buffer = "";
        
        for(int i = 0; i < currentMessage.Length; i++)
        {
            if(i == currentIndex)
            {
                buffer += char.ToUpper(newchar);
                continue;
            }
            buffer += currentMessage[i];
        }
        currentMessage = buffer;
        tmpInput.text = currentMessage;
        currentIndex++;
        if (currentIndex >= targetMessage.Length)
        {
            Debug.Log("Line Full");
            currentIndex = targetMessage.Length - 1;
            return;
        }
        if (currentMessage[currentIndex] == ' ')
        {
            currentIndex ++;
        }
     
       
        Debug.Log("Message After: " + currentMessage);
        /*
        string buffer = "";
        for (int i = 0; i < targetMessage.Length; i++)
        {
           
            buffer += char.ToUpper(newValue[i]);
        }
        //foreach(char c in newValue)
       // {
        //    buffer += char.ToUpper(c);
        //}

       currentMessage = buffer;
        */

    }
    public string GetInputMessage()
    {
        return currentMessage;
    }

    public void SetInputMessage(string newValue)
    {
        currentMessage = newValue;
        tmpInput.text = currentMessage;
    }

    
}

// Message plan
// Remove Blank text
// Input message Start with blank "_" and " ".
// still use currentIndex
// if currentIndex lands on " ", currentIndex++;


// AddLetter
// if(!(currentIndex >= message.Length))
// UpdateString (new char on currentIndex)
// currentIndex++

// RemoveLetter
// if(!(currentIndex <= 0))
// currentIndex--;
// UpdateString ("-" on currentIndex)


