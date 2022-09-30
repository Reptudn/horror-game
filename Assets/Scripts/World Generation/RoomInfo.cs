using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType{
    NORMAL,
    EVENT
}

public enum RoomShape{
    SQUARE,
    CIRCLE,
    OTHER
}

public class RoomInfo : MonoBehaviour
{
    public string roomName;
    public GameObject[] snappingPoints;
    
}
