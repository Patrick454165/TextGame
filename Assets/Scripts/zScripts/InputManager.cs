using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public TMP_Text storyText; // the story 
    public TMP_InputField userInput; // the input field object
    public TMP_Text inputText; // part of the input field where user enters response
    public TMP_Text placeHolderText; // part of the input field for initial placeholder text
    public ScrollRect scrollRect; //scrolls
    
    
    private string story; // holds the story to display
    private List<string> commands = new List<string>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        commands.Add("go");
        commands.Add("get");
        commands.Add("restart");
        story = storyText.text;
        userInput.onEndEdit.AddListener(GetInput);

        //NavigationManager.instance.onRestart += GameReset;
    }

    IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();

        scrollRect.verticalNormalizedPosition = 0f; //move to bottom.
    }


    void GetInput(string input)
    {
        
        userInput.text = "";
        userInput.ActivateInputField();

        if(input != "")
        {
            char[] delims = { ' ' };
            string[] parts = input.ToLower().Split(delims); //part[0] is command, [1] is directive
            if(parts.Length >= 2)
            {
                
            
                if (commands.Contains(parts[0]))
                {
                    UpdateStory(input);
                    if(parts[0] == "go")
                    {
                        if(NavigationManager.instance.SwitchRooms(parts[1])){
                            
                        }
                        else
                        {
                            UpdateStory("Direction does not exist or is locked, try again");
                        }
                        
                    }else if(parts[0] == "get")
                    {
                        if(NavigationManager.instance.getItem(parts[1]))
                        {
                            GameManager.instance.inventory.Add(parts[1]);
                            UpdateStory("You add a(n) " + parts[1] + " to your inventory.");
                        }
                        else
                        {
                            UpdateStory("Sorry, " + parts[1] + " is not in this room.");
                        }
                    }
                    else if(parts[0] == "restart")
                    {
                        NavigationManager.instance.GameRestart();
                    }
                    
                    
                }
                else //command not valid
                {
                    UpdateStory("Invalid Command, try again");
                }
            }
        }
    }
    public void UpdateStory(string msg)
    {
        story += "\n" + msg;
        storyText.text = story;
        StartCoroutine("ScrollToBottom");
    }
}
