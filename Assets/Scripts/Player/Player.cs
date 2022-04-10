using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerPropertyTakenData
{
    public List<List<Tile>> tilesTaken;
}

[System.Serializable]
public class PlayerGameData
{
    public int pointGranted;
    public int handSize;
    public int numNode;
    public bool hasSolarInNetwork;
    public bool hasHeatInNetwork;
}
public class Player : MonoBehaviour
{
    [HideInInspector]
    public PlayerState Owner;

    public PlayerPropertyTakenData propertyTakenData;
    public PlayerGameData gameData;

    
}
