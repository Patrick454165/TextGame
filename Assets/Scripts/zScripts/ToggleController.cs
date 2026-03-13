using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ToggleController : MonoBehaviour
{
    public static InputManager instance;

    public TMP_Text storyText; // the story 
    public TMP_Text userInput; // the input field object
    public TMP_Text inputText; // part of the input field where user enters response
    public TMP_Text placeHolderText; // part of the input field for initial placeholder text
    public Text toggleText;
    public Text sliderText;
    public ScrollRect scrollRect; //scrolls
    public Image background;
    public Image input_field;
    private bool darkmode;
    private Toggle toggle;

    //font size
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //PlayerPrefs.HasKey("darkmode")
        darkmode = PlayerPrefs.GetInt("darkmode", 1) == 1 ? true : false; //1 is true for dark mode
        toggle = GetComponent<Toggle>();
        SetTheme();

        toggle.onValueChanged.AddListener(UpdateTheme);
    }

    void UpdateTheme(bool isChecked)
    {
        darkmode = isChecked;
        PlayerPrefs.SetInt("darkmode", darkmode ? 1 : 0);
        SetTheme();
    }

    void SetTheme()
    {
        if (darkmode)
        {
            toggle.isOn = true;
            background.color = Color.black;
            storyText.color = Color.white;
            inputText.color = Color.white;
            placeHolderText.color = Color.white;
            toggleText.color = Color.white;
            userInput.color = Color.white;
            input_field.color = Color.black;
            sliderText.color = Color.white;
        }
        else
        {
            toggle.isOn = false;
            background.color = Color.white;
            storyText.color = Color.black;
            inputText.color = Color.black;
            placeHolderText.color = Color.black;
            toggleText.color = Color.black;
            userInput.color = Color.black;
            input_field.color = Color.white;
            sliderText.color = Color.black;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
