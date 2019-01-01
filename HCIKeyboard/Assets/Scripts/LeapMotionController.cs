using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Leap;
using Leap.Unity;
using System;

/*
 * 这个类用来控制Leap Motion的相关性能
 * 不会在Update()中进行什么操作，而是新建一个监听线程，每隔0.05秒扫描一次指尖位置，并新开一个线程
 * 所有线程都会改动几个可public访问的接口
 */

public class LeapMotionController : MonoBehaviour {

    private double DeltaZ = 0.03;
    private double ButtonInterval = 0.025;

    private Vector3 LeftHandCenter;

    private Vector3 RightHandCenter;

    private Vector3 LeftHand;
    private Vector3 RightHand;
    private LeapProvider mProvider;

    private TextMesh xMesh;
    private TextMesh yMesh;
    private TextMesh zMesh;

    private Frame mFrame = null;

    public int CursorPosition = 5;
    public bool CursorDown = false;
    public int SelectPosition = 0;
    public bool SelectDown = false;
    public bool BackSpaceDown = false;
    public bool SelectLeft = false;
    public bool SelectRight = false;

    private Hand GetLeftHand() {
        
        if (mFrame == null || mFrame.Hands == null || mFrame.Hands.Count == 0) {
            return null;
        }
        //遍历所有的手
        mFrame.Hands.Sort((Hand x, Hand y) => (x.WristPosition.x < y.WristPosition.x ? -1 : 1));
        if (mFrame.Hands.Count > 1 && mFrame.Hands[1].IsRight) {
            return mFrame.Hands[1];
        } else if (mFrame.Hands.Count > 0 && mFrame.Hands[0].IsRight) {
            return mFrame.Hands[0];
        } else {
            return null;
        }

    }

    private Hand GetRightHand() {

        if (mFrame == null || mFrame.Hands == null || mFrame.Hands.Count == 0) {
            return null;
        }

        //遍历所有的手
        mFrame.Hands.Sort((Hand x, Hand y) => (x.WristPosition.x < y.WristPosition.x ? -1 : 1));
        if (mFrame.Hands.Count > 0 && mFrame.Hands[0].IsLeft) {
            return mFrame.Hands[0];
        } else if (mFrame.Hands.Count > 1 && mFrame.Hands[1].IsLeft) {
            return mFrame.Hands[1];
        } else {
            return null;
        }

        
    }

    private Vector3 GetVector3(Vector vector) {
        return new Vector3(vector.x, vector.y, vector.z);
    }

    private Vector3 GetRawLeftHandPosition() {
        Hand LeftHand = GetLeftHand();
        if (LeftHand == null) {
            return Vector3.zero;
        } else {
            return GetVector3(LeftHand.PalmPosition);
        }
    }

    private Vector3 GetRawRightHandPosition() {
        Hand RightHand = GetRightHand();
        if (RightHand == null || RightHand.Fingers == null || RightHand.Fingers.Count < 2 || RightHand.Fingers[1] == null) {
            return Vector3.zero;
        } else {
            Bone Distal = RightHand.Fingers[1].Bone(Bone.BoneType.TYPE_DISTAL);
            if (Distal == null) {
                return Vector3.zero;
            }
            return GetVector3(Distal.Center);
        }
    }

    private Vector3 LeftHandPosition() {
        Vector3 Raw = GetRawLeftHandPosition();
        if (Raw.Equals(Vector3.zero) || (Raw - LeftHand).magnitude < 识别误差) {
            return LeftHand;
        } else {
            return LeftHand = Raw;
        }
    }

    private Vector3 RightHandPosition() {
        Vector3 Raw = GetRawRightHandPosition();
        if (Raw.Equals(Vector3.zero) || (Raw - RightHand).magnitude < 识别误差) {
            return RightHand;
        } else {
            return RightHand = Raw;
        }
    }

    private double 校准间隔时间 = 0.2;
    private double 识别误差 = 0.01;
    private double 快速移动阈值 = 0.05;

