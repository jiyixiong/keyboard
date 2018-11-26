using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardComponent : MonoBehaviour
{
    public const string SPACE = "  ";
    public const string BACK = "Back";
    public const string ABC = "ABC";
    public const string QEH = "123\n?!#";
    public const string UP = "UP";
    public const string LOW = "low";

    public const int CENTER_ITEM = 15;
    public const int KEY_NUMBER = 30;
    public const int POSITION_SPACE = 28;

    public enum KeyLetterEnum
    {
        LowerCase, UpperCase, NonLetters
    }

    public static readonly string[] allLettersLowercase = new string[]
    {
        "q","w","e","r","t","y","u","i","o","p",
        "a","s","d","f","g","h","j","k","l",
        UP,"z","x","c","v","b","n","m",
        QEH,SPACE,BACK
    };

    public static readonly string[] allLettersUppercase = new string[]
    {
        "Q","W","E","R","T","Y","U","I","O","P",
        "A","S","D","F","G","H","J","K","L",
        LOW,"Z","X","C","V","B","N","M",
        QEH,SPACE,BACK
    };

    public static readonly string[] allSpecials = new string[]
    {
        "1","2","3","4","5","6","7","8","9","0",
        "@","#","£","_","&","-","+","(",")",
        "*","\"","'",":",";","/","!","?",
        ABC,SPACE,BACK
    };
}

