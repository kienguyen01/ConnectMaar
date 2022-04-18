using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public struct GameStateConfig
{
    public PlayerState PlayerStateClass;
    public TileManager TileManagerClass;
}

public class GameState : MonoBehaviour
{
    public GameStateConfig config;

    [HideInInspector]
    public List<PlayerState> playerStates;

    [HideInInspector]
    public TileManager tileManager;

    public static GameState instance { get; private set; }

    public void Awake()
    {
        Assert.IsNull(instance);
        instance = this;

        //Assert.IsNotNull(config.PlayerStateClass);
        //Assert.IsTrue(PlayerStarts.Length > 0);

        Debug.Log($"TileManager - {(config.TileManagerClass ? "true" : "false")}");
        Debug.Log($"PlayerState in GameState - {(config.PlayerStateClass ? "true" : "false")}");

        if (config.TileManagerClass)
        {
            tileManager = Instantiate(config.TileManagerClass);
        }

        createPlayer();

    }



    Connector selectedConnector;
    Connection currentConnection;

    private void createPlayer()
    {
        playerStates.Add(Instantiate(config.PlayerStateClass));
        playerStates.Add(Instantiate(config.PlayerStateClass));
        playerStates[0].RefillHand();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (selectedConnector == null || selectedConnector.getLength() == selectedConnector.MaxLength)
            {
                if (playerStates[0].gameData.isTurn && isConnectionEnd)
                {
                    playerStates[0].RefillHand();
                    playerStates[0].gameData.isTurn = false;
                    playerStates[0].EndTurn();

                    playerStates[0].FinalizeConnection(currentConnection);
                    currentConnection = null;
                    isConnectionEnd = false;
                }
                else
                {
                    playerStates[0].gameData.isTurn = true;
                }
                Debug.Log("isTurn" + playerStates[0].gameData.isTurn.ToString());
            }
        }
        if (playerStates[0].gameData.isTurn)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (selectedConnector != null)
                {
                    playerStates[0].gameData.Inventory.Add(selectedConnector);
                    foreach (Tile t in selectedConnector.GetTiles())
                    {
                        t.SelectedBy = null;
                    }
                    playerStates[0].gameData.tilesChosen.Clear();
                    playerStates[0].AbortConnection(currentConnection);
                }
                selectedConnector = playerStates[0].gameData.Inventory.Find(x => x.MaxLength == 1);
                playerStates[0].gameData.Inventory.Remove(selectedConnector);
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (selectedConnector != null)
                {
                    playerStates[0].gameData.Inventory.Add(selectedConnector);
                    foreach (Tile t in selectedConnector.GetTiles())
                    {
                        t.SelectedBy = null;
                    }
                    playerStates[0].gameData.tilesChosen.Clear();
                    playerStates[0].AbortConnection(currentConnection);
                }
                selectedConnector = playerStates[0].gameData.Inventory.Find(x => x.MaxLength == 2);
                playerStates[0].gameData.Inventory.Remove(selectedConnector);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if(selectedConnector != null)
                {
                    playerStates[0].gameData.Inventory.Add(selectedConnector);
                    foreach (Tile t in selectedConnector.GetTiles())
                    {
                        t.SelectedBy = null;
                    }
                    playerStates[0].gameData.tilesChosen.Clear();
                    playerStates[0].AbortConnection(currentConnection);
                }
                selectedConnector = playerStates[0].gameData.Inventory.Find(x => x.MaxLength == 3);
                
                playerStates[0].gameData.Inventory.Remove(selectedConnector);
            }

            if (Input.GetMouseButtonDown(0) && selectedConnector.MaxLength > selectedConnector.getLength())
            {
                Tile t = chooseTile();
                if (t.SelectedBy != null && !(t == selectedConnector.GetLastTile()))
                {
                    selectedConnector.AddTile(t);
                    if (selectedConnector.MaxLength == selectedConnector.getLength())
                    {
                        if(selectedConnector.getLength() == 3)
                        {
                                    
                            //selectedConnector.IsValidLengthThree()
                        }
                        if (currentConnection == null)
                        {
                            currentConnection = playerStates[0].StartConnection();
                        }
                        currentConnection.Connectors.Add(selectedConnector);
                        selectedConnector = null;
                    }
                }
                else
                {
                    selectedConnector.RemoveTile(t);
                }
            }
            //pressed = false;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            tileManager.tiles[12][16].OwnedBy = playerStates[0];
        }
        
    }

    /*void selectConnector(int Length)
    {
        //switch (Length)
        //{
        //    case 1:
        //        chooseTile(1);
        //        break;
        //    case 2:
        //        chooseTile(2);
        //        break;
        //    case 3:
        //        chooseTile(3);
        //        break;
        //    default: break;
        //}
    }*/

    bool isConnectionEnd = false;

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

            if (selectedConnector.getLength() < 2 || Connector.IsValidLengthThree(selectedConnector.GetTiles()[0], selectedConnector.GetTiles()[1], tileTouched))
            {
                if (tileManager.isOccupied(tileTouched))
                {
                    isConnectionEnd = true;
                }
                else if (!tileManager.isOccupied(tileTouched))
                {
                    isConnectionEnd = false;
                }
                if (tileTouched.OwnedBy == null)
                    tileTouched.onSelected(playerStates[0]);
            }
            return tileTouched;
        }
        
        return null;
    }
}

//insufficient length of connector used but still can end turn

//choose a random tiles next to one in the connection is allowed

//if you choose 3, after you placed all 3 you can't undo no more, not sure if it is a problem 
//at all


