using UnityEngine;
using UnityEngine.UI;

public class KeyboardItem : KeyboardComponent
{
    public static bool forceInit = true;

    public bool isLeft = false;

    //information
    private Text letter;
    public int position;
    private Renderer quadFront;
    private const string QUAD_FRONT = "Front";

    //click
    public bool getclicked = false;
    public bool clicked = false;
    public bool held = false;
    private float clickHoldTimer = 0f;
    private float clickHoldTimeLimit = 0.15f;

    //Materials
    [SerializeField, HideInInspector]
    private Material keyNormalMaterial;
    [SerializeField, HideInInspector]
    private Material keySelectedMaterial;
    [SerializeField, HideInInspector]
    private Material keyPressedMaterial;


    //Methods
    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (letter == null || quadFront == null)
        {  // Check if initialized
            letter = gameObject.GetComponentInChildren<Text>();
            //Debug.Log(letter.text);
            quadFront = transform.Find(QUAD_FRONT).GetComponent<Renderer>();
        }
    }

    public enum KeyMaterialEnum//types of material
    {
        Normal, Selected, Pressed
    }

    //Holding or not
    public void Holding()//cursor is on the button
    {
        if (!clicked)// Is not already being clicked?
            ChangeDisplayedMaterial(keySelectedMaterial);
        else
            HoldClick();
    }

    public void StopHolding()//cursor left
    {
        ChangeDisplayedMaterial(keyNormalMaterial);
    }

    public void GetHolding()//cursor left
    {
        ChangeDisplayedMaterial(keySelectedMaterial);
    }

    //click & holdclick
    public void Click()
    {
        clicked = true;
        ChangeDisplayedMaterial(keyPressedMaterial);
    }
    public void unHold()
    {
        ChangeDisplayedMaterial(keyNormalMaterial);
    }

    //delete or not?
    private void HoldClick()//keep pressing
    {
        ChangeDisplayedMaterial(keyPressedMaterial);
        clickHoldTimer += Time.deltaTime;
        if (clickHoldTimer >= clickHoldTimeLimit)
        {// Check if time of click is over
            clicked = false;
            clickHoldTimer = 0f;
        }
    }

    //get information
    //letter
    public string GetLetter()//get button's letter
    {
        return letter.text;
    }
    //position
    //delete or not ?
    public int Position
    {
        get
        {
            return position;
        }
        set
        {
            position = value;
        }
    }

    //MeshName
    //delete or not ?
    public string GetMeshName()
    {
        if (quadFront == null)
            Init();
        return quadFront.GetComponent<MeshFilter>().sharedMesh.name;
    }

    //change information
    //button
    public void SetKeyText(int line)
    {
        string value = "";
        if (this.isLeft)
            value = leftLetters[line * 5 + position];
        else if (!this.isLeft)
        {
            value = rightLetters[position];
            letter.fontSize = 4;
        }
        //Debug.Log(value);
        if (!letter.text.Equals(value))
        {
            letter.text = value;
        }
    }

    //material
    private void ChangeDisplayedMaterial(Material material)
    {
        quadFront.sharedMaterial = material;
    }

    public void SetMaterials(Material keyNormalMaterial, Material keySelectedMaterial, Material keyPressedMaterial)
    {
        this.keyNormalMaterial = keyNormalMaterial;
        this.keySelectedMaterial = keySelectedMaterial;
        this.keyPressedMaterial = keyPressedMaterial;
    }

    void OnTriggerEnter(Collider other)
    {

        string cursorname = other.gameObject.name;
        //Debug.Log(cursorname);
        
        if(cursorname.Equals("LeftCursor")&&this.isLeft)
            {
                GetHolding();
                held = true;
            }
        else if(cursorname.Equals("RightCursor")&&!this.isLeft)
            {
                GetHolding();
                held = true;
            }



    }
    void OnTriggerExit(Collider other)
    {

        string cursorname = other.gameObject.name;
        //Debug.Log(cursorname);
       
        if(cursorname.Equals("LeftCursor")&&this.isLeft)
            {
                StopHolding();
                held = false;
            }
        else if(cursorname.Equals("RightCursor")&&!this.isLeft)
            
            {
                StopHolding();
                held = false;
            }


    }
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("get");
    }
}



