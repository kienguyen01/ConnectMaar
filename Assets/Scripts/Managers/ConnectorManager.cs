using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Connector : Structure
{
    public virtual int MaxLength { get => 1; }

    protected List<Tile> tiles;

    private void Awake()
    {
        tiles = new List<Tile>();
    }

    public int getLength()
    {
        return tiles.Count;
    }

    public Connector AddTile(Tile t)
    {
        tiles.Add(t);
        return this;
    }

    public Connector RemoveTile(Tile t)
    {
        tiles.Remove(t);
        return this;
    }

    public List<Tile> GetTiles()
    {
        return tiles;
    }

    public Tile GetLastTile()
    {
        if (tiles.Count > 0)
            return tiles[^1];
        else
            return null;
    }
}

[System.Serializable]
public class Connection : ScriptableObject
{
    public List<Connector> Connectors;
    int length;
    Player ownedPlayer;

    private void Awake()
    {
        Connectors = new List<Connector>();
    }
}
public class ConnectorManager : MonoBehaviour
{

    List<Connector> tempConnectors = new List<Connector>();
    List<Connector> takenConnectors = new List<Connector>();

    bool isValidLengthThree(Tile firstTile, Tile secondTile, Tile thirdTile)
    {
        int x = firstTile.X;
        int y = firstTile.Y;
        if (y % 2 == 1)
        {
            if (secondTile.Y == y)
            {
                if (secondTile.X == x + 1 && thirdTile.X == x + 2 && thirdTile.Y == y)
                {
                    return true;
                }
                else if (secondTile.X == x - 1 && thirdTile.X == x - 2 && thirdTile.Y == y)
                {
                    return true;
                }
            }
            else if (secondTile.X == x)
            {
                if (secondTile.Y == y - 1 && thirdTile.X == x - 1 && thirdTile.Y == y - 2)
                {
                    return true;
                }
                else if (secondTile.Y == y + 1 && thirdTile.X == x - 1 && thirdTile.Y == y + 2)
                {
                    return true;
                }
            }
            else if (secondTile.X == x + 1)
            {
                if (secondTile.Y == y - 1 && thirdTile.X == x + 1 && thirdTile.Y == y - 2)
                {
                    return true;
                }
                else if (secondTile.Y == y + 1 && thirdTile.X == x + 1 && thirdTile.Y == y + 2)
                {
                    return true;
                }
            }
        }
        else
        {
            if (secondTile.Y == y)
            {
                if (secondTile.X == x + 1 && thirdTile.X == x + 2 && thirdTile.Y == y)
                {
                    return true;
                }
                else if (secondTile.X == x - 1 && thirdTile.X == x - 2 && thirdTile.Y == y)
                {
                    return true;
                }
            }
            else if (secondTile.X == x)
            {
                if (secondTile.Y == y + 1 && thirdTile.X == x + 1 && thirdTile.Y == y + 1)
                {
                    return true;
                }
                else if (secondTile.Y == y - 1 && thirdTile.X == x + 1 && thirdTile.Y == y - 1)
                {
                    return true;
                }
            }
            else if (secondTile.X == x - 1)
            {
                if (secondTile.Y == y + 1 && thirdTile.X == x - 1 && thirdTile.Y == y + 2)
                {
                    return true;
                }
                else if (secondTile.Y == y - 1 && thirdTile.X == x - 1 && thirdTile.Y == y - 2)
                {
                    return true;
                }
            }

        }

        return false;
    }

}
