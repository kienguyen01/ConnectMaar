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

    /// <summary>
    /// Function that selects one tile when tapped
    /// </summary>
    /// <returns></returns>
    Tile chooseTile()
    {
        Tile tileTouched;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        RaycastHit hitInfo;


        if (Physics.Raycast(ray, out hitInfo))
        {

            GameObject tileObjectTouched = hitInfo.collider.transform.gameObject;
            tileTouched = tileObjectTouched.GetComponent<Tile>();

            tileTouched.onSelected(new Player());
            
            return tileTouched;
        }

        return null;
    }
}
