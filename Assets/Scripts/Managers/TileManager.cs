using mixpanel;
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
    public House houseConnectedPrefab;
    public SolarPanel solarPrefab;
    public HMaria ChurchPrefab;
    public DeMeent deMeentStadiumPrefab;
    public Node nodePrefab;
    public Tile solarHexPrefab;
    public House windTurbinePrefab;
    public HeatPump heatPumpPrefab;
    public Tile heatpumpHexPrefab;
    public DaltonCollege daltonPrefab;
    public AFAS AFASPrefab;
    public Bloemwijk bloemwijkPrefab;
    public Investa investaPrefab;

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

    float[,] Noise;
    int deltaX;
    int deltaY;

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

        Noise = NoiseGenerator.Calc2D(1000, 1000, 0.10f);
        deltaX = Random.Range(0, 950);
        deltaY = Random.Range(0, 950);

        Debug.Log(Noise);
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
            for (int x = 0; x < 35; x++)
            {
                List<Tile> tileRow = new List<Tile>();

                for (int y = 0; y < 35; y++)
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
    public List<Tile> getNeigbours(Tile t)
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
            hex_cell = (Tile)Instantiate(hexPrefab, new Vector3(xPos, 0, y * zOffset), Quaternion.identity);
            hex_cell.GetComponentInChildren<MeshRenderer>().material.color = new Color(0.1f, 0.56f, 0.89f, 1);
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

            float nVal = Noise[x + deltaX, y + deltaY];

            if (nVal < 144)
            {
                hex_cell.GetComponentInChildren<MeshRenderer>().material.color = new Color(0.55f, 0.74f, 0.22f, 1);
            }
            else if(nVal < 225)
            {
                hex_cell.GetComponentInChildren<MeshRenderer>().material.color = new Color(0.42f, 0.63f, 0.3f, 1);
            }
            else
            {
                hex_cell.GetComponentInChildren<MeshRenderer>().material.color = new Color(0.40f, 0.54f, 0.34f, 1);
            }
        }

        if ((x == 2 && y == 1) || (x == 6 && y == 2) || (x == 10 && y == 0) || (x == 8 && y == 5) || (x == 8 && y == 8) || (x == 8 && y == 5) 
            || (x == 13 && y == 10) || (x == 6 && y == 10) || (x == 4 && y == 13) || (x == 2 && y == 13))
        {
            House house_cell = (House)Instantiate(housePrefab, new Vector3(xPos, 0.2f, y * zOffset), Quaternion.Euler(0, 90, 0));
            house_cell.transform.localScale = new Vector3(0.08f, 0.16f, 0.16f);
            house_cell.name = "house_" + x + "_" + y;
            house_cell.transform.localScale = new Vector3(0.08f, 0.16f, 0.16f);
            hex_cell.AddStructure<House>(house_cell);
        }

        if(x == 0 && y == 0)
        {
            House house_cell = (House)Instantiate(windTurbinePrefab, new Vector3(hex_cell.X * xOffset, 1.25f, hex_cell.Y * zOffset), Quaternion.identity);
            house_cell.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            house_cell.IsWindmill = true;
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
        Vector3 v3 = hex_cell.transform.position;
        GameObject icon;
        switch (tileCoords)
        {
            case "007|000":
                SolarPanel solar_cell = (SolarPanel)Instantiate(solarPrefab, new Vector3(hex_cell.X * xOffset, 0.2f, hex_cell.Y * zOffset), Quaternion.Euler(0, -90, 0));
                solar_cell.transform.SetParent(hex_cell.transform);
                solar_cell.name = "solar_" + hex_cell.X + "_" + hex_cell.Y;
                hex_cell.AddStructure<House>(solar_cell);
                break;
            case "000|007":
                DeMeent stadium_cell = (DeMeent)Instantiate(deMeentStadiumPrefab, new Vector3(hex_cell.X * xOffset + 1f, 0.205f, hex_cell.Y * zOffset - 1.6f), Quaternion.Euler(0, 0, 0));
                stadium_cell.transform.localScale = new Vector3(0.18f, 0.18f, 0.18f);
                stadium_cell.name = "stadium_" + hex_cell.X + "_" + hex_cell.Y;

                hex_cell.openInfoCard += (PlayerState player) =>
                {
                    pH.canvas = GameObject.Find("StadiumCard").GetComponent<Canvas>();
                    pH.Popup();
                    //Debug.Log("!!! OpenInfoCard !!!");
                    //todo open infocard
                };
                hex_cell.AddStructure<DeMeent>(stadium_cell);
                break;
            case "011|006":
                HMaria church_cell = (HMaria)Instantiate(ChurchPrefab, new Vector3(hex_cell.X * xOffset + 0.507f, 0.2f, hex_cell.Y * zOffset - 0.55f), Quaternion.Euler(0, 0, 0));
                church_cell.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                church_cell.name = "church_" + hex_cell.X + "_" + hex_cell.Y;

                hex_cell.openInfoCard += (PlayerState player) =>
                {
                    pH.canvas = GameObject.Find("ChurchCard").GetComponent<Canvas>();
                    pH.Popup();
                    //Debug.Log("!!! OpenInfoCard !!!");
                    //todo open infocard
                };
                hex_cell.AddStructure<HMaria>(church_cell);
                break;
            case "004|002":
                v3.y += 0.201f;
                icon = Instantiate(GameState.instance.models.SolarCableIcon, hex_cell.transform);
                icon.transform.position = v3;
                icon.transform.parent = hex_cell.transform;
                hex_cell.IsScrabbleForSolar = true;
                break;
            case "009|002":
                v3.y += 0.201f;
                icon = Instantiate(GameState.instance.models.SolarCableIcon, hex_cell.transform);
                icon.transform.position = v3;
                icon.transform.parent = hex_cell.transform;
                hex_cell.IsScrabbleForSolar = true;
                break;
            case "010|009":
                v3.y += 0.201f;
                icon = Instantiate(GameState.instance.models.SolarCableIcon, hex_cell.transform);
                icon.transform.position = v3;
                icon.transform.parent = hex_cell.transform;
                hex_cell.IsScrabbleForSolar = true;
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
        //Debug.Log(Noise[x, y]);
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
            hex_cell = (Tile)Instantiate(hexPrefab, new Vector3(xPos, 0, y * zOffset), Quaternion.identity);
            hex_cell.GetComponentInChildren<MeshRenderer>().material.color = new Color(0.1f, 0.56f, 0.89f, 1);
            hex_cell.name = "Hex_" + x + "_" + y;
            hex_cell.X = x;
            hex_cell.Y = y;
            addMethods(hex_cell);
            setEmpties(hex_cell);
            return hex_cell;
        }
        else if (scrabbleHeat.Contains(tileCoords))
        {
            hex_cell = (Tile)Instantiate(hexPrefab, new Vector3(xPos, 0, y * zOffset), Quaternion.identity);
            hex_cell.GetComponentInChildren<MeshRenderer>().material.color = new Color(0.70f, 0.84f, 0.83f, 1);
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

        float nVal = Noise[x + deltaX, y + deltaY];

        if (nVal < 120)
        {
            hex_cell.GetComponentInChildren<MeshRenderer>().material.color = new Color(0.55f, 0.74f, 0.22f, 1);
        }
        else if (nVal < 200)
        {
            hex_cell.GetComponentInChildren<MeshRenderer>().material.color = new Color(0.42f, 0.63f, 0.3f, 1);
        }
        else
        {
            hex_cell.GetComponentInChildren<MeshRenderer>().material.color = new Color(0.40f, 0.54f, 0.34f, 1);
        }

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

    private void Update()
    {
        
    }

    public void replacePrefab(Tile hex_cell, List<string> houses)
    {
        string tileCoords = hex_cell.X.ToString().PadLeft(3, '0') + "|" + hex_cell.Y.ToString().PadLeft(3, '0');
        foreach(string tile in houses)
        {
            if(tileCoords == tile)
            {
                if(hex_cell.OwnedBy != null)
                {
                    hex_cell.GetComponentInChildren <House>().enabled = false;
                    if (hex_cell.Y % 2 == 0)
                    {
                        House house_cell = (House)Instantiate(houseConnectedPrefab, new Vector3(hex_cell.X * xOffset, 0.2f, hex_cell.Y * zOffset), Quaternion.Euler(0, 90, 0));
                        house_cell.transform.localScale = new Vector3(0.08f, 0.16f, 0.16f);

                        hex_cell.AddStructure<House>(house_cell);
                    }
                    else
                    {
                        House house_cell = (House)Instantiate(houseConnectedPrefab, new Vector3(hex_cell.X * xOffset + 0.52f, 0.2f, hex_cell.Y * zOffset + 0.012f), Quaternion.Euler(0, 90, 0));
                        house_cell.transform.localScale = new Vector3(0.08f, 0.16f, 0.16f);
                        hex_cell.AddStructure<House>(house_cell);
                    }
                }
            }
        }
    }

    public void instantiateSpecialTile(Tile hex_cell, List<string> churches, List<string> solars, List<string> stadiums, List<string> houses, List<string> scrambleSolars, List<string> windTurbines, List<string> scrambleHeats, List<string> heats)
    {
        string tileCoords = hex_cell.X.ToString().PadLeft(3, '0') + "|" + hex_cell.Y.ToString().PadLeft(3, '0');

        foreach(string tile in churches)
        {
            if(tileCoords == tile)
            {
                HMaria church_cell = (HMaria)Instantiate(ChurchPrefab, new Vector3(hex_cell.X + 0.737f, 0.2f, hex_cell.Y * zOffset), Quaternion.Euler(0, 0, 0));
                church_cell.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                church_cell.name = "church_" + hex_cell.X + "_" + hex_cell.Y;
                hex_cell.openInfoCard += (PlayerState player) =>
                {
                    pH.canvas = GameObject.Find("ChurchCard").GetComponent<Canvas>();
                    pH.Popup();
                };
                hex_cell.AddStructure<HMaria>(church_cell);
            }
        }
        
        if(tileCoords == "031|005")
        {
            DeMeent stadium_cell = (DeMeent)Instantiate(deMeentStadiumPrefab, new Vector3(hex_cell.X * xOffset + 1f, 0.205f, hex_cell.Y * zOffset - 1.67f), Quaternion.Euler(0, 0, 0));
            stadium_cell.transform.localScale = new Vector3(0.18f, 0.18f, 0.18f);
            stadium_cell.name = "demeent_" + hex_cell.X + "_" + hex_cell.Y;
            hex_cell.openInfoCard += (PlayerState player) =>
            {
                pH.canvas = GameObject.Find("DeMeentCard").GetComponent<Canvas>();
                pH.Popup();
                //Debug.Log("!!! OpenInfoCard !!!");
                //todo open infocard
            };
            hex_cell.AddStructure<DeMeent>(stadium_cell);
        }

        if(tileCoords == "030|023")
        {
            AFAS stadium_cell = (AFAS)Instantiate(AFASPrefab, new Vector3(hex_cell.X * xOffset + 1f, 0.205f, hex_cell.Y * zOffset - 0.2f), Quaternion.Euler(0, 0, 0));
            stadium_cell.transform.localScale = new Vector3(0.13f, 0.13f, 0.13f);
            stadium_cell.name = "stadium_" + hex_cell.X + "_" + hex_cell.Y;
            hex_cell.openInfoCard += (PlayerState player) =>
            {
                pH.canvas = GameObject.Find("StadiumCard").GetComponent<Canvas>();
                pH.Popup();
                //Debug.Log("!!! OpenInfoCard !!!");
                //todo open infocard
            };
            hex_cell.AddStructure<AFAS>(stadium_cell);
        }
        
        if(tileCoords == "020|008")
        {
            Bloemwijk bloemwijk_cell = (Bloemwijk)Instantiate(bloemwijkPrefab, new Vector3(hex_cell.X * xOffset + 0.36f, 0.22f, hex_cell.Y * zOffset), Quaternion.Euler(0, 90, 0));
            bloemwijk_cell.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            bloemwijk_cell.name = "bloemwijk_" + hex_cell.X + "_" + hex_cell.Y;
            hex_cell.openInfoCard += (PlayerState player) =>
            {
                pH.canvas = GameObject.Find("BloemwijkCard").GetComponent<Canvas>();
                pH.Popup();
                //Debug.Log("!!! OpenInfoCard !!!");
                //todo open infocard
            };
            hex_cell.AddStructure<Bloemwijk>(bloemwijk_cell);
        }

        if(tileCoords == "008|029")
        {
            Investa investa_cell = (Investa)Instantiate(investaPrefab, new Vector3(hex_cell.X * xOffset + 0.84f, 0.205f, hex_cell.Y * zOffset), Quaternion.Euler(0, 0, 0));
            investa_cell.transform.localScale = new Vector3(0.13f, 0.13f, 0.13f);
            investa_cell.name = "investa_" + hex_cell.X + "_" + hex_cell.Y;
            hex_cell.openInfoCard += (PlayerState player) =>
            {
                pH.canvas = GameObject.Find("Investa").GetComponent<Canvas>();
                pH.Popup();
                //Debug.Log("!!! OpenInfoCard !!!");
                //todo open infocard
            };
            hex_cell.AddStructure<Investa>(investa_cell);
        }

        if(tileCoords == "019|021")
        {
            DaltonCollege dalton_cell = (DaltonCollege)Instantiate(daltonPrefab, new Vector3(hex_cell.X * xOffset + 1.06f, 0.222f, hex_cell.Y * zOffset), Quaternion.Euler(0, 0, 0));
            dalton_cell.transform.localScale = new Vector3(0.002f, 0.003f, 0.003f);
            dalton_cell.name = "dalton_" + hex_cell.X + "_" + hex_cell.Y;
            hex_cell.openInfoCard += (PlayerState player) =>
            {
                pH.canvas = GameObject.Find("DaltonCollegeCard").GetComponent<Canvas>();
                pH.Popup();
                //Debug.Log("!!! OpenInfoCard !!!");
                //todo open infocard
            };
            hex_cell.AddStructure<DaltonCollege>(dalton_cell);
        }

        foreach (string tile in houses)
        {
            if(tileCoords == tile)
            {
                if (hex_cell.Y % 2 == 0)
                {
                    House house_cell = (House)Instantiate(housePrefab, new Vector3(hex_cell.X * xOffset, 0.2f, hex_cell.Y * zOffset), Quaternion.Euler(0, 90, 0));
                    house_cell.transform.localScale = new Vector3(0.08f, 0.16f, 0.16f);

                    hex_cell.AddStructure<House>(house_cell);
                }
                else
                {
                    House house_cell = (House)Instantiate(housePrefab, new Vector3(hex_cell.X * xOffset + 0.52f, 0.2f, hex_cell.Y * zOffset + 0.012f), Quaternion.Euler(0, 90, 0));
                    house_cell.transform.localScale = new Vector3(0.08f, 0.16f, 0.16f);
                    hex_cell.AddStructure<House>(house_cell);
                }
            }
        }

        foreach (string tile in scrambleSolars)
        {
            if(tileCoords == tile)
            {
                Vector3 v3 = hex_cell.transform.position;
                v3.y += 0.201f;
                GameObject icon = Instantiate(GameState.instance.models.SolarCableIcon, hex_cell.transform);
                icon.transform.position = v3;
                icon.transform.parent = hex_cell.transform;
                hex_cell.IsScrabbleForSolar = true;
            }
        }
        foreach (string tile in scrambleHeats)
        {
            if (tileCoords == tile)
            {
                Vector3 v3 = hex_cell.transform.position;
                v3.y += 0.201f;
                GameObject icon = Instantiate(GameState.instance.models.HeatPipeIcon, hex_cell.transform);
                icon.transform.position = v3;
                icon.transform.parent = hex_cell.transform;
                hex_cell.IsScrabbleForHeat = true;
            }
        }
        foreach (string tile in heats)
        {
            if(tileCoords == tile)
            {
                if(hex_cell.Y % 2 == 0)
                {
                    HeatPump heat_cell = (HeatPump)Instantiate(heatPumpPrefab, new Vector3(hex_cell.X * xOffset + 0.116f, 0.4f, hex_cell.Y * zOffset + 0.15f), Quaternion.identity);
                    heat_cell.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                    hex_cell.AddStructure<HeatPump>(heat_cell);

                }
                else
                {
                    HeatPump heat_cell = (HeatPump)Instantiate(heatPumpPrefab, new Vector3(hex_cell.X * xOffset + 1.455f, 0.4f, hex_cell.Y * zOffset + 0.155f), Quaternion.identity);
                    heat_cell.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                    hex_cell.AddStructure<HeatPump>(heat_cell);
                }
            }
        }

        foreach (string tile in solars)
        {
            if (tileCoords == tile)
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
                    SolarPanel solar_cell = (SolarPanel)Instantiate(solarPrefab, new Vector3(hex_cell.X + 1.8f, 0.2f, hex_cell.Y * zOffset - 0.065f), Quaternion.Euler(0, -90, 0));
                    solar_cell.transform.SetParent(hex_cell.transform);
                    solar_cell.name = "solar_" + hex_cell.X + "_" + hex_cell.Y;
                    hex_cell.AddStructure<SolarPanel>(solar_cell);
                }
            }
        }

        foreach (string tile in windTurbines)
        {
            if (tileCoords == tile)
            {
                if (hex_cell.Y % 2 == 0)
                {
                    House house_cell = (House)Instantiate(windTurbinePrefab, new Vector3(hex_cell.X * xOffset, 1.25f, hex_cell.Y * zOffset), Quaternion.Euler(0, 180, 0));
                    house_cell.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                    house_cell.IsWindmill = true;
                    hex_cell.AddStructure<House>(house_cell);
                }
                else
                {
                    House house_cell = (House)Instantiate(windTurbinePrefab, new Vector3(hex_cell.X * xOffset + 0.52f, 1.25f, hex_cell.Y * zOffset), Quaternion.Euler(0, 180, 0));
                    house_cell.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                    house_cell.IsWindmill = true;
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
            
            if (tile.IsSpecial())
            {
                if ((tile.GetSpecialOriginTile().Structure.SolarRequired ? Instigator.gameData.hasSolarInNetwork : true) && (tile.GetSpecialOriginTile().Structure.HeatRequired ? Instigator.gameData.hasHeatInNetwork : true)){
                    (bool valid, Tile tile, Tile source, Tile previous) result = isValidTileToChoose(tile.GetSpecialOriginTile(), Instigator);
                    var specialNeighbours = getSpecialNeighbours(tile.GetSpecialOriginTile());
                    bool specialValid = false;
                    foreach (var item in specialNeighbours)
                    {
                        if (isValidTileToChoose(item, Instigator).valid)
                        {
                            specialValid = true;
                        }
                    }
                    
                    if (tile.OwnedBy == null && tile.SelectedBy == null && specialValid)
                    {//Save previous steps of the connection for the connector to trace back origin
                        Instigator.gameData.tilesChosen.Add(tile);
                        tile.SelectedBy = Instigator;
                        GameState.instance.SelectedConnector.PreviousStep = result.previous;
                        GameState.instance.SelectedConnector.Source = result.source;
                        tile.Connector = GameState.instance.SelectedConnector;
                    }
                    

                    /*if (specialValid)
                    {
                        foreach (Tile t in specialNeighbours)
                        {
                            Instigator.gameData.tilesChosen.Add(t);
                            t.SelectedBy = Instigator;
                        }
                    }*/
                    /*Instigator.gameData.tilesChosen.Add(tile);
                    tile.SelectedBy = Instigator;*/
                }
                else
                {
                    GameState.instance.showWarningMessage(tile);
                    foreach (var item in neighbours)
                    {
                        if (item.Connector)
                        {
                            item.Connector.UsedForConnector = false;
                        }
                    }
                }
            }
            else
            {
                (bool valid, Tile tile, Tile source, Tile previous) result = isValidTileToChoose(tile, Instigator);
                if (result.valid)
                {
                    if (tile.OwnedBy == null && tile.SelectedBy == null /*&& ((Instigator.gameData.tilesChosen.Count > 0) ? (neighbours.Contains(Instigator.gameData.tilesChosen.Peek()) || neighbours.Exists(x=>x.OwnedBy == Instigator)) : true)*/)
                    {//Save previous steps of the connection for the connector to trace back origin
                        Instigator.gameData.tilesChosen.Add(tile);
                        tile.SelectedBy = Instigator;
                        GameState.instance.SelectedConnector.PreviousStep = result.previous;
                        GameState.instance.SelectedConnector.Source = result.source;
                        tile.Connector = GameState.instance.SelectedConnector;
                    }
                    //else if (tile.SelectedBy == Instigator && !tile.Connector.UsedForConnector)
                    //{//Remove all traces of the connector from the previous connector when it gets unselected

                    //    foreach (Tile t in tile.Connector.GetTiles())
                    //    {
                    //        Instigator.gameData.tilesChosen.Remove(t);
                    //        // t.Connector.PreviousStep.Connector.UsedForConnector = false;
                    //        t.SelectedBy = null;
                    //        t.Connector.PreviousStep = null;
                    //        t.Connector.Source = null;
                    //        t.Connector = null;
                    //    }
                    //}
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

    (bool valid, Tile tile, Tile Source, Tile PreviousStep) isValidTileToChoose(Tile t, PlayerState playerState)
    {
        List<Tile> neighbours = getNeigbours(t);
        foreach (Tile neighbour in neighbours)
        {
            bool go = true;
            if (neighbour.Structure.IsNode || isOccupiedBySamePlayer(neighbour, playerState))
            {
                if (neighbour.Structure is House || neighbour.IsSpecial())
                {
                    go = true;
                }
                else if (neighbour.Connector)
                {
                    go = false;
                    if (neighbour.Connector == GameState.instance.SelectedConnector)
                    {
                        go = true;
                    }
                    else if (neighbour.Connector.GetLastTile() == neighbour /*&& !neighbour.Connector.UsedForConnector*/ && neighbour == neighbour.Connector.GetLastTile())
                    {
                        if (!((t.Structure is HeatPump) ? !GameState.instance.SelectedConnector.IsHeat : false || (t.Structure is SolarPanel) ? !GameState.instance.SelectedConnector.IsSolar : false))
                        {
                            if (GameState.instance.SelectedConnector.IsSpecial == neighbour.Connector.IsSpecial)
                            {
                                neighbour.Connector.UsedForConnector = true;
                                go = true;
                            }
                        }
                    }
                }
                if (go)
                    return (true, t, (neighbour.Connector ? neighbour.Connector.Source : neighbour), neighbour);
            }
        }
        return (false, null, null, null);
    }
}