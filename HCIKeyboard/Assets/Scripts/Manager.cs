using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField] private Text input;//User input
    [SerializeField] private Text instruction;//Instructions

    //private List<string> words;//pool of instructions
    private string[] words;
    [SerializeField] private int index;//instruction index

    [SerializeField]
    private GameObject leftkeyboard;
    private GameObject rightkeyboard;
    private KeyboardStatus keyboardstatus;
    private LeftKeyboard leftKeyboardstatus;
    private RightKeyboard rightKeyboardstatus;


    //Methods
    void Start()
    {


        leftKeyboardstatus = leftkeyboard.GetComponent<LeftKeyboard>();//get status
        rightKeyboardstatus  = rightkeyboard.GetComponent<RightKeyboard>();//get status
        Debug.Log("manager");
        /*TextAsset txtAsset = (TextAsset)Resources.Load("phrases", typeof(TextAsset));

        words = txtAsset.text.Split('\n');
        Debug.Log(words[0].Length);
        
        if (index >= words.Length)//check for error
        {
            Debug.Log("Start index is incorrect, please check it out.");
            index = 0;
        }
        words[index] = words[index].Remove(words[index].Length - 1, 1);
        Debug.Log(words[0].Length);
        instruction.text = String.Format("\"{0}\" ", words[index]);//give instruction*/
    }

    void Update()
    {
        /*
        if (input.text == words[index])//check that whether input  is correct
        {
            keyboardstatus.output = "";
            index += 1;
            input.text = "";
            if (index >= words.Length)
                index = 0;
            instruction.text = String.Format(" \"{0}\" ", words[index]);
        }*/
    }

}
