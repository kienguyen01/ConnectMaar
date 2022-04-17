using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Connector : Structure
{
    public virtual int Length { get => 1; }

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
