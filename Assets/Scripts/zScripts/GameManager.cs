using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<string> inventory = new List<string>();
    public Button saveButton;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    public void Start()
    {
        NavigationManager.instance.onRestart += ResetGame;
        saveButton.onClick.AddListener(Save);
        Load();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/player.save")) //we want to load file
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream aFile = File.Open(Application.persistentDataPath + "/player.save", FileMode.Open);
            SaveState gameState = (SaveState) bf.Deserialize(aFile);
            aFile.Close();
            Room room = NavigationManager.instance.GetRoomByName(gameState.currentRoom);
            List<string> inven = gameState.inventory;
            if(room != null)
            {
                NavigationManager.instance.SwitchRooms(room);
            }
            if (inven != null)
            {
                inventory=inven;
            }
            
        }
        else
        {
            NavigationManager.instance.GameRestart();
        }
        
    }

    public void Save()
    {
        SaveState gameState = new SaveState();
        gameState.currentRoom = NavigationManager.instance.currentRoom.name;
        gameState.inventory = inventory;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream aFile = File.Create(Application.persistentDataPath + "/player.save");
        InputManager.instance.UpdateStory("Save successful");
        //Debug.Log(Application.persistentDataPath);
        bf.Serialize(aFile, gameState);
        aFile.Close();
    }

    void ResetGame()
    {
        Debug.Log("Run");
        inventory.Clear();
        
    }
}
