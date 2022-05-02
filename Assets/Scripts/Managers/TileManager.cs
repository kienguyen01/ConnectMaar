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
    
    public static PopupHandler pH;

    public List<SpecialBuilding> specialBuildingList;

    List<List<Tile>> specialBuildings = new List<List<Tile>>();

    List<Tile> allHouses;
   
    List<Tile> allSolar;

    List<Tile> allStadiums;

    List<Tile> allHeat;

    List<Tile> allChurches;

    List<Tile> scrambleSolar;

    List<Tile> scrambleHeat;



    int WIDTH_MAP = 20;
    int HEIGHT_MAP = 20;

    float xOffset = 1.04f * 1.5f;
    float zOffset = 0.9f * 1.5f;

    public static List<List<Tile>> tiles = new List<List<Tile>>();

    public UnityAction<PlayerState> OnTileChosen;

    private void Awake()
    {
        pH = this.gameObject.AddComponent(typeof(PopupHandler)) as PopupHandler;

        if(SceneManager.GetActiveScene() ==  SceneManager.GetSceneByName("Tutorial"))
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
            for (int x = 0; x < WIDTH_MAP; x++)
            {
                List<Tile> tileRow = new List<Tile>();

                for (int y = 0; y < HEIGHT_MAP; y++)
                {
                    tileRow.Add((GenerateTilesMap(x, y)));
                }

                tiles.Add(tileRow);
            }
        }

        allChurches = new List<Tile>();
        allHouses = new List<Tile>();
        allStadiums = new List<Tile>();
        allHeat = new List<Tile>();
        allSolar = new List<Tile>();


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
            hex_cell.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            hex_cell.name = "Hex_" + x + "_" + y;
            hex_cell.X = x;
            hex_cell.Y = y;
        }
        else
        {
            hex_cell = (Tile)Instantiate(hexPrefab, new Vector3(xPos, 0, y * zOffset), Quaternion.identity);
            hex_cell.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f); 
            hex_cell.name = "Hex_" + x + "_" + y;
            hex_cell.X = x;
            hex_cell.Y = y;
        }

        if ((x == 2 && y == 1) || (x == 6 && y == 2) || (x == 10 && y == 0) || (x == 8 && y == 5) || (x == 8 && y == 8) || (x == 8 && y == 5) 
            || (x == 13 && y == 10) || (x == 6 && y == 10) || (x == 4 && y == 13) || (x == 2 && y == 13) || (x==0 && y==0))
        {
            House house_cell = (House)Instantiate(housePrefab, new Vector3(xPos, 0.2f, y * zOffset), Quaternion.identity);
            house_cell.name = "house_" + x + "_" + y;
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
                SpecialBuilding stadium_cell = (SpecialBuilding)Instantiate(stadium, new Vector3(hex_cell.X * xOffset + 1f, 0.205f, hex_cell.Y * zOffset), Quaternion.Euler(-90, 0, 0));
                stadium_cell.transform.localScale = new Vector3(0.15f, 0.15f, 0.3f);
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
                SpecialBuilding church_cell = (SpecialBuilding)Instantiate(church, new Vector3(hex_cell.X * xOffset + 0.507f, 0.35f, hex_cell.Y * zOffset - 0.55f), Quaternion.Euler(-90, 90, 0));
                church_cell.transform.localScale = new Vector3(15.0f, 15.0f, 15.0f);
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
    Tile randomizeTile(int xMax, int yMax, int xMin, int yMin, Tile tile)
    {
        tile.X = Random.Range(xMax, xMin);
        tile.Y = Random.Range(yMax, yMin);

        string output = tile.X.ToString().PadLeft(3, '0') + "|" + tile.Y.ToString().PadLeft(3, '0');

        return tile;
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
            house_cell.name = "house_" + x + "_" + y;
            hex_cell.AddStructure<House>(house_cell);
        }

        if ((x == 2 && y == 12) || (x == 3 && y == 19) || (x == 5 && y == 19) || (x == 3 && y == 16))
        {
            SolarPanel solar_cell = (SolarPanel)Instantiate(solarPrefab, new Vector3(xPos, 0.2f, y * zOffset), Quaternion.Euler(0, -90, 0));
            solar_cell.transform.SetParent(hex_cell.transform);
            solar_cell.name = "solar_" + x + "_" + y;
            
            hex_cell.AddStructure<SolarPanel>(solar_cell);
        }

        if (x == 5 && y == 14)
        {
            SpecialBuilding church_cell = (SpecialBuilding)Instantiate(church, new Vector3(xPos + 0.507f, 0.35f, y * zOffset - 0.55f), Quaternion.Euler(-90, 90, 0));
            church_cell.transform.localScale = new Vector3(15.0f, 15.0f, 15.0f);
            church_cell.name = "church_" + x + "_" + y;
            hex_cell.AddStructure<SpecialBuilding>(church_cell);
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

        allStadiums.Add(randomizeTile(9, 17, 9, 17, hex_cell));

        allHouses.Add(randomizeTile(9, 16, 5, 12, hex_cell));
        allHouses.Add(randomizeTile(12, 16, 5, 12, hex_cell));
        allHouses.Add(randomizeTile(17, 12, 5, 5, hex_cell));

        scrambleSolar.Add(randomizeTile(12, 12, 2, 1, hex_cell));
        scrambleSolar.Add(randomizeTile(12, 12, 2, 1, hex_cell));
        scrambleSolar.Add(randomizeTile(12, 12, 2, 1, hex_cell));

        foreach(Tile tile in allStadiums)
        {
            SpecialBuilding stadium_cell = (SpecialBuilding)Instantiate(stadium, new Vector3(tile.X * xOffset + 1f, 0.205f, tile.Y * zOffset), Quaternion.Euler(-90, 0, 0));
            stadium_cell.transform.localScale = new Vector3(0.15f, 0.15f, 0.3f);
            stadium_cell.name = "stadium_" + tile.X + "_" + tile.Y;
            stadium_cell.SolarRequired = true;
            tile.openInfoCard += (PlayerState player) =>
            {
                pH.canvas = GameObject.Find("StadiumCard").GetComponent<Canvas>();
                pH.Popup();
                //Debug.Log("!!! OpenInfoCard !!!");
                //todo open infocard
            };
            tile.AddStructure<SpecialBuilding>(stadium_cell);
        }

        foreach(Tile tile in allHouses)
        {
            House house_cell = (House)Instantiate(housePrefab, new Vector3(tile.X * xOffset, 0.2f, tile.Y * zOffset), Quaternion.identity);
            tile.AddStructure<House>(house_cell);
        }

        foreach(Tile tile in scrambleSolar)
        {
            tile.IsScrambleForSolar = true;
            tile.gameObject.AddComponent<Outline>().OutlineWidth = 4.0f;
        }

        //switch (tileCoords)
        //{
        //    case "009|017":
        //        SpecialBuilding stadium_cell = (SpecialBuilding)Instantiate(stadium, new Vector3(hex_cell.X * xOffset + 1f, 0.205f, hex_cell.Y * zOffset), Quaternion.Euler(-90, 0, 0));
        //        stadium_cell.transform.localScale = new Vector3(0.15f, 0.15f, 0.3f);
        //        stadium_cell.name = "stadium_" + hex_cell.X + "_" + hex_cell.Y;
        //        stadium_cell.SolarRequired = true;
        //        hex_cell.openInfoCard += (PlayerState player) =>
        //        {
        //            pH.canvas = GameObject.Find("StadiumCard").GetComponent<Canvas>();
        //            pH.Popup();
        //            //Debug.Log("!!! OpenInfoCard !!!");
        //            //todo open infocard
        //        };
        //        hex_cell.AddStructure<SpecialBuilding>(stadium_cell);
        //        break;
        //    case "005|014":
        //        hex_cell.AddStructure<SpecialBuilding>();
        //        break;
        //    case "012|016":
        //        House house_cell = (House)Instantiate(housePrefab, new Vector3(hex_cell.X * xOffset, 0.2f, hex_cell.Y * zOffset), Quaternion.identity);
        //        hex_cell.AddStructure<House>(house_cell);
        //        break;
        //    case "009|015":
        //        hex_cell.IsScrambleForSolar = true;
        //        break;
        //    case "012|015":
        //        hex_cell.IsScrambleForSolar = true;
        //        break;
        //    case "004|002":
        //        hex_cell.IsScrambleForSolar = true;
        //        hex_cell.gameObject.AddComponent<Outline>().OutlineWidth = 4.0f;
        //        //hex_cell.gameObject.AddComponent<Outline>().OutlineColor = Color.blue;
        //        break;
        //    case "009|002":
        //        hex_cell.IsScrambleForSolar = true;
        //        hex_cell.gameObject.AddComponent<Outline>().OutlineWidth = 4.0f;
        //        //hex_cell.gameObject.AddComponent<Outline>().OutlineColor = Color.blue;
        //        break;
        //    case "010|009":
        //        hex_cell.IsScrambleForSolar = true;
        //        hex_cell.gameObject.AddComponent<Outline>().OutlineWidth = 4.0f;
        //        //hex_cell.gameObject.AddComponent<Outline>().OutlineColor = Color.blue;
        //        break;
        //    default:
        //        break;
        //}
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
        Tile origin = tile.GetSpecialOriginTile();//left = 10,15 | origin = 10,16 | right = 11,16 | top = 10,17

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