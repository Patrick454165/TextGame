using System;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;

public class NavigationManager : MonoBehaviour
{
    public static NavigationManager instance;
    public Room startingRoom;
    public Room currentRoom;
    private Dictionary<string, Room> exitRooms = new Dictionary<string, Room>();
    public Exit toKeyNorth;
    public List<Room> rooms; //has all rooms
    public delegate void Restart();
    public event Restart onRestart;
    

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
        
        currentRoom=startingRoom;
    }

    // Update is called once per frame
    void Unpack()
    {

        string description;
        if(currentRoom.useAltDesc){description=currentRoom.altDescription;}else{description = currentRoom.description;}
        exitRooms.Clear();
        if(currentRoom.name=="start"){currentRoom.useAltDesc=true;}
        if(currentRoom.name == "dragon")
        {
            if(!GameManager.instance.inventory.Contains("sword"))
            {
                InputManager.instance.UpdateStory(description + "There isn't any time to move before their fiery breath torches you, sending you to an early grave. Your skull joins theirs on the string. THE END"+"\n-------------------------------");
                GameRestart();
            }
            else if (NavigationManager.instance.currentRoom.useAltDesc)
            {
                InputManager.instance.UpdateStory(description + ". " + currentRoom.exits[0].description);
            }
            else
            {
                InputManager.instance.UpdateStory(description + ". " + currentRoom.exits[1].description);
            }
        }
            
        else
        {
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

            
        }
        
        
    }

    public void UnlockRoom(string direction, Room room)
    {
        exitRooms.Add(direction, room);
    }
    public void GameRestart()
    {
        onRestart.Invoke(); //calling my restart event to happen;
        currentRoom=startingRoom; //put back at start
        toKeyNorth.isHidden=true;
        InputManager.instance.toCoinEast.isHidden=true;
        InputManager.instance.toWinEast.isHidden=true;
        foreach (Room r in NavigationManager.instance.rooms)
        {
            r.items.Clear();
            r.useAltDesc=false;
            if(!GameManager.instance.inventory.Contains(r.startingItem)){r.items.Add(r.startingItem);}
            
                
                
            
        }
        
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
    public void SwitchRooms(Room room)
    {
        currentRoom = room;
        Unpack();
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
        
        foreach(string i in currentRoom.items)
        {
            if(i == item)
            {
                
                if (item == "orb")
                {
                    toKeyNorth.isHidden=false;
                }
                currentRoom.items.Remove(i);
                currentRoom.useAltDesc=true;
            
                return true; //item found
            }

        }
        
        return false; // item not found
    }

    public Room GetRoomByName(string name)
    {
        foreach(Room aroom in rooms)
        {
            if(name == aroom.name)
            {
                return aroom;
            }
            
        }
        return null;
    }

}
