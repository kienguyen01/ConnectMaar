using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileManager : MonoBehaviour
{
    public Tile hexPrefab;
    public House housePrefab;
    public SolarPanel solarPrefab;
    public SpecialBuilding church;
    public SpecialBuilding stadium;

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

    //todo discuss if this approach is doable and how to make sure these tiles having special buildings
    void InitSpecialBuildings()
    {/*
        List<Tile> stadium = new List<Tile>();
        //Add specialbuilding tiles
        stadium.Add((tiles[10])[(18)]);
        stadium.Add((tiles[10])[(17)]);
        stadium.Add((tiles[10])[(16)]);
        stadium.Add((tiles[9])[(17)]);

        specialBuildings.Add(stadium);*/
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
            SpecialBuilding church_cell = (SpecialBuilding)Instantiate(church, new Vector3(xPos + 0.512f, 0.35f, y * zOffset - 0.636f), Quaternion.Euler(-90, 90, 0));
            church_cell.transform.localScale = new Vector3(15.0f, 15.0f, 15.0f);
            church_cell.transform.SetParent(hex_cell.transform);
            church_cell.name = "church_" + x + "_" + y;
        }

        if (x == 9 && y == 17)
        {
            SpecialBuilding stadium_cell = (SpecialBuilding)Instantiate(stadium, new Vector3(xPos + 0.5f, 0.205f, y * zOffset), Quaternion.Euler(-90, 0, 0));
            stadium_cell.transform.localScale = new Vector3(0.15f, 0.15f, 0.3f);
            stadium_cell.transform.SetParent(hex_cell.transform);
            stadium_cell.name = "stadium_" + x + "_" + y;
        }

        addMethods(hex_cell);
        addSpecialBuilding(hex_cell);

        setEmpties(hex_cell);

        return hex_cell;
    }

    private void setEmpties(Tile hex_cell)
    {
        if(hex_cell.Structure == null)
        {
            hex_cell.AddStructure<EmptyStructure>();
        }
    }
    
    private void addSpecialBuilding(Tile hex_cell)
    {
        string tileCoords = hex_cell.X.ToString().PadLeft(3, '0') + "|" + hex_cell.Y.ToString().PadLeft(3, '0');

        switch (tileCoords)
        {
            case "009|017":
                hex_cell.AddStructure<SpecialBuilding>();
                break;
            case "005|014":
                hex_cell.AddStructure<SpecialBuilding>();
                break;
            default:
                break;
        }
    }

    public bool isOccupied(Tile tile) {
        if (tile.occupied)
        {
            return true;
        }
        else if (tile.IsSpecial(this))
        {
            return true;
        }
        return false;
    }

    private void addMethods(Tile tile)
    {
        //tile.onTaken += (PlayerState Instigator) =>
        //{
        //    tile.OwnedBy = Instigator;
        //    List<Tile> temp = getNeigbours(tile);
        //};
        tile.onSelected += (PlayerState Instigator) =>
        {
            if (!Instigator.gameData.tilesChosen.Contains(tile))
            {
                Instigator.gameData.tilesChosen.Add(tile);
                tile.SelectedBy = Instigator;
            }
            else
            {
                Instigator.gameData.tilesChosen.Remove(tile);
                tile.SelectedBy = null;
            }
        };

        tile.transform.SetParent(this.transform);
    }

    private void endButtonOnclick(PlayerState Instigator)
    {
        Instigator.gameData.tilesTaken.AddRange(Instigator.gameData.tilesChosen);
        Instigator.gameData.tilesChosen = new List<Tile>();
        foreach(Tile tile in Instigator.gameData.tilesChosen)
        {
            tile.OwnedBy = Instigator;
            
        }
    }

    private bool isOccupiedBySamePlayer(Tile tile, PlayerState Instigator)
    {
        if (tile.occupied && (tile.OwnedBy == Instigator || tile.SelectedBy == Instigator))
        {
            return true;
        }

        return false;
    }

    bool isValidTileToChoose(Tile t, PlayerState playerState)
    {
        foreach (Tile tile in getNeigbours(t))
        {
            if (isOccupiedBySamePlayer(tile, playerState))
            {
                return true;
            }
        }
        return false;
    }

    
}

//TODO TODAY AND TOMORROW:
//CONNECTORS MANAGER
//TILE MANAGER WITH LOGIC OF CONNECTION
//COLOR CELLS ON EVENTS
