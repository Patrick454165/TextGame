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
    [TextArea]
    public string commandDescription;
    public Exit toCoinEast;
    public Exit toWinEast;
    
    
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
        toCoinEast.isHidden=true;
        toWinEast.isHidden=true;
        commands.Add("go");
        commands.Add("get");
        commands.Add("restart");
        commands.Add("save");
        commands.Add("inventory");
        commands.Add("commands");
        commands.Add("use");
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
                    }else if (parts[0] == "use")
                    {
                        if(NavigationManager.instance.currentRoom.name == "dragon" && GameManager.instance.inventory.Contains("sword") && parts[1] == "sword" && NavigationManager.instance.currentRoom.useAltDesc==false)
                        {
                            UpdateStory("Before the dragon can strike, you slay the dragon in one fell blow with Sigebert. The dragon's head falls limply to the ground.");
                            toCoinEast.isHidden=false;
                            NavigationManager.instance.UnlockRoom("east", NavigationManager.instance.GetRoomByName("coin"));
                            
                            UpdateStory(" " + toCoinEast.description);
                            NavigationManager.instance.currentRoom.useAltDesc=true;
                        }else if(NavigationManager.instance.currentRoom.name == "exit" && GameManager.instance.inventory.Contains("coin") && parts[1] == "coin" && NavigationManager.instance.currentRoom.useAltDesc==false)
                        {
                            toWinEast.isHidden=false;
                            NavigationManager.instance.UnlockRoom("east", NavigationManager.instance.GetRoomByName("win"));
                            
                            UpdateStory("You toss Davio's coin into the corner and he LUNGES after it, beginning to tear at it ferociously, cackling to himself as he does so. The exit is now clear behind his pile of worthless gold and jewels. " + toWinEast.description);
                            NavigationManager.instance.currentRoom.useAltDesc=true;
                        }
                        else
                        {
                            UpdateStory("This item cannot be used here.");
                        }
                    }
                    
                    
                    
                    
                }
                else //command not valid
                {
                    UpdateStory("Invalid Command, try again");
                }
            }//two
            else if(parts.Length > 0)
            {
                if(parts[0] == "restart")
                {
                    NavigationManager.instance.GameRestart();
                }
                else if(parts[0] == "save")
                {
                    GameManager.instance.Save();
                    
                }
                else if(parts[0] == "commands")
                {
                    UpdateStory(commandDescription);
                }
                else if(parts[0] == "inventory")
                {
                    if (GameManager.instance.inventory.Count != 0)
                    {
                        foreach(string i in GameManager.instance.inventory)
                        {UpdateStory(i);}
                    }
                    else
                    {
                        UpdateStory("You don't currently have anything.");
                    }
                    
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
