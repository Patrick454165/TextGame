using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderController : MonoBehaviour
{
    public Slider slider;
    public float sValue;
    public TMP_Text storyText; // the story 
    public TMP_Text userInput; // the input field object
    public TMP_Text inputText; // part of the input field where user enters response
    public TMP_Text placeHolderText; // part of the input field for initial placeholder text
    public Text toggleText;
    public Text sliderText;
    public float textMin;
    public float textMax;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider.onValueChanged.AddListener(delegate {onSlide(); });
        slider.value = PlayerPrefs.GetFloat("sValue", .5f);
        sValue = slider.value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onSlide()
    {
        PlayerPrefs.SetFloat("sValue", sValue);
        sValue=slider.value;
        storyText.fontSize=(Mathf.Lerp(textMin, textMax, sValue)*0.08428571429f); 
        userInput.fontSize=Mathf.Lerp(textMin, textMax, sValue);; 
        inputText.fontSize=Mathf.Lerp(textMin, textMax, sValue);; 
        placeHolderText.fontSize=Mathf.Lerp(textMin, textMax, sValue);; 
        toggleText.fontSize=(int)Mathf.Lerp(textMin, textMax-2, sValue);;
        sliderText.fontSize=(int)Mathf.Lerp(textMin, textMax, sValue);;
    }
}
