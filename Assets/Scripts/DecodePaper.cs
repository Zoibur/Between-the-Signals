using System.Collections.Generic;
using UnityEngine;

public class DecodePaper : MonoBehaviour
{
    public string LetterToSequence(char letter)
    {
        return char.ToUpper(letter) switch {
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
            _ => throw new System.Exception("Invalid letter '" + letter + "'")
        };
    }

    public char SequenceToLetter(string sequence)
    {
        return (sequence) switch 
        {
           ".-" => 'A',
            "-..." => 'B',
            "-.-."  => 'C',
            "-.." => 'D',
            "." => 'E',
            "..-." => 'F',
            "--." => 'G',
            "...." => 'H',
            ".." => 'I',
            ".---" => 'J',
            "-.-"  => 'K',
            ".-.." => 'L',
            "--" => 'M',
            "-."  => 'N',
            "---" => 'O',
            ".--." => 'P',
            "--.-"  => 'Q',
            ".-." => 'R',
            "..." => 'S',
            "-"  => 'T',
            "..-"  => 'U',
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
        foreach (char c in text) {
            if (c == ' ') {
                buffer += '/';
                continue;
            }
            
            buffer += LetterToSequence(c) + ' ';
        }
        
        return buffer;
    }

    public string GenerateToWords(string code)
    {
        string[] message = code.Split(" ");
        string result = "";
        for(int i = 0; i < message.Length; i++)
        {
            result += SequenceToLetter(message[i]);
        }
       
        return result;
    }
    public void Start()
    {
        Debug.Log(GenerateMorseCode("The quick brown fox jumps over the lazy dog"));
    }
}
