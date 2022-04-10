using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Connector : MonoBehaviour
{
    List<Tile> tiles = new List<Tile>();
    Tile endingTile;
    Tile startingTile;
    int length;
    Player ownedPlayer;
}
public class ConnectorManager : MonoBehaviour
{
    List<Connector> connectors = new List<Connector>();

    private void OnTileChosen()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            chooseTile();
        }
    }
    Tile chooseTile()
    {
        Tile tileTouched;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        RaycastHit hitInfo;


        if (Physics.Raycast(ray, out hitInfo))
        {

            GameObject tileObjectTouched = hitInfo.collider.transform.gameObject;
            tileTouched = tileObjectTouched.GetComponent<Tile>();

            MeshRenderer mr = tileTouched.GetComponentInChildren<MeshRenderer>();
            mr.material.color = Color.red;

            Debug.Log(tileTouched.name);
            
            return tileTouched;
        }

        return null;
    }
}
