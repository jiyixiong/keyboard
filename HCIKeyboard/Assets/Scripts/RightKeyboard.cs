using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightKeyboard : KeyboardComponent
{
    [SerializeField]
    public string output;
    public int maxOutputLength = 30;
    [SerializeField]
    public Text targetGameObject;
    public Component typeHolder;
    private Component textComponent;

    [SerializeField]
    public bool isReflectionPossible;

    private KeyboardItem[] keys;
    private bool areLettersActive = true;
    private bool isLowercase = true;

    private const char BLANKSPACE = ' ';
    private const string TEXT = "text";

    //Methods
    void Awake()
    {
        //targetGameObject = GameObject.Find("input");
        //typeHolder = targetGameObject.GetComponent<UnityEngine.UI.Text>();
        //Debug.Log(targetGameObject);
        //Debug.Log(typeHolder);
    }

    public void SetKeys(KeyboardItem[] keys)
    {
        Debug.Log("Right Status Creat");
        this.keys = keys;
    }

    public void HandleClick(KeyboardItem clicked)
    {
        // string value = clicked.GetLetter();
        // if (value.Equals(QEH) || value.Equals(ABC))// special signs pressed
        //     ChangeSpecialLetters();
        // else if (value.Equals(UP) || value.Equals(LOW)) // upper/lower case pressed
        //     LowerUpperKeys();
        // else if (value.Equals(SPACE))
        //     TypeKey(BLANKSPACE);
        // else if (value.Equals(BACK))
        //     BackspaceKey();
        // else// Normal letter
        //     TypeKey(value[0]);
    }

    public int getClick()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i].held)
            {
                if(!keys[i].clicked)
                    {
                        keys[i].Click();
                        return keys[i].position;
                    }
            }
        }
        return -1;
    }
   public void cancelClick()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i].clicked = false;
        }
    }

    public void setHold(int pos)
    {
        if(pos>0)
            this.keys[pos-1].GetHolding();
    }
    public void cancelHold(int pos)
    {
        if(pos>0)
            this.keys[pos-1].unHold();
    }
    //Change keyboard
    private void ChangeSpecialLetters()
    {
        // KeyLetterEnum ToDisplay = areLettersActive ? KeyLetterEnum.NonLetters : KeyLetterEnum.LowerCase;
        // areLettersActive = !areLettersActive;
        // isLowercase = true;
        // for (int i = 0; i < keys.Length; i++)
        //     keys[i].SetKeyText(ToDisplay);
    }

    private void LowerUpperKeys()
    {
        // KeyLetterEnum ToDisplay = isLowercase ? KeyLetterEnum.UpperCase : KeyLetterEnum.LowerCase;
        // isLowercase = !isLowercase;
        // for (int i = 0; i < keys.Length; i++)
        //     keys[i].SetKeyText(ToDisplay);
    }

    //Delete
    private void BackspaceKey()
    {
        // if (output.Length >= 1)
        // {
        //     //textComponent = targetGameObject.GetComponent(typeHolder.GetType());
        //     //textComponent.GetType().GetProperty(TEXT).SetValue(textComponent, output.Remove(output.Length - 1, 1), null);
        //     targetGameObject.text = output.Remove(output.Length - 1, 1);
        //     output = output.Remove(output.Length - 1, 1);
        // }
    }

    //Input
    private void TypeKey(char key)
    {
        // if (output.Length < maxOutputLength)
        // {
        //     // textComponent = targetGameObject.GetComponent(typeHolder.GetType());
        //     targetGameObject.text = output + key.ToString();
        //     output = output + key.ToString();
        // }
    }

    public void setOutput(ref string stringRef)
    {
        output = stringRef;
    }
}
