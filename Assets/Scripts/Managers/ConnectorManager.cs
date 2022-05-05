using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Connector : Structure
{
    public virtual int MaxLength { get => 1; }

    public override bool IsBuilding { get => false; }

    public override bool IsConnector { get => true; }

    public override bool IsHeat { get => false; }

    public override bool IsSolar { get => false; }

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

    public Connector ResetConnector()
    {
        foreach (Tile t in this.GetTiles())
        {
            t.SelectedBy = null;
        }
        tiles = new List<Tile>();
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

    public static bool IsValidLengthThree(Tile firstTile, Tile secondTile, Tile thirdTile)
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
                if (secondTile.Y == y + 1 && thirdTile.X == x + 1 && thirdTile.Y == y + 2)
                {
                    return true;
                }
                else if (secondTile.Y == y - 1 && thirdTile.X == x + 1 && thirdTile.Y == y - 2)
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

[System.Serializable]
public class Connection : ScriptableObject
{
    public List<Connector> Connectors;
    int length;
    Player ownedPlayer;

    public Connector GetLastConnector()
    {
        if (Connectors.Count > 0)
            return Connectors[^1];
        else
            return null;
    }

    private void Awake()
    {
    }
}

public class ConnectorManager : MonoBehaviour
{
    List<Connector> tempConnectors = new List<Connector>();
    List<Connector> takenConnectors = new List<Connector>();
}