    private Queue<string> xMeshQueue = new Queue<string>();
    private bool reset = false;

    private int SelectPositionRaw = 0;
    private int SelectPositionRawCount = 0;
    private double RightHandAvgX = 0.0;
    private double RightHandSumX = 0.0;
    private int RightHandCount = 0;

    private double LeftHandAvgX = 0.0;
    private double LeftHandSumX = 0.0;
    private int LeftHandCount = 0;

    private void UpdateSelectPosition() {
        Hand LeftHand = GetLeftHand();
        if (LeftHand != null && LeftHand.Fingers != null) {
            int Raw =  LeftHand.Fingers.FindAll((Finger f) => f.IsExtended).Count;
            if (Raw == SelectPositionRaw) {
                if (++SelectPositionRawCount > 5) {
                    SelectPosition = SelectPositionRaw;
                }
            } else {
                SelectPositionRaw = Raw;
                SelectPositionRawCount = 0;
            }
        } else {
            SelectPositionRaw = 0;
            SelectPositionRawCount = 0;
        }
    }

    private void UpdateBackSpace() {
        Vector3 RightHandPos = RightHandPosition();
        if (!RightHandPos.Equals(Vector3.zero)) {
            RightHandSumX += RightHandPos.x;
            if (++RightHandCount >= 5) {
                double Raw = RightHandSumX / RightHandCount;
                if (Raw < RightHandAvgX - 快速移动阈值) {
                    BackSpaceDown = true;
                } else if (Raw > RightHandAvgX + 识别误差) {
                    BackSpaceDown = false;
                }
                RightHandAvgX = Raw;
                RightHandSumX = 0.0;
                RightHandCount = 0;
            }
        }
    }

    private void UpdateSwitchPage() {
        Hand L = GetLeftHand();
        Vector3 LeftHandPos = LeftHandPosition();
        if (!LeftHandPos.Equals(Vector3.zero)) {
            LeftHandSumX += LeftHandPos.x;
            if (++LeftHandCount >= 5) {
                double Raw = LeftHandSumX / LeftHandCount;
                if (Raw < LeftHandAvgX - 快速移动阈值) {
                    SelectRight = false;
                    if (L != null  && L.PalmNormal.x < 0) {
                        SelectLeft = true;
                    }
                }
                if (Raw > LeftHandAvgX + 快速移动阈值) {
                    SelectLeft = false;
                    if (L != null && L.PalmNormal.x > 0) {
                        SelectRight = true;
                    }
                    
                }
                if (SelectLeft && Raw > LeftHandAvgX + 识别误差) {
                    SelectLeft = false;
                }
                if (SelectRight && Raw < LeftHandAvgX - 识别误差) {
                    SelectRight = false;
                }
                LeftHandAvgX = Raw;
                LeftHandSumX = 0.0;
                LeftHandCount = 0;
            }
        }
    }

