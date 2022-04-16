using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Connector : Structure
{
    protected int length;
    protected List<Tile> tiles;
    public int getLength()
    {
        return tiles.Count;
    } 
}

public abstract class Connection : MonoBehaviour
{
    List<Connector> tiles = new List<Connector>();
    int length;
    Player ownedPlayer;
}
public class ConnectorManager : MonoBehaviour
{
    List<Connector> tempConnectors = new List<Connector>();
    List<Connector> takenConnectors = new List<Connector>();
}
