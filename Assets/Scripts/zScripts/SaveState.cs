using UnityEngine;
using System.Collections.Generic;

[System.Serializable]

public class SaveState
{
    public string currentRoom;
    public List<string> inventory = new List<string>();
}