    // Use this for initialization
    void Start () {

        mProvider = FindObjectOfType<LeapProvider>() as LeapProvider;
        mFrame = mProvider.CurrentFrame;
        xMesh = GameObject.Find("x tip").GetComponent<TextMesh>();
        yMesh = GameObject.Find("y tip").GetComponent<TextMesh>();
        zMesh = GameObject.Find("z tip").GetComponent<TextMesh>();
        LeftHandCenter = Vector3.zero;
        RightHandCenter = Vector3.zero;
        LeftHand = RightHand = Vector3.zero;

        bool 校准成功 = false;
        System.Action 校准 = () => {
            xMeshQueue.Enqueue("左手握拳，将右手食指放在你认为合适的“5”键上方位置并持续3秒以完成校准");
            bool LeftSuccess = false;
            bool RightSuccess = false;
            double Interval = 0.0;
            while (Interval < 3.0) {
                Interval += 校准间隔时间;
                Thread.Sleep((int)(校准间隔时间 * 1000));
                Vector3 LeftPosition = GetRawLeftHandPosition();
                Vector3 RightPosition = GetRawRightHandPosition();
                if (LeftPosition.Equals(Vector3.zero) || RightPosition.Equals(Vector3.zero)) {
                    LeftSuccess = RightSuccess = false;
                    LeftHandCenter = RightHandCenter = Vector3.zero;
                    continue;
                }
                if ((LeftPosition - LeftHandCenter).magnitude > 识别误差) {
                    LeftHandCenter = LeftPosition;
                    LeftSuccess = false;
                } else {
                    LeftSuccess = true;
                }
                if ((RightPosition - RightHandCenter).magnitude > 识别误差) {
                    RightHandCenter = RightPosition;
                    RightSuccess = false;
                } else {
                    RightSuccess = true;
                }
            }
            if (LeftSuccess && RightSuccess) {
                xMeshQueue.Enqueue("校准成功");
                校准成功 = true;
            } else {
                xMeshQueue.Enqueue("校准失败");
                校准成功 = false;
                Thread.Sleep(200);
            }
        };
        new Thread(() => {
            while (!校准成功) {
                校准();
            }
            LeftHand = LeftHandCenter;
            RightHand = RightHandCenter;
            reset = true;
        }).Start();
    }

    private string ShowVector(Vector3 vector) {
        return "(" + vector.x.ToString() + "," + vector.y.ToString() + "," + vector.z.ToString() + ")";
    }
	
	// Update is called once per frame
	void Update () {

        lock (xMeshQueue) {
            if (xMeshQueue.Count > 0) {
                string Content = xMeshQueue.Dequeue();
                xMesh.text = Content;
            }
        }
        mFrame = mProvider.CurrentFrame;
        Vector3 LeftPosition = LeftHandPosition();
        Vector3 RightPosition = RightHandPosition();

        if (RightPosition.x < RightHandCenter.x - ButtonInterval) {
            if (RightPosition.y < RightHandCenter.y - ButtonInterval) {
                CursorPosition = 7;
            } else if (RightPosition.y < RightHandCenter.y + ButtonInterval) {
                CursorPosition = 4;
            } else {
                CursorPosition = 1;
            }
        } else if (RightPosition.x < RightHandCenter.x + ButtonInterval) {
            if (RightPosition.y < RightHandCenter.y - ButtonInterval) {
                CursorPosition = 8;
            } else if (RightPosition.y < RightHandCenter.y + ButtonInterval) {
                CursorPosition = 5;
            } else {
                CursorPosition = 2;
            }
        } else {
            if (RightPosition.y < RightHandCenter.y - ButtonInterval) {
                CursorPosition = 9;
            } else if (RightPosition.y < RightHandCenter.y + ButtonInterval) {
                CursorPosition = 6;
            } else {
                CursorPosition = 3;
            }
        }

        if (RightPosition.z > RightHandCenter.z + DeltaZ) {
            Hand RHand = GetRightHand();
            if (RHand != null && RHand.Fingers != null && RHand.Fingers.Count > 0 && RHand.Fingers.TrueForAll((Finger f) => !f.IsExtended)) {
                SelectDown = true;
                CursorDown = false;
            }  else {
                CursorDown = true;
                SelectDown = false;
            }
            
        } else {
            SelectDown = false;
            CursorDown = false;
        }

        if (reset) {
            UpdateSelectPosition();
            UpdateBackSpace();
            UpdateSwitchPage();
            xMesh.text = "光标：" + CursorPosition.ToString();
            xMesh.text += " 按下：" + CursorDown.ToString();
            xMesh.text += " 数字：" + SelectPosition.ToString();
            xMesh.text += " 选词：" + SelectDown.ToString();
        }
        
        yMesh.text = "左滑：" + SelectLeft.ToString() + " 右滑：" + SelectRight.ToString() + " 左手坐标：" + ShowVector(LeftPosition);
        zMesh.text = "退格：" + BackSpaceDown.ToString() + " 右手坐标：" + ShowVector(RightPosition);
        
    }
}
