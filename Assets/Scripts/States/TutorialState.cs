using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public struct TutorialStateConfig
{
    public PlayerState PlayerStateClass;
    public TileManager TileManagerClass;
}


public class TutorialState : MonoBehaviour
{
    public TutorialStateConfig config;

    [HideInInspector]
    public List<PlayerState> playerStates;

    [HideInInspector]
    public TileManager tileManager;

    public static TutorialState instance { get; private set; }

    bool firstPipeCheck = false;


    Button button1;


    public void Awake()
    {
        Assert.IsNull(instance);
        instance = this;

        //Assert.IsNotNull(config.PlayerStateClass);
        //Assert.IsTrue(PlayerStarts.Length > 0);

        //Debug.Log($"TileManager - {(config.TileManagerClass ? "true" : "false")}");
        //Debug.Log($"PlayerState in GameState - {(config.PlayerStateClass ? "true" : "false")}");
        Debug.Log($"Welcome to the ConnectMarr Tutorial. To begin please press on the start turn key");
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
        playerStates[0].RefillHand();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (playerStates[0].gameData.isTurn)
            {
                playerStates[0].gameData.isTurn = false;
                playerStates[0].EndTurn();

                playerStates[0].FinalizeConnection(currentConnection);
                currentConnection = null;
            }
            else
            {
                playerStates[0].RefillHand();
                playerStates[0].gameData.isTurn = true;
            }
            Debug.Log("isTurn" + playerStates[0].gameData.isTurn.ToString());
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
                selectedConnector = playerStates[0].gameData.Inventory.Find(x => x.MaxLength == 3);
                playerStates[0].gameData.Inventory.Remove(selectedConnector);
            }

            if (Input.GetMouseButtonDown(0) && selectedConnector.MaxLength > selectedConnector.getLength() && !EventSystem.current.IsPointerOverGameObject())
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    Debug.Log("Clicked on the UI");
                } else
                {
                    Tile t = chooseTile();
                    if (t.SelectedBy != null && !(t == selectedConnector.GetLastTile()))
                    {
                        selectedConnector.AddTile(t);
                        if (selectedConnector.MaxLength == selectedConnector.getLength())
                        {
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
                
            }
            //pressed = false;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            tileManager.tiles[0][0].OwnedBy = playerStates[0];
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

            if (!tileManager.isOccupied(tileTouched))
                tileTouched.onSelected(playerStates[0]);
            return tileTouched;
        }

        return null;
    }
}
