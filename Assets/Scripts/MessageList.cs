using UnityEngine;
using System.Collections.Generic;
//using System;

public class MessageList 
{

    List<string> message = new List<string>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public MessageList()
    {
        message.Add("ATT YES");
        message.Add("REI NO PL");
        message.Add("FL BU");
        message.Add("SUP PLY");
        message.Add("TE ST");
    }

    public string GetRandomMessage()
    {
        int randNum = Random.Range(0, message.Count);
        return message[randNum];
    }
    public string LetterToSequence(char letter)
    {
        return char.ToUpper(letter) switch
        {
            'A' => ".",
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
    public string GenerateToWords(string code)
    {
        string[] message = code.Split(" ");
        string result = "";
        for (int i = 0; i < message.Length; i++)
        {
            result += SequenceToLetter(message[i]);
        }

        return result;
    }
}
