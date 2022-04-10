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
        hex_cell.onSelected += (Player Instigator) =>
        {
            //Assert
            MeshRenderer mr = hex_cell.GetComponentInChildren<MeshRenderer>();
            mr.material.color = Color.red;

            List<Tile> temp = getNeigbours(hex_cell);
        };

        hex_cell.transform.SetParent(this.transform);

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
}
