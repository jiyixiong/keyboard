using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leap;
using Leap.Unity;

public class Manager : MonoBehaviour
{
    [SerializeField] private Text input;//User input
    [SerializeField] private Text instruction;//Instructions

    //private List<string> words;//pool of instructions
    private string[] words;
    [SerializeField] private int index;//instruction index

    [SerializeField] private GameObject leftkeyboard;
    [SerializeField] private GameObject rightkeyboard;
    //private KeyboardStatus keyboardstatus;
    private LeftKeyboard leftKeyboardstatus;
    private RightKeyboard rightKeyboardstatus;

    private TextMesh inputText;


    public static bool Gesture_left = false;
    public static bool Gesture_right = false;
    public static bool Gesture_up = false;
    public static bool Gesture_down = false;
    public static bool Gesture_zoom = false;
    public static float movePOs = 0.0f;
 
    [SerializeField] private LeapProvider mProvider;
    private Frame mFrame;
    private Hand mHand;
    private Finger myFinger;
    //是否握拳
    bool myBool = false;
    private Vector leftPosition;
    private Vector rightPosition;
    public static float zoom = 1.0f;
    [Tooltip("Velocity (m/s) of Palm ")]
    public float smallestVelocity = 1.45f;//手掌移动的最小速度
    [Tooltip("Velocity (m/s) of Single Direction ")]
    [Range(0, 1)]
    public float deltaVelocity = 1.0f;//单方向上手掌移动的速度

    //Methods
    void Start()
    {


        leftKeyboardstatus = leftkeyboard.GetComponent<LeftKeyboard>();//get status
        rightKeyboardstatus = rightkeyboard.GetComponent<RightKeyboard>();//get status
		mProvider = FindObjectOfType<LeapProvider>() as LeapProvider;
        inputText = GameObject.Find("inputText").GetComponent<TextMesh>();
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
		  mFrame = mProvider.CurrentFrame;//获取当前帧
                                      //获得手的个数


        //print ("hand num are " + mFrame.Hands.Count);
        
        //判断是否为握拳状态
        //遍历所有的手
        foreach (Hand hand in mFrame.Hands)
        {
           myBool= isCloseHand(hand);
            //Finger myFinger = hand.Finger(3);
            //Debug.Log(myFinger);
            float x= hand.GrabStrength;
            //Debug.Log(x);


            if (hand==null)
            {
                myBool = false;
            }
            
        }
		if(myBool)
			{
            List<KeyboardItem> allleftKeys = new List<KeyboardItem>(this.leftkeyboard.GetComponentsInChildren<KeyboardItem>());
            for (int i = 0; i < allleftKeys.Count; i++)
                allleftKeys[i].GetHolding();
            List<KeyboardItem> allrightKeys = new List<KeyboardItem>(this.rightkeyboard.GetComponentsInChildren<KeyboardItem>());
            for (int i = 0; i < allrightKeys.Count; i++)
                allrightKeys[i].GetHolding();
      
			}
			else{
				List<KeyboardItem> allleftKeys = new List<KeyboardItem>(this.leftkeyboard.GetComponentsInChildren<KeyboardItem>());
            for (int i = 0; i < allleftKeys.Count; i++)
                allleftKeys[i].StopHolding();
            List<KeyboardItem> allrightKeys = new List<KeyboardItem>(this.rightkeyboard.GetComponentsInChildren<KeyboardItem>());
            for (int i = 0; i < allrightKeys.Count; i++)
                allrightKeys[i].StopHolding();
			}
    }
	  protected bool isCloseHand(Hand hand)     //是否握拳 
    {
        List<Finger> listOfFingers = hand.Fingers;
        int count = 0;
        for (int f = 0; f < listOfFingers.Count; f++)
        { //循环遍历所有的手~~
            Finger finger = listOfFingers[f];
            if ((finger.TipPosition - hand.PalmPosition).Magnitude < 0.08f)    // Magnitude  向量的长度 。是(x*x+y*y+z*z)的平方根。
                                                                                          //float deltaCloseFinger = 0.05f;
            {
                count++;
                //if (finger.Type == Finger.FingerType.TYPE_THUMB)
                //Debug.Log ((finger.TipPosition - hand.PalmPosition).Magnitude);
            }
        }
        return (count == 5);
    }

    public void addInputText(String inputString){ //输入框加入字符
        inputText.text = inputText.text + inputString;
    }

    public void delInputText(){ //删除一个字符
        inputText.text = inputText.text.Remove(inputText.text.Length-1);
    }
}
