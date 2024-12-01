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
        morsecodes.Add("MARK RUSH 588 944");
        morsecodes.Add("ARMY BASE 9AM");
        morsecodes.Add("AMMO AID MAIN CITY");
        morsecodes.Add("SPY COUP HUSH");
        morsecodes.Add("KILL FOE WEST");
        morsecodes.Add("ARMY BASE WIPE");
        morsecodes.Add("LOOT 408 718");
        morsecodes.Add("BASE WIPE 896 232");
        morsecodes.Add("JAM BASE SPY");
        morsecodes.Add("RIOT WEST CITY");
        morsecodes.Add("BOMB 594 921");
        morsecodes.Add("KILL SPY 7AM");
        morsecodes.Add("ARMY BASE 794 912");
        morsecodes.Add("OPS TASK KILL SPY");
        morsecodes.Add("KILL RANK FOE EAST");
        morsecodes.Add("AID 849 189");
        morsecodes.Add("RAID CITY 782 899");
        morsecodes.Add("RAIN HELL HOME");
        morsecodes.Add("JAM CODE HUSH");
        morsecodes.Add("RAID HQ 149 495");
        morsecodes.Add("BOMB HQ 149 495");
        morsecodes.Add("FIND THE SPY");
        morsecodes.Add("HIDE THE DEAD");
        morsecodes.Add("FIND 501 245");
        morsecodes.Add("BOMB KEY 694 129");
        morsecodes.Add("MOVE THE BOMB");
        morsecodes.Add("SEEK OUT OUR ALLY");
        morsecodes.Add("TRAP 717 792");
        morsecodes.Add("SAFE ZONE 801 791");
        morsecodes.Add("KILL ZONE 359 495");
        morsecodes.Add("HELP ARMY BASE");
        


        // ARMY BASE BOMB FOE AID SPY KILL OPS CODE MARK TASK CAMO JAM MINE RUSH WIPE COUP  RANK RIOT AMMO 
       
      

        sideWords[0] = "NORTH";
        sideWords[1] = "EAST";
        sideWords[2] = "SOUTH";
        sideWords[3] = "WEST";

        locationWords[0] = "DOVDHOUR";
        locationWords[1] = "KOP";
        locationWords[2] = "LECH";
        locationWords[3] = "ARTUPHER";

        actionWords[0] = "AIRSTRIKE";
        actionWords[1] = "FLANK";
        actionWords[2] = "INFILTRATION";
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
                buffer += "\n";
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
            "DOVDHOUR" => "ALPHA",
            "KOP" => "BETA",
            "LECH" => "GAMMA",
            "ARTUPHER" => "DELTA",

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
