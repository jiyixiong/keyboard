using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : KeyboardComponent
{
    [SerializeField]
    private string clickHandle;
	
	 [SerializeField]
    private GameObject leftkeyboard;
	 [SerializeField]
    private GameObject rightkeyboard;
	[SerializeField]
    private KeyboardItem[] leftkeys;
	[SerializeField]
	private KeyboardItem[] rightkeys;


    //Materials
    [SerializeField]
    private Material keyNormalMaterial;
    [SerializeField]
    private Material keySelectedMaterial;
    [SerializeField]
    private Material keyPressedMaterial;

    public bool wasStaticOnStart;

    //borders
    private float leftBorder;
    private float rightBorder;
    private float topBorder;
    private float bottomBorder;

    //Methods
    void Awake()
    {
		Debug.Log("creator");
        InitKeys();
        ChangeMaterialOnKeys();
        ManageKeys();
        SetComponents();
    }

    public void InitKeys()
    {
        if (leftkeys == null || KeyboardItem.forceInit)
        {
            List<KeyboardItem> allleftKeys = new List<KeyboardItem>(this.leftkeyboard.GetComponentsInChildren<KeyboardItem>());
            for (int i = 0; i < allleftKeys.Count; i++)
            {
                allleftKeys[i].position = i;
                allleftKeys[i].Init();
				allleftKeys[i].isLeft = true;
            }
            leftkeys = allleftKeys.ToArray();
        }
		if (rightkeys == null || KeyboardItem.forceInit)
        {
            List<KeyboardItem> allrightKeys = new List<KeyboardItem>(this.rightkeyboard.GetComponentsInChildren<KeyboardItem>());
            for (int i = 0; i < allrightKeys.Count; i++)
            {
                allrightKeys[i].position = i;
                allrightKeys[i].Init();
				allrightKeys[i].isLeft = false;
            }
            rightkeys = allrightKeys.ToArray();
        }
    }

    public void ChangeMaterialOnKeys()
    {
        foreach (KeyboardItem key in leftkeys)
            key.SetMaterials(keyNormalMaterial, keySelectedMaterial, keyPressedMaterial);
		foreach (KeyboardItem key in rightkeys)
            key.SetMaterials(keyNormalMaterial, keySelectedMaterial, keyPressedMaterial);
    }

    public void ManageKeys()
    {
        FillAndPlaceKeys();
    }

    private void FillAndPlaceKeys()
    {
		Debug.Log("set keys");
		foreach(KeyboardItem lkey in leftkeys)
			lkey.SetKeyText(0);
		foreach(KeyboardItem rkey in rightkeys)
			rkey.SetKeyText(-1);
    }

    private void SetComponents()
    {
	
	  LeftKeyboard leftboard = leftkeyboard.GetComponent<LeftKeyboard>();
	  leftboard.SetKeys(leftkeys);
	  RightKeyboard rightboard = rightkeyboard.GetComponent<RightKeyboard>();
	  rightboard.SetKeys(rightkeys);

    }

    public string ClickHandle
    {
        get
        {
            return clickHandle;
        }
        set
        {
          //  clickHandle = value;
            //KeyboardTaskRaycaster rayCaster = GetComponent<KeyboardTaskRaycaster>();
            //rayCaster.SetClickButton(clickHandle);
        }
    }

    private void ChangeBorders(Vector4 newBorder)
    {
        leftBorder = newBorder.x;
        bottomBorder = newBorder.y;
        rightBorder = newBorder.z;
        topBorder = newBorder.w;
    }
}
