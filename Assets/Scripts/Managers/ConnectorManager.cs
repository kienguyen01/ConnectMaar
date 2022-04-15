using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Connector : Structure
{
    protected int length;

    public int getLength()
    {
        return this.length;
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
