using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class TileManager : MonoBehaviour
{
    public Tile hexPrefab;
    public House housePrefab;
    public SolarPanel solarPrefab;
    public SpecialBuilding church;
    public SpecialBuilding stadium;
    public Node nodePrefab;
    public Tile solarHexPrefab;
    public House windTurbinePrefab;
    
    public static PopupHandler pH;

    public List<SpecialBuilding> specialBuildingList;

    List<List<Tile>> specialBuildings = new List<List<Tile>>();

    public List<string> allHouses;
   
    public List<string> allSolar;

    public List<string> allStadiums;

    public List<string> allHeat;

    public List<string> allChurches;

    public List<string> scrabbleSolar;

    public List<string> scrabbleHeat;

    public List<string> windTurbines;



    int WIDTH_MAP = 20;
    int HEIGHT_MAP = 20;

    float xOffset = 1.04f;
    float zOffset = 0.9f;

    public static List<List<Tile>> tiles = new List<List<Tile>>();

    public UnityAction<PlayerState> OnTileChosen;

    private void Awake()
    {
        pH = this.gameObject.AddComponent(typeof(PopupHandler)) as PopupHandler;
        allChurches = new List<string>();
        allHouses = new List<string>();
        allStadiums = new List<string>();
        allHeat = new List<string>();
        allSolar = new List<string>();
        scrabbleSolar = new List<string>();
        windTurbines = new List<string>();
        windTurbines.Add("000|000");
        //addSpecialTiles();
        if (SceneManager.GetActiveScene() ==  SceneManager.GetSceneByName("Tutorial"))
        {
            for (int x = 0; x < 15; x++)
            {
                List<Tile> tileRow = new List<Tile>();

                for (int y = 0; y < 15; y++)
                {
                    tileRow.Add((GenerateTutorialMap(x, y)));
                }

                tiles.Add(tileRow);
            }
        } 
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("SampleScene"))
        {
            for (int x = 0; x < 30; x++)
            {
                List<Tile> tileRow = new List<Tile>();

                for (int y = 0; y < 30; y++)
                {
                    tileRow.Add((GenerateTilesMap(x, y)));
                }

                tiles.Add(tileRow);
            }
            foreach(List<Tile> tileRow in tiles)
            {
                foreach(Tile tile in tileRow)
                {
                    instantiateSpecialTile(tile, allChurches, allSolar, allStadiums, allHouses, scrabbleSolar, windTurbines, scrabbleHeat, allHeat);
                }
            }
        }
    }

    public int tileAvailable
    {
        get
        {
            return tiles.Count;
        }
    }

    public UnityAction<int> OnTileAvailableChanged;


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
    
    public void CreateMultiplayerMap()
    {
        for (int x = 0; x < 30; x++)
        {
            List<Tile> tileRow = new List<Tile>();

            for (int y = 0; y < 30; y++)
            {
                tileRow.Add((GenerateTilesMap(x, y)));
            }

            tiles.Add(tileRow);
        }
        foreach (List<Tile> tileRow in tiles)
        {
            foreach (Tile tile in tileRow)
            {
                instantiateSpecialTile(tile, allChurches, allSolar, allStadiums, allHouses, scrabbleSolar, windTurbines, scrabbleHeat, allHeat);
            }
        }
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
            if (t.Y != 0)
            {
                neighbours.Add((tiles[t.X])[(t.Y - 1)]);
                if(t.X != tiles.Count - 1)
                    neighbours.Add((tiles[t.X + 1])[(t.Y - 1)]);
            }
            if(t.X != tiles.Count - 1)
            {
                neighbours.Add((tiles[t.X + 1])[(t.Y)]);
                if(t.Y != tiles[0].Count - 1)
                    neighbours.Add((tiles[t.X + 1])[(t.Y + 1)]);
            }
            if(t.Y != tiles[0].Count - 1)
                neighbours.Add((tiles[t.X])[(t.Y + 1)]);
            if (t.X != 0)
                neighbours.Add((tiles[t.X - 1])[(t.Y)]);
        }
        else
        {
            if(t.X != 0)
            {
                neighbours.Add((tiles[t.X - 1])[(t.Y)]);
                if(t.Y != tiles[0].Count - 1)
                    neighbours.Add((tiles[t.X - 1])[(t.Y + 1)]);
                if (t.Y != 0)
                    neighbours.Add((tiles[t.X - 1])[(t.Y - 1)]);
            }
            if (t.Y != tiles[0].Count - 1)
                neighbours.Add((tiles[t.X])[(t.Y + 1)]);
            if (t.X != tiles.Count - 1)
                neighbours.Add((tiles[t.X + 1])[(t.Y)]);
            if (t.Y != 0)
                neighbours.Add((tiles[t.X])[(t.Y - 1)]);
        }
        return neighbours;
    }


    Tile GenerateTutorialMap(int x, int y)
    {
        Tile hex_cell;
        float xPos = x * xOffset;
        // check odd row => go inside
        if (y % 2 != 0)
        {
            xPos += xOffset / 2f;
        }
        if (x == 4 && y == 2)
        {
            hex_cell = (Tile)Instantiate(solarHexPrefab, new Vector3(xPos, 0, y * zOffset), Quaternion.identity);
            hex_cell.name = "Hex_" + x + "_" + y;
            hex_cell.X = x;
            hex_cell.Y = y;
        }
        else
        {
            hex_cell = (Tile)Instantiate(hexPrefab, new Vector3(xPos, 0, y * zOffset), Quaternion.identity);
            hex_cell.name = "Hex_" + x + "_" + y;
            hex_cell.X = x;
            hex_cell.Y = y;
        }

        if ((x == 2 && y == 1) || (x == 6 && y == 2) || (x == 10 && y == 0) || (x == 8 && y == 5) || (x == 8 && y == 8) || (x == 8 && y == 5) 
            || (x == 13 && y == 10) || (x == 6 && y == 10) || (x == 4 && y == 13) || (x == 2 && y == 13))
        {
            House house_cell = (House)Instantiate(housePrefab, new Vector3(xPos, 0.2f, y * zOffset), Quaternion.identity);
            house_cell.transform.localScale = new Vector3(0.08f, 0.16f, 0.16f);
            house_cell.name = "house_" + x + "_" + y;
            house_cell.transform.localScale = new Vector3(0.08f, 0.16f, 0.16f);
            hex_cell.AddStructure<House>(house_cell);
        }

        if(x == 0 && y == 0)
        {
            House house_cell = (House)Instantiate(windTurbinePrefab, new Vector3(hex_cell.X * xOffset, 1.25f, hex_cell.Y * zOffset), Quaternion.identity);
            house_cell.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            hex_cell.AddStructure<House>(house_cell);
        }

        addMethods(hex_cell);
        addSpecialBuildingsTutorial(hex_cell);

        setEmpties(hex_cell);

        return hex_cell;
    }

    private void addSpecialBuildingsTutorial(Tile hex_cell)
    {
        string tileCoords = hex_cell.X.ToString().PadLeft(3, '0') + "|" + hex_cell.Y.ToString().PadLeft(3, '0');

        switch (tileCoords)
        {
            case "007|000":
                SolarPanel solar_cell = (SolarPanel)Instantiate(solarPrefab, new Vector3(hex_cell.X * xOffset, 0.2f, hex_cell.Y * zOffset), Quaternion.Euler(0, -90, 0));
                solar_cell.transform.SetParent(hex_cell.transform);
                solar_cell.name = "solar_" + hex_cell.X + "_" + hex_cell.Y;
                hex_cell.AddStructure<House>(solar_cell);
                break;
            case "000|007":
                SpecialBuilding stadium_cell = (SpecialBuilding)Instantiate(stadium, new Vector3(hex_cell.X * xOffset + 1f, 0.205f, hex_cell.Y * zOffset), Quaternion.Euler(0, 0, 0));
                stadium_cell.transform.localScale = new Vector3(0.18f, 0.18f, 0.18f);
                stadium_cell.name = "stadium_" + hex_cell.X + "_" + hex_cell.Y;
                stadium_cell.SolarRequired = true;
                hex_cell.openInfoCard += (PlayerState player) =>
                {
                    pH.canvas = GameObject.Find("StadiumCard").GetComponent<Canvas>();
                    pH.Popup();
                    //Debug.Log("!!! OpenInfoCard !!!");
                    //todo open infocard
                };
                hex_cell.AddStructure<SpecialBuilding>(stadium_cell);
                break;
            case "011|006":
                SpecialBuilding church_cell = (SpecialBuilding)Instantiate(church, new Vector3(hex_cell.X * xOffset + 0.507f, 0.2f, hex_cell.Y * zOffset - 0.55f), Quaternion.Euler(0, 0, 0));
                church_cell.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                church_cell.name = "church_" + hex_cell.X + "_" + hex_cell.Y;
                church_cell.SolarRequired = true;
                hex_cell.openInfoCard += (PlayerState player) =>
                {
                    pH.canvas = GameObject.Find("ChurchCard").GetComponent<Canvas>();
                    pH.Popup();
                    //Debug.Log("!!! OpenInfoCard !!!");
                    //todo open infocard
                };
                hex_cell.AddStructure<SpecialBuilding>(church_cell);
                break;
            case "004|002":
                hex_cell.IsScrambleForSolar = true;
                break;
            case "009|002":
                hex_cell.IsScrambleForSolar = true;
                break;
            case "010|009":
                hex_cell.IsScrambleForSolar = true;
                break;
            default:
                break;
        }
    }

    public string randomizeTile(int xMax, int yMax, int xMin, int yMin)
    {
        int X = Random.Range(xMax, xMin);
        int Y = Random.Range(yMax, yMin);

        string output = X.ToString().PadLeft(3, '0') + "|" + Y.ToString().PadLeft(3, '0');

        return output;
    }

    public Tile GenerateTilesMap(int x, int y)
    {
        Tile hex_cell;
        float xPos = x * xOffset;
        // check odd row => go inside
        if (y % 2 != 0)
        {
            xPos += xOffset / 2f;
        }
        string tileCoords = x.ToString().PadLeft(3, '0') + "|" + y.ToString().PadLeft(3, '0');

        if (scrabbleSolar.Contains(tileCoords))
        {
            hex_cell = (Tile)Instantiate(solarHexPrefab, new Vector3(xPos, 0, y * zOffset), Quaternion.identity);
            hex_cell.name = "Hex_" + x + "_" + y;
            hex_cell.X = x;
            hex_cell.Y = y;
            addMethods(hex_cell);
            setEmpties(hex_cell);
            return hex_cell;
        }
        hex_cell = (Tile)Instantiate(hexPrefab, new Vector3(xPos, 0, y * zOffset), Quaternion.identity);
        hex_cell.name = "Hex_" + x + "_" + y;
        hex_cell.X = x;
        hex_cell.Y = y;
        addMethods(hex_cell);
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

/*    public void addSpecialTiles()
    {
        allStadiums.Add(randomizeTile(9, 17, 9, 17));

        allHouses.Add(randomizeTile(9, 16, 5, 12));
        allHouses.Add(randomizeTile(12, 16, 5, 12));
        allHouses.Add(randomizeTile(17, 12, 5, 5));
        allHouses.Add(randomizeTile(19, 19, 2, 1));
        allHouses.Add(randomizeTile(19, 19, 2, 1));
        allHouses.Add(randomizeTile(19, 19, 2, 1));
        allHouses.Add(randomizeTile(12, 16, 12, 16));
        allHouses.Add(randomizeTile(10, 16, 10, 16));
        allHouses.Add(randomizeTile(15, 15, 15, 15));


        scrambleSolar.Add(randomizeTile(19, 19, 2, 1));
        scrambleSolar.Add(randomizeTile(19, 19, 2, 1));
        scrambleSolar.Add(randomizeTile(19, 19, 2, 1));

        allChurches.Add(randomizeTile(5, 14, 5, 14));

        allSolar.Add(randomizeTile(19, 19, 2, 1));
        allSolar.Add(randomizeTile(19, 19, 2, 1));
        allSolar.Add(randomizeTile(19, 19, 2, 1));
        allSolar.Add(randomizeTile(19, 19, 2, 1));

    }*/

    public void instantiateSpecialTile(Tile hex_cell, List<string> churches, List<string> solars, List<string> stadiums, List<string> houses, List<string> scrambleSolars, List<string> windTurbines, List<string> scrambleHeats, List<string> heats)
    {
        string tileCoords = hex_cell.X.ToString().PadLeft(3, '0') + "|" + hex_cell.Y.ToString().PadLeft(3, '0');

        foreach(string tile in churches)
        {
            if(tileCoords == tile)
            {
                SpecialBuilding church_cell = (SpecialBuilding)Instantiate(church, new Vector3(hex_cell.X + 0.507f, 0.2f, hex_cell.Y * zOffset - 0.55f), Quaternion.Euler(0, 0, 0));
                church_cell.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                church_cell.name = "church_" + hex_cell.X + "_" + hex_cell.Y;
                hex_cell.AddStructure<SpecialBuilding>(church_cell);
            }
        }

        foreach(string tile in solars)
        {
            if(tileCoords == tile)
            {
                if (hex_cell.Y % 2 == 0)
                {
                    SolarPanel solar_cell = (SolarPanel)Instantiate(solarPrefab, new Vector3(hex_cell.X + 0.56f, 0.2f, hex_cell.Y * zOffset), Quaternion.Euler(0, -90, 0));
                    solar_cell.transform.SetParent(hex_cell.transform);
                    solar_cell.name = "solar_" + hex_cell.X + "_" + hex_cell.Y;
                    hex_cell.AddStructure<SolarPanel>(solar_cell);

                }
                else if (hex_cell.Y % 2 != 0)
                {
                    SolarPanel solar_cell = (SolarPanel)Instantiate(solarPrefab, new Vector3(hex_cell.X + 0.62f, 0.2f, hex_cell.Y * zOffset - 0.065f), Quaternion.Euler(0, -90, 0));
                    solar_cell.transform.SetParent(hex_cell.transform);
                    solar_cell.name = "solar_" + hex_cell.X + "_" + hex_cell.Y;
                    hex_cell.AddStructure<SolarPanel>(solar_cell);
                }
            }
        }

        foreach (string tile in stadiums)
        {
            if(tileCoords == tile)
            {
                SpecialBuilding stadium_cell = (SpecialBuilding)Instantiate(stadium, new Vector3(hex_cell.X * xOffset + 1f, 0.205f, hex_cell.Y * zOffset), Quaternion.Euler(0, 0, 0));
                stadium_cell.transform.localScale = new Vector3(0.18f, 0.18f, 0.18f);
                stadium_cell.name = "stadium_" + hex_cell.X + "_" + hex_cell.Y;
                stadium_cell.SolarRequired = true;
                hex_cell.openInfoCard += (PlayerState player) =>
                {
                    pH.canvas = GameObject.Find("StadiumCard").GetComponent<Canvas>();
                    pH.Popup();
                    //Debug.Log("!!! OpenInfoCard !!!");
                    //todo open infocard
                };
                hex_cell.AddStructure<SpecialBuilding>(stadium_cell);
            }
        }

        foreach (string tile in houses)
        {
            if(tileCoords == tile)
            {
                if (hex_cell.Y % 2 == 0)
                {
                    House house_cell = (House)Instantiate(housePrefab, new Vector3(hex_cell.X * xOffset, 0.2f, hex_cell.Y * zOffset), Quaternion.identity);
                    house_cell.transform.localScale = new Vector3(0.08f, 0.16f, 0.16f);

                    hex_cell.AddStructure<House>(house_cell);
                }
                else
                {
                    House house_cell = (House)Instantiate(housePrefab, new Vector3(hex_cell.X * xOffset + 0.52f, 0.2f, hex_cell.Y * zOffset + 0.012f), Quaternion.identity);
                    house_cell.transform.localScale = new Vector3(0.08f, 0.16f, 0.16f);
                    hex_cell.AddStructure<House>(house_cell);
                }
            }
        }

        foreach (string tile in scrambleSolars)
        {
            if(tileCoords == tile)
            {
                hex_cell.IsScrambleForSolar = true;
            }
        }

        foreach(string tile in windTurbines)
        {
            if (tileCoords == tile)
            {
                if (hex_cell.Y % 2 == 0)
                {
                    House house_cell = (House)Instantiate(windTurbinePrefab, new Vector3(hex_cell.X * xOffset, 1.25f, hex_cell.Y * zOffset), Quaternion.identity);
                    house_cell.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

                    hex_cell.AddStructure<House>(house_cell);
                }
                else
                {
                    House house_cell = (House)Instantiate(windTurbinePrefab, new Vector3(hex_cell.X * xOffset, 1.25f, hex_cell.Y * zOffset), Quaternion.identity);
                    house_cell.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                    hex_cell.AddStructure<House>(house_cell);
                }
            }
        }
    }

    public bool isOccupied(Tile tile) {
        if (tile.occupied)
        {
            return true;
        }
        else if (tile.IsSpecial())
        {
            return true;
        }
        return false;
    }

    public bool IsSolarPanel(Tile tile)
    {
        if (tile.Structure.IsSolar)
        {
            return true;
        }
        return false;
    }

    public bool IsHeatPipe(Tile tile)
    {
        if (tile.Structure.IsHeat)
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
            List<Tile> neighbours = getNeigbours(tile);



            if (tile.IsSpecial() && ((tile.GetSpecialOriginTile().Structure.SolarRequired && Instigator.gameData.hasSolarInNetwork) || (tile.GetSpecialOriginTile().Structure.HeatRequired && Instigator.gameData.hasHeatInNetwork)))
            {
                foreach (Tile t in getSpecialNeighbours(tile))
                {
                    Instigator.gameData.tilesChosen.Push(t);
                    t.SelectedBy = Instigator;
                }
            }

            if (isValidTileToChoose(tile, Instigator) && !tile.IsSpecial())
            {
                if (tile.OwnedBy == null && tile.SelectedBy == null && ((Instigator.gameData.tilesChosen.Count > 0) ? (neighbours.Contains(Instigator.gameData.tilesChosen.Peek()) || neighbours.Exists(x=>x.OwnedBy == Instigator)) : true))
                {
                    Instigator.gameData.tilesChosen.Push(tile);
                    tile.SelectedBy = Instigator;
                }
                else if (tile.SelectedBy == Instigator && ((Instigator.gameData.tilesChosen.Count > 0) ? (Instigator.gameData.tilesChosen.Peek() == tile) : false))
                {
                    Instigator.gameData.tilesChosen.Pop();
                    tile.SelectedBy = null;
                }
            }
            else
            {
                /*Debug.LogWarning(Instigator.gameData.tilesChosen.Peek());
                Debug.LogWarning(tile);
                foreach (Tile t in getNeigbours(tile))
                {
                    Debug.Log(t.X + "   " + t.Y);
                }*/
            }
        };

        tile.transform.SetParent(this.transform);
    }

    public List<Tile> getSpecialNeighbours(Tile tile)
    {
        List<Tile> neighbours = new List<Tile>();
        Tile origin = tile.GetSpecialOriginTile();

        if (origin.Y % 2 == 0)
        {
            neighbours.Add(tiles[origin.X][origin.Y - 1]);
            neighbours.Add(tiles[origin.X + 1][origin.Y]);
            neighbours.Add(tiles[origin.X][origin.Y + 1]);
        }
        else
        {
            neighbours.Add(tiles[origin.X + 1][origin.Y -1]);
            neighbours.Add(tiles[origin.X + 1][origin.Y]);
            neighbours.Add(tiles[origin.X + 1][origin.Y + 1]);
        }
        neighbours.Add(origin);
        return neighbours;
    }

    private bool isOccupiedBySamePlayer(Tile tile, PlayerState Instigator)
    {
        if (tile.OwnedBy == Instigator || tile.SelectedBy == Instigator)
        {
            return true;
        }

        return false;
    }

    bool isValidTileToChoose(Tile t, PlayerState playerState)
    {
        foreach (Tile tile in getNeigbours(t))
        {
            if (tile.Structure.IsNode || isOccupiedBySamePlayer(tile, playerState) && (tile.HasBuilding() || tile.SelectedBy != null))
            {
                return true;
            }
        }
        return false;
    }
}