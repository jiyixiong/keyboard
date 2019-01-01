using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardComponent : MonoBehaviour
{
    // public const string SPACE = "  ";
    // public const string BACK = "Back";
    // public const string ABC = "ABC";
    // public const string QEH = "123\n?!#";
    // public const string UP = "UP";
    // public const string LOW = "low";

    public const string K11 = "1 0\n";
    public const string K12 = "2\nABC";
    public const string K13 = "3\nDEF";
    public const string K21 = "4\nGHI";
    public const string K22 = "5\nJKL";
    public const string K23 = "6\nMNO";
    public const string K31 = "7\nPQRS";
    public const string K32 = "8\nTUV";
    public const string K33 = "9\nWXYZ";

    // public const string K11 = "1 0\ncap.,";
    // public const string K12 = "2\nABC;";
    // public const string K13 = "3\nDEF'";
    // public const string K21 = "4\nGHI";
    // public const string K22 = "5\nJKL?";
    // public const string K23 = "6\nMNO\"";
    // public const string K31 = "7\nPQRS";
    // public const string K32 = "8\nTUV:";
    // public const string K33 = "9\nWXYZ";

    // public const int CENTER_ITEM = 15;
    // public const int KEY_NUMBER = 30;
    // public const int POSITION_SPACE = 28;

    // public enum KeyLetterEnum
    // {
    //     LowerCase, UpperCase, NonLetters
    // }
    
    public static readonly string[] leftLetters = new string[]
    {
        "0",    "cap",  "1",    ",",    ".",
        "A",    "B",    "2",    "C",    ";",
        "D",    "E",    "3",    "F",    "'",
        "G",    "H",    "4",    "I",    " ",
        "J",    "K",    "5",    "L",    "?", 
        "M",    "N",    "6",    "O",    "\"",
        "P",    "Q",    "7",    "R",    "S",
        "T",    "U",    "8",    "V",    ":", 
        "W",    "X",    "9",    "Y",    "Z"
    };

    public static readonly string[] rightLetters = new string[]
    {
        K11, K12, K13, K21, K22, K23, K31, K32, K33
    };

    // public static readonly string[] allLettersUppercase = new string[]
    // {
    //     "Q","W","E","R","T","Y","U","I","O","P",
    //     "A","S","D","F","G","H","J","K","L",
    //     LOW,"Z","X","C","V","B","N","M",
    //     QEH,SPACE,BACK
    // };

    // public static readonly string[] allSpecials = new string[]
    // {
    //     "1","2","3","4","5","6","7","8","9","0",
    //     "@","#","£","_","&","-","+","(",")",
    //     "*","\"","'",":",";","/","!","?",
    //     ABC,SPACE,BACK
    // };
}

