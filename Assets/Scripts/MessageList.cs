using UnityEngine;
using System.Collections.Generic;
//using System;

public class MessageList 
{

    List<string> morsecodes = new List<string>();

    string[] sideWords = new string[4];
    string[] locationWords = new string[4];
    string[] actionWords = new string[4];
    string[] timeWords = new string[4];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public MessageList()
    {
        morsecodes.Add("ATT YES");
        morsecodes.Add("REI NO PL");
        morsecodes.Add("FL BU");
        morsecodes.Add("SUP PLY");
        morsecodes.Add("TE ST");

        sideWords[0] = "NORTH";
        sideWords[1] = "EAST";
        sideWords[2] = "SOUTH";
        sideWords[3] = "WEST";

        locationWords[0] = "ARMAVIR";
        locationWords[1] = "BOBROVKA";
        locationWords[2] = "OKNO";
        locationWords[3] = "DONGUZ";

        actionWords[0] = "AIRSTRIKE";
        actionWords[1] = "FLANK";
        actionWords[2] = "INFILTRATE";
        actionWords[3] = "AMBUSH";

        timeWords[0] = "MIDNIGHT";
        timeWords[1] = "MORNING";
        timeWords[2] = "NOON";
        timeWords[3] = "EVENING";

    }

    public string GetRandomMorseCodeMessage()
    {
        int randNum = Random.Range(0, morsecodes.Count);
        return morsecodes[randNum];
    }
    public string LetterToSequence(char letter)
    {
        return char.ToUpper(letter) switch
        {
            'A' => ".-",
            'B' => "-...",
            'C' => "-.-.",
            'D' => "-..",
            'E' => ".",
            'F' => "..-.",
            'G' => "--.",
            'H' => "....",
            'I' => "..",
            'J' => ".---",
            'K' => "-.-",
            'L' => ".-..",
            'M' => "--",
            'N' => "-.",
            'O' => "---",
            'P' => ".--.",
            'Q' => "--.-",
            'R' => ".-.",
            'S' => "...",
            'T' => "-",
            'U' => "..-",
            'V' => "...-",
            'W' => ".--",
            'X' => "-..-",
            'Y' => "-.--",
            'Z' => "--..",
            '1' => ".----",
            '2' => "..---",
            '3' => "...--",
            '4' => "....-",
            '5' => ".....",
            '6' => "-....",
            '7' => "--...",
            '8' => "---..",
            '9' => "----.",
            '0' => "-----",
            _ => throw new System.Exception("Invalid letter '" + letter + "'")
        };
    }
   public char SequenceToLetter(string sequence)
    {
        return (sequence) switch
        {
            ".-" => 'A',
            "-..." => 'B',
            "-.-." => 'C',
            "-.." => 'D',
            "." => 'E',
            "..-." => 'F',
            "--." => 'G',
            "...." => 'H',
            ".." => 'I',
            ".---" => 'J',
            "-.-" => 'K',
            ".-.." => 'L',
            "--" => 'M',
            "-." => 'N',
            "---" => 'O',
            ".--." => 'P',
            "--.-" => 'Q',
            ".-." => 'R',
            "..." => 'S',
            "-" => 'T',
            "..-" => 'U',
            "...-" => 'V',
            ".--" => 'W',
            "-..-" => 'X',
            "-.--" => 'Y',
            "--.." => 'Z',
            _ => throw new System.Exception("Invalid sequence ''" + sequence + "''")
        };
    }
    public string GenerateMorseCode(string text)
    {
        string buffer = "";
        foreach (char c in text)
        {
            if (c == ' ')
            {
                buffer += '/';
                continue;
            }

            buffer += LetterToSequence(c) + "   ";
        }

        return buffer;
    }

    public string GenerateBlank(string text)
    {
        string buffer = "";
        foreach (char c in text) {
            if (c == ' ')
            {
                buffer += ' ';
                continue;
            }
            else
            {
                buffer += '-';
            }
           
        }
        return buffer;
    }

    public string GetRandomSecretMessage()
    {
        // Create random of words
        string realMessage = "";
        realMessage += sideWords[Random.Range(0, 4)];
        realMessage += " " + locationWords[Random.Range(0, 4)];
        realMessage += " " + actionWords[Random.Range(0, 4)];
        realMessage += " " + timeWords[Random.Range(0, 4)];

        return realMessage;

    }

    public string GenerateSecretMessage(string code)
    {
     
        string[] message = code.Split(" ");
        string result = "";
        for (int i = 0; i < message.Length; i++)
        {
            result += WordToCodeName(message[i]);
            result += " ";
        }

        return result;
    }

    public string WordToCodeName(string word)
    {
        return (word) switch
        {
            // Side
            "NORTH" => "AIR",
            "EAST" => "EARTH",
            "SOUTH" => "FIRE",
            "WEST" => "WATER",

            // Location
            "ARMAVIR" => "ALPHA",
            "BOBROVKA" => "BETA",
            "OKNO" => "GAMMA",
            "DONGUZ" => "DELTA",

            // Action
            "AIRSTRIKE" => "DRAGON",
            "FLANK" => "DOG",
            "INFILTRATION" => "RAT",
            "AMBUSH" => "SNAKE",

            // Time
            "MIDNIGHT" => "WINTER",
            "MORNING" => "SPRING",
            "NOON" => "SUMMER",
            "EVENING" => "FALL",

            _ => throw new System.Exception("Invalid word '" + word + "'")
        };
    }
}
