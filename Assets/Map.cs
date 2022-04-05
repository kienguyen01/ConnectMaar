using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{

    public GameObject hexPrefab;
    public GameObject housePrefab;
    public GameObject solarPrefab;

    int WIDTH_MAP = 20;
    int HEIGHT_MAP = 20;

    float xOffset = 1f;
    float zOffset = 0.86f;

    // Start is called before the first frame update
    void Start()
    {
        House myHouse = housePrefab.GetComponent<House>();
        for (int x = -3; x < WIDTH_MAP - 6; x++)
        {
            for (int y = 9; y < HEIGHT_MAP; y++)
            {
                float xPos = x * xOffset;
                // check odd row => go inside
                if(y % 2 != 0)
                {
                    xPos += xOffset / 2f;
                }
                GameObject hex_cell = (GameObject)Instantiate(hexPrefab, new Vector3(xPos, 0, y * zOffset), Quaternion.identity);
                hex_cell.name = "Hex_" + x + "_" + y;
                hex_cell.transform.SetParent(this.transform);

                if ((x == 3 && y == 12) || (x == 5 && y == 10) || (x == 7 && y == 15) || (x == -2 && y == 19))
                {
                    GameObject house_cell = (GameObject)Instantiate(housePrefab, new Vector3(xPos, 0.2f, y * zOffset), Quaternion.identity);
                    house_cell.transform.SetParent(transform);
                    house_cell.name = "house_" + x + "_" + y;
                }

                if((x == -2 && y == 12) || (x == -3 && y == 19) || (x == 5 && y == 19) || (x == -3 && y == 16))
                {
                    GameObject solar_cell = (GameObject)Instantiate(solarPrefab, new Vector3(xPos, 0.2f, y * zOffset), Quaternion.identity);
                    solar_cell.transform.SetParent(transform);
                    solar_cell.name = "solar_" + x + "_" + y;
                }
                
                
            }
        }

        for (int x = 0; x > -WIDTH_MAP; x--)
        {
            for (int y = 0; y < HEIGHT_MAP / 2; y++)
            {
                float xPos = x * xOffset;
                // check odd row => go inside
                if (y % 2 != 0)
                {
                    xPos += xOffset / 2f;
                }
                GameObject hex_cell = (GameObject)Instantiate(hexPrefab, new Vector3(xPos, 0, y * zOffset), Quaternion.identity);

                hex_cell.name = "Hex_" + x + "_" + y;

                hex_cell.transform.SetParent(this.transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
