using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileManager : MonoBehaviour
{
    public Tile hexPrefab;
    public House housePrefab;
    public SolarPanel solarPrefab;
    public SpecialBuildings church;
    public SpecialBuildings stadium;

    List<List<Tile>> specialBuildings = new List<List<Tile>>();


    int WIDTH_MAP = 20;
    int HEIGHT_MAP = 20;

    float xOffset = 1.04f;
    float zOffset = 0.9f;

    //public Tile[,] tiles;

    public List<List<Tile>> tiles = new List<List<Tile>>();

    public UnityAction<PlayerState> OnTileChosen;

    public int tileAvailable
    {
        get
        {
            return tiles.Count;
        }
    }

    public UnityAction<int> OnTileAvailableChanged;
    List<Tile> windturbineTiles;
    void InitPowerSources()
    {
        List<Tile> powerSources = new List<Tile>();

        windturbineTiles = powerSources;

    }

    List<Tile> housesTiles;

    void InitHouses()
    {
        //some random shit goes here
        List<Tile> houses = new List<Tile>();

        housesTiles = houses;
    }

    //todo discuss if this approach is doable and how to make sure these tiles having special buildings
    void InitSpecialBuildings()
    {
        List<Tile> stadium = new List<Tile>();
        //Add specialbuilding tiles
        stadium.Add((tiles[10])[(18)]);
        stadium.Add((tiles[10])[(17)]);
        stadium.Add((tiles[10])[(16)]);
        stadium.Add((tiles[9])[(17)]);

        specialBuildings.Add(stadium);
    }

    public List<Tile> getSpecialBuildingChosen(Tile t)
    { 
        List<Tile> specialBuildingChosen = new List<Tile>();

        for (int i = 0; i < specialBuildings.Count; i++)
        {
            if (specialBuildings[i].Contains(t))
            {
                specialBuildingChosen = specialBuildings[i];
            }
        }

        return specialBuildingChosen;
    }

    private void Start()
    {
        
        for (int x = 0; x < WIDTH_MAP; x++)
        {
            List<Tile> tileRow = new List<Tile>();

            for (int y = 0; y < HEIGHT_MAP; y++)
            {
                tileRow.Add((GenerateTilesMap(x, y)));
            }

            tiles.Add(tileRow);
        }

        InitSpecialBuildings();
    }
    
    /// <summary>
    /// Function to retrive neighbouring cells from a tile
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public List<Tile> getNeigbours(Tile t)//TODO optimize
    {
        List<Tile> neighbours = new List<Tile>();

        if (t.Y % 2 != 0)
        {
            neighbours.Add((tiles[t.X])[(t.Y - 1)]);
            neighbours.Add((tiles[t.X + 1])[(t.Y - 1)]);
            neighbours.Add((tiles[t.X + 1])[(t.Y)]);
            neighbours.Add((tiles[t.X + 1])[(t.Y + 1)]);
            neighbours.Add((tiles[t.X])[(t.Y + 1)]);
            neighbours.Add((tiles[t.X - 1])[(t.Y)]);
        }
        else
        {
            neighbours.Add((tiles[t.X-1])[(t.Y - 1)]);
            neighbours.Add((tiles[t.X])[(t.Y - 1)]);
            neighbours.Add((tiles[t.X + 1])[(t.Y)]);
            neighbours.Add((tiles[t.X])[(t.Y + 1)]);
            neighbours.Add((tiles[t.X - 1])[(t.Y + 1)]);
            neighbours.Add((tiles[t.X - 1])[(t.Y)]);
        }

        return neighbours;
    }

    public List<Tile> getSpecialBuildings(Tile t)
    {
        List<Tile> specialbuildings = new List<Tile> ();

        specialbuildings.Add((tiles[t.X])[(t.Y)]);
        specialbuildings.Add((tiles[t.X-1])[(t.Y-1)]);
        specialbuildings.Add((tiles[t.X])[(t.Y-1)]);
        specialbuildings.Add((tiles[t.X])[(t.Y-2)]);


        return specialbuildings;
    }

    /// <summary>
    /// Get all tiles that are touched, passing List<Tile> because of special buildings will be a list of tiles
    /// </summary>
    /// <param name="tiles"></param>
    /// <returns></returns>
    public List<Tile> getTilesTouched(List<Tile> tiles)
    {
        List<Tile> touchedTiles = new List<Tile>();
        foreach(Tile t in tiles)
        {
            touchedTiles.Add(t);
        }
        return touchedTiles; 
    }




    public List<Tile> getPossibleStepBy3(Tile t)
    {
        List<Tile> possibleTilesStepBy3 = new List<Tile>();

        if(t.X < 2 && t.Y > 2)
        {
            possibleTilesStepBy3.Add((tiles[t.X + 2])[t.Y]);
            possibleTilesStepBy3.Add((tiles[t.X - 1])[t.Y + 2]);
            possibleTilesStepBy3.Add((tiles[t.X + 1])[t.Y + 2]);
            possibleTilesStepBy3.Add((tiles[t.X + 1])[t.Y - 2]);
            possibleTilesStepBy3.Add((tiles[t.X - 1])[t.Y - 2]);
        }
        if(t.X < 1 && t.Y > 2)
        {
            possibleTilesStepBy3.Add((tiles[t.X + 2])[t.Y]);
            possibleTilesStepBy3.Add((tiles[t.X + 1])[t.Y + 2]);
            possibleTilesStepBy3.Add((tiles[t.X + 1])[t.Y - 2]);
        }
        if (t.Y < 2 && t.X > 2)
        {
            possibleTilesStepBy3.Add((tiles[t.X - 2])[t.Y]);
            possibleTilesStepBy3.Add((tiles[t.X + 2])[t.Y]);
            possibleTilesStepBy3.Add((tiles[t.X - 1])[t.Y + 2]);
            possibleTilesStepBy3.Add((tiles[t.X + 1])[t.Y + 2]);
        }
        else
        {
            possibleTilesStepBy3.Add((tiles[t.X - 2])[t.Y]);
            possibleTilesStepBy3.Add((tiles[t.X + 2])[t.Y]);
            possibleTilesStepBy3.Add((tiles[t.X - 1])[t.Y + 2]);
            possibleTilesStepBy3.Add((tiles[t.X + 1])[t.Y + 2]);
            possibleTilesStepBy3.Add((tiles[t.X + 1])[t.Y - 2]);
            possibleTilesStepBy3.Add((tiles[t.X - 1])[t.Y - 2]);
        }
        

        return possibleTilesStepBy3;
    }

    Tile GenerateTilesMap(int x, int y)
    {
        float xPos = x * xOffset;
        // check odd row => go inside
        if (y % 2 != 0)
        {
            xPos += xOffset / 2f;
        }
        Tile hex_cell = (Tile)Instantiate(hexPrefab, new Vector3(xPos, 0, y * zOffset), Quaternion.identity);
        hex_cell.name = "Hex_" + x + "_" + y;
        hex_cell.X = x;
        hex_cell.Y = y;

        addMethods(hex_cell);
        

        if ((x == 3 && y == 12) || (x == 5 && y == 10) || (x == 7 && y == 15) || (x == 2 && y == 19))
        {
            House house_cell = (House)Instantiate(housePrefab, new Vector3(xPos, 0.2f, y * zOffset), Quaternion.identity);
            house_cell.transform.SetParent(hex_cell.transform);
            house_cell.name = "house_" + x + "_" + y;
        }

        if ((x == 2 && y == 12) || (x == 3 && y == 19) || (x == 5 && y == 19) || (x == 3 && y == 16))
        {
            SolarPanel solar_cell = (SolarPanel)Instantiate(solarPrefab, new Vector3(xPos, 0.2f, y * zOffset), Quaternion.Euler(0, -90, 0));
            solar_cell.transform.SetParent(hex_cell.transform);
            solar_cell.name = "solar_" + x + "_" + y;
        }

        if (x == 5 && y == 14)
        {
            SpecialBuildings church_cell = (SpecialBuildings)Instantiate(church, new Vector3(xPos, 0.35f, y * zOffset + 0.45f), Quaternion.Euler(-90, 90, 0));
            church_cell.transform.localScale = new Vector3(15.0f, 15.0f, 15.0f);
            church_cell.transform.SetParent(hex_cell.transform);
            church_cell.name = "church_" + x + "_" + y;
        }

        if (x == 10 && y == 18)
        {
            SpecialBuildings stadium_cell = (SpecialBuildings)Instantiate(stadium, new Vector3(xPos, 0.205f, y * zOffset - 0.923f), Quaternion.Euler(-90, 0, 0));
            stadium_cell.transform.localScale = new Vector3(0.15f, 0.15f, 0.3f);
            stadium_cell.transform.SetParent(hex_cell.transform);
            stadium_cell.name = "stadium_" + x + "_" + y;
        }
        return hex_cell;
    }

    private void addMethods(Tile tile)
    {
        tile.onTaken += (PlayerState Instigator) =>
        {
            tile.OwnedBy = Instigator;
            List<Tile> temp = getNeigbours(tile);
        };
        tile.onSelected += (PlayerState Instigator) =>
        {
            List<Tile> temp = getSpecialBuildingChosen(tile);
                                                                                                                                                 

            foreach(Tile t in temp)
            {
                t.SelectedBy = Instigator;
                //Instigator.gameData.tilesChosen.Add(t);
            }
            tile.SelectedBy = Instigator;
        };

        tile.transform.SetParent(this.transform);
    }



    bool isValidNormalConnection(List<Tile> chosenTiles, PlayerState Owner)
    {
        Tile startTile = chosenTiles[0];
        Tile endTile = chosenTiles[-1];

        foreach (Tile windturbine in windturbineTiles) {
            List<Tile> surroundingTiles = getNeigbours(windturbine);

            if (surroundingTiles.Contains(startTile))
            {
                if (housesTiles.Contains(endTile))
                {
                    return true;
                }

                foreach (List<Tile> specialBuiling in specialBuildings)
                {
                    if (specialBuiling.Contains(endTile))
                    {
                        return true;
                    }
                }
            }
            else if (surroundingTiles.Contains(startTile))
            {
                if (housesTiles.Contains(endTile))
                {
                    return true;
                }
                foreach (List<Tile> specialBuiling in specialBuildings)
                {
                    if (specialBuiling.Contains(endTile))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    bool isValidSpecialConnection(List<Tile> chosenTiles, PlayerState Owner)
    {


        return false;
    }

}

//TODO TODAY AND TOMORROW:
//CONNECTORS MANAGER
//TILE MANAGER WITH LOGIC OF CONNECTION
//COLOR CELLS ON EVENTS
