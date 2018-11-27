using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Leap;
using Leap.Unity;


public class PositionMapper : MonoBehaviour {

    public Vector3 LeftCursorPosition;
    public Vector3 RightCursorPosition;
    public bool LeftCursorDown = false;
    public bool RightCursorDown = false;

    [SerializeField]
    [Tooltip("竖直方向认为按下的阈值长度")]
    private float ThresoldDeltaz;

    [SerializeField]
    [Tooltip("按钮之间九宫格相邻两个中心距离")]
    private float ButtonInterval;

    [SerializeField]
    [Tooltip("左手的中心坐标")]
    private Vector3 LeftHandCenter;

    [SerializeField]
    [Tooltip("右手的中心坐标")]
    private Vector3 RightHandCenter;

    [SerializeField]
    private LeapProvider mProvider;

    private TextMesh xMesh;
    private TextMesh yMesh;
    private TextMesh zMesh;

    //用于设置参数，现在不动
    private bool reseting = false;

    private Vector3 LeftKeyboardCenter;
    private Vector3 RightKeyboardCenter;

    [SerializeField]
    private GameObject LeftCursor;
    [SerializeField]
    private GameObject RightCursor;

    int frameId = 0;

    Vector2 PositionMap3to2(Vector3 Position3) {
        return Camera.main.WorldToScreenPoint(Position3);
    }

    // Use this for initialization
    void Start () {
        mProvider = FindObjectOfType<LeapProvider>() as LeapProvider;
        xMesh = GameObject.Find("x tip").GetComponent<TextMesh>();
        yMesh = GameObject.Find("y tip").GetComponent<TextMesh>();
        zMesh = GameObject.Find("z tip").GetComponent<TextMesh>();

        LeftKeyboardCenter = GameObject.Find("leftKeyboard").transform.position;
        RightKeyboardCenter = GameObject.Find("rightKeyboard").transform.position;
        ResetArguments();
	}

    void ResetArguments() {
        frameId = 0;
        reseting = true;
    }

	
	// Update is called once per frame
	void Update () {
        frameId++;
        if (frameId % 3 !=0 ) {
            return;
        }
        Frame mFrame = mProvider.CurrentFrame;

        //遍历所有的手
        mFrame.Hands.Sort((Hand x, Hand y) => (x.WristPosition.x < y.WristPosition.x ? -1 : 1));
        bool leftHand = true;
        foreach (Hand hand in mFrame.Hands) {
            if (hand != null && leftHand) {
                //这玩意并不能识别左手，所以认为左边的手是左手
                leftHand = false;
                //查找食指末端位置
                Finger TargetFinger = hand.Fingers[1];
                if (TargetFinger != null) {
                    Bone bone = TargetFinger.Bone(Bone.BoneType.TYPE_DISTAL);
                    if (bone != null) {
                        Vector3 EndPoint = new Vector3(-bone.NextJoint.x, bone.NextJoint.y, bone.NextJoint.z);
                        Vector3 EndPoint3D = (EndPoint - LeftHandCenter) / ButtonInterval + LeftKeyboardCenter;
                        xMesh.text = EndPoint.ToString();
                        EndPoint3D.z = LeftKeyboardCenter.z;
                        LeftCursor.transform.position = LeftCursorPosition = EndPoint3D;
                        if ((EndPoint - LeftHandCenter).z > ThresoldDeltaz) {
                            zMesh.text = "left hand down";
                            LeftCursorDown = true;
                        } else {
                            zMesh.text = "left hand get";
                            LeftCursorDown = false;
                        }
                    }
                }
            } else if (hand != null) {
                Finger TargetFinger = hand.Fingers[1];
                if (TargetFinger != null) {
                    Bone bone = TargetFinger.Bone(Bone.BoneType.TYPE_DISTAL);
                    if (bone != null) {
                        Vector3 EndPoint = new Vector3(-bone.NextJoint.x, bone.NextJoint.y, bone.NextJoint.z);
                        Vector3 EndPoint3D = (EndPoint - RightHandCenter) / ButtonInterval + RightKeyboardCenter;
                        yMesh.text = EndPoint.ToString();
                        EndPoint3D.z = LeftKeyboardCenter.z;
                        RightCursor.transform.position = RightCursorPosition = EndPoint3D;
                        if ((EndPoint - RightHandCenter).z > ThresoldDeltaz) {
                            zMesh.text = "right hand down";
                            RightCursorDown = true;
                        } else {
                            zMesh.text = "right hand get";
                            RightCursorDown = false;
                        }
                    }
                }
            }
        }
    }
}
