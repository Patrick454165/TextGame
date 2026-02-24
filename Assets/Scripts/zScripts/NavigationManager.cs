using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NavigationManager : MonoBehaviour
{
    public static NavigationManager instance;
    public Room startingRoom;
    public Room currentRoom;
    private Dictionary<string, Room> exitRooms = new Dictionary<string, Room>();
    public Exit toKeyNorth;
    public delegate void Restart();
    public event Restart onRestart;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

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
        Unpack();
        currentRoom=startingRoom;
    }

    // Update is called once per frame
    void Unpack()
    {
        string description = currentRoom.description;

        exitRooms.Clear();
        foreach (Exit e in currentRoom.exits)
        {
            if (!e.isHidden)
            {
                
            
                description += ". " + e.description;
                exitRooms.Add(e.direction.ToString(), e.room);
            }
        }
        description += "\n-------------------------------";

        InputManager.instance.UpdateStory(description);

        if(currentRoom.name == "dragon")
        {
            GameRestart();
        }
    }

    public void GameRestart()
    {
        onRestart.Invoke(); //calling my restart event to happen;
        currentRoom=startingRoom; //put back at start
        toKeyNorth.isHidden=true;
        
        Unpack();

    }

    public bool SwitchRooms(string direction)
    {
        if (exitRooms.ContainsKey(direction))
        {
            if (GameManager.instance.inventory.Contains("Key") || !getExit(direction).isLocked)
            {
                currentRoom = exitRooms[direction];
                InputManager.instance.UpdateStory("you go " + direction + "\n");
                Unpack();
                return true;
            }
            else
            {
                return false;
            }
            
        }
        else
        {
            return false;
        }
    }


    Exit getExit(string direction)
    {
        foreach(Exit e in currentRoom.exits)
        {
            if(e.direction.ToString() == direction)
            {
                return e;
            }
            
        }
        return null;
    }

    public bool getItem(string item)
    {
        bool isFound=false;
        foreach(string i in currentRoom.items)
        {
            if(i == item)
            {
                isFound = true;
                if (item == "orb")
                {
                    toKeyNorth.isHidden=false;
                }
            }

        }
        if (isFound)
        {
            currentRoom.items.Remove(item);
            currentRoom.description = "This room no longer holds the orb.";
            return true; //item found
        }
        return false; // item not found
    }

}
