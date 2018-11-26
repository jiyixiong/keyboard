using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardTaskCreator : KeyboardComponent
{
    [SerializeField]
    private string clickHandle;

    private KeyboardItem[] keys;

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
        InitKeys();
        ChangeMaterialOnKeys();
        if (!Application.isPlaying)
            ManageKeys();
        SetComponents();
    }

    public void InitKeys()
    {
        if (keys == null || KeyboardItem.forceInit)
        {
            List<KeyboardItem> allKeys = new List<KeyboardItem>(GetComponentsInChildren<KeyboardItem>());
            for (int i = 0; i < allKeys.Count; i++)
            {
                allKeys[i].position = i;
                allKeys[i].Init();
            }
            keys = allKeys.ToArray();
        }
    }

    public void ChangeMaterialOnKeys()
    {
        foreach (KeyboardItem key in keys)
            key.SetMaterials(keyNormalMaterial, keySelectedMaterial, keyPressedMaterial);
    }

    public void ManageKeys()
    {
        FillAndPlaceKeys();
    }

    private void FillAndPlaceKeys()
    {
        foreach (KeyboardItem key in keys)
            key.SetKeyText(KeyboardItem.KeyLetterEnum.LowerCase);
    }

    private void SetComponents()
    {
       // KeyboardTaskRaycaster rayCaster = GetComponent<KeyboardTaskRaycaster>();
        //rayCaster.SetClickButton(ClickHandle);
        KeyboardStatus status = GetComponent<KeyboardStatus>();
      //  status.SetKeys(keys);
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
