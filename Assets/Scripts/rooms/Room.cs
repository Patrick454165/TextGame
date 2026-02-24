using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Room", menuName = "Text/Room")]
public class Room : ScriptableObject
{
    public string roomName;
    [TextArea]
    public string description;
    public Exit[] exits;
    public List<string> items = new List<string>();
    
}