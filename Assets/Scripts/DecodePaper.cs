using System.Collections.Generic;
using UnityEngine;

public class DecodePaper : MonoBehaviour
{
    private string LetterToSequence(char letter)
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
    
    private string GenerateMorseCode(string text)
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

    public void Start()
    {
        Debug.Log(GenerateMorseCode("The quick brown fox jumps over the lazy dog"));
    }
}
