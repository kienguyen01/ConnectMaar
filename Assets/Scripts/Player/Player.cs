using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct PlayerInfo
{
    public string name;
    //achievements
    
}
public class Player : MonoBehaviour //Probably to be removed
{
    [HideInInspector]
    public PlayerState Owner;

    public PlayerGameData gameData;
}
