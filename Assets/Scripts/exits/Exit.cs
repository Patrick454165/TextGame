using UnityEngine;

[CreateAssetMenu(fileName = "Exit", menuName = "Text/Exit")]
public class Exit : ScriptableObject
{
    public enum Direction { north, south, east, west};
    public Direction direction;

    [TextArea]
    public string description;
    public Room room; //room it's attached to
    public bool isLocked;
    public bool isHidden;
}

