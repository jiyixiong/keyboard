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

    //Keyboard
    [SerializeField] private GameObject leftkeyboard;
    [SerializeField] private GameObject rightkeyboard;
    //private KeyboardStatus keyboardstatus;
    private LeftKeyboard leftKeyboardstatus;
    private RightKeyboard rightKeyboardstatus;

    //Input box
    [SerializeField] private TextMesh inputText;
    [SerializeField] private TextMesh hintsText; 

    //const color
    private string red_left = "<color=#ff0000>";
    private string red_right = "</color>";

    private NumToStrList translator;
    private List<string> results;


    private int cur_pos;
    private bool cursor_down;
    private bool select_down;
    private bool backsapce;
    private bool left_slide;
    private bool right_slide;


    private string query_string = "";
    private int level;
    private bool only_right = true;

    //test
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


    //[SerializeField] private PositionMapper mapper;
    [SerializeField] private LeapMotionController controller;

   
    //Methods
    void Start()
    {
        initKeyboard();
        //listenCursordown();
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
        //checkOnlyRight();
        //cleanText();

        
            listenCursor();
            listenCursordown();
            listenSelectdown();
            listenBack();
            listenSelectGestrue();
            listenGesture();
            Debug.Log(query_string);
        
    }

     void initKeyboard()
    {
        leftKeyboardstatus = leftkeyboard.GetComponent<LeftKeyboard>();//get status
        rightKeyboardstatus = rightkeyboard.GetComponent<RightKeyboard>();//get status
        controller = GetComponent<LeapMotionController>();//get listener
        //mProvider = FindObjectOfType<LeapProvider>() as LeapProvider;
        inputText = GameObject.Find("input").GetComponent<TextMesh>();
        inputText.text = "";
        hintsText = GameObject.Find("hints").GetComponent<TextMesh>();
        hintsText.text = "";
        
        query_string = "";
        level = 0;
        results = new List<string>();
        translator = new NumToStrList();

        only_right = true;

        cursor_down = false;
        select_down = false;
        backsapce = false;
        left_slide = false;
        right_slide = false;

        cur_pos = -1;

        Debug.Log("manager init");
    }

    void checkOnlyRight()
    {
        if (true)//check gesture
            only_right = true;
    }

    void listenCursor()
    {
        int now_pos = controller.CursorPosition;
        //Debug.Log(now_pos);
        if(now_pos != cur_pos)
        {
            rightKeyboardstatus.cancelHold(cur_pos);
            cur_pos = now_pos;
            rightKeyboardstatus.setHold(cur_pos);
        }
    }
    void listenCursordown()
    {
        if(controller.CursorDown && !cursor_down)
        {
            this.level = 0;
            int pos = controller.CursorPosition;
            query_string += pos.ToString();
            
            this.results = translator.getStrList(query_string,level);//query
            setHints(1);
            cursor_down = true;
        }
        else if(!controller.CursorDown)
            cursor_down =false;

        //old version
        // if (controller.LeftCursorDown)
        // {
        //     string letter = leftKeyboardstatus.getClick();
        //     if (!letter.Equals("") && !only_right)
        //     {
        //         addInputText(letter);
        //     }

        // }
        // else
        //     leftKeyboardstatus.cancelClick();
    }
    void listenSelectdown()
    {
        if(controller.SelectDown && !select_down)
        {
            int finger = controller.SelectPosition;
            if(finger == 0)
            {
                addInputText(" ");
                select_down = true;
            }
            else if(finger < results.Count)
            {
                addInputText(results[finger-1]);
                query_string = "";
                level = 0;
                results.Clear();
                setHints(1);
                select_down = true;
            }
            else
            {
                select_down = true;
                return;
            }

        }
        else if(!controller.SelectDown)
            select_down = false;
        //
        // if (controller.RightCursorDown)
        // {
        //     int btn = rightKeyboardstatus.getClick();
        //     if (btn != -1)
        //         {
        //             Debug.Log(btn);
        //             query_string += btn.ToString();
        //             Debug.Log(query_string);
        //             //get results
        //             //set hints
        //             leftKeyboardstatus.setBtn(btn);
        //         }

        // }
        // else
        //     rightKeyboardstatus.cancelClick();
    }

    void listenBack()
    {
        if(controller.BackSpaceDown && !backsapce)
        {
            Debug.Log("get back");
            backsapce = true;
            if(query_string.Length > 0)
            {
                query_string = query_string.Remove(query_string.Length - 1);
                level = 0;
                
                this.results = translator.getStrList(query_string,level);//query
                if(query_string.Length == 0)
                    this.results.Clear();
                setHints(1);
            }
            else
                delInputText();
        }
        else if(!controller.BackSpaceDown)
            backsapce = false;
    }

    void listenSelectGestrue()
    {
        int index = controller.SelectPosition;
        if(index==0 || index > results.Count)
        {
            setHints(1);
        }
        else
        {
            setHints(index);
        }
    }
    void listenGesture()
    {
        if(controller.SelectLeft&&results.Count==5&&!left_slide)
        {
            level++;
            List<string> cur_results = translator.getStrList(query_string,level);//query
            if(cur_results.Count > 0)
                this.results = cur_results;
            else
                level--;
            setHints(1);
            left_slide = true;
            right_slide = false;
            
        }
        else if(controller.SelectRight&&level>0&&!right_slide)
        {
            level--;
            this.results = translator.getStrList(query_string,level);//query
            setHints(1);

            left_slide = false;
            right_slide = true;
        }
        else
        {  
            left_slide = false;  
            right_slide = false;
            return;
        }
    }
    public void addInputText(String inputString)
    { //输入框加入字符
        inputText.text = inputText.text + inputString;
    }

    public void delInputText()
    { //删除一个字符
        if(inputText.text != "")
            inputText.text = inputText.text.Remove(inputText.text.Length - 1);
    }

    void setHints(int index)
    {
        string hints = "";
        for(int i=0; i<this.results.Count; i++)
        {
            if(i==index-1)
            { 
                hints += red_left;
                hints += results[i];
                hints += red_right;
                hints += "  ";
            }
            else
            {
                hints += results[i];
                hints += "  ";
            }
        }
        hintsText.text = hints;
    }




    protected bool isCloseHand(Hand hand)     //是否握拳 
    {
        List<Finger> listOfFingers = hand.Fingers;
        int count = 0;
        for (int f = 0; f < listOfFingers.Count; f++)
        { //循环遍历所有的手~~
            Finger finger = listOfFingers[f];
            if ((finger.TipPosition - hand.PalmPosition).Magnitude < 0.06f)    // Magnitude  向量的长度 。是(x*x+y*y+z*z)的平方根。
                                                                               //float deltaCloseFinger = 0.05f;
            {
                count++;
                //if (finger.Type == Finger.FingerType.TYPE_THUMB)
                //Debug.Log ((finger.TipPosition - hand.PalmPosition).Magnitude);
            }
        }
        return (count == 5);
    }
       void cleanText()
    {
		  mFrame = mProvider.CurrentFrame;
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

                    delInputText();
    }

}
