using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

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
    bool allSolar = true;
    bool allHeat = true;
    bool hasStandard = false;
    bool hasHeat = false;
    bool hasSolar = false;
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
        playerStates[0].RefillHand();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (selectedConnector == null || selectedConnector.getLength() == selectedConnector.MaxLength)
            {
                if (playerStates[0].gameData.isTurn)
                {
                    foreach (Connector connector in currentConnection.Connectors)
                    {
                        if (!connector.GetType().Equals(typeof(SolarPanelConnector)))
                        {
                            allSolar = false;
                        }
                        if (!connector.GetType().Equals(typeof(HeatPipeConnector)))
                        {
                            allHeat = false;
                        }
                        foreach (Tile t in connector.GetTiles())
                        {
                            if (t.IsScrabbleForHeat)
                                playerStates[0].AddHeatPipeConnector();

                            if (t.IsScrabbleForSolar)
                                playerStates[0].AddSolarConnector();
                        }
                    }
                    if ((isSolarConnectionEnd && allSolar) || (isHeatConnectionEnd && allHeat) || (hasStandard && !hasHeat && !hasSolar && isNormalConnectionEnd))
                    {
                        playerStates[0].RefillHand();
                        playerStates[0].gameData.isTurn = false;
                        playerStates[0].EndTurn();

                        playerStates[0].FinalizeConnection(currentConnection);
                        if (isSolarConnectionEnd && allSolar)
                        {
                            playerStates[0].gameData.hasSolarInNetwork = true;
                        }
                        if (isHeatConnectionEnd && allHeat)
                        {
                            playerStates[0].gameData.hasHeatInNetwork = true;
                        }
                    }
                    hasHeat = false;
                    hasSolar = false;
                    allSolar = true;
                    allHeat = true;
                    hasStandard = false;
                    isHeatConnectionEnd = false;
                    isSolarConnectionEnd = false;
                    currentConnection = null;
                    isNormalConnectionEnd = false;
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
                hasStandard = true;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("2 chosen");
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
                hasStandard = true;

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
                Debug.Log("3 chosen");
                selectedConnector = playerStates[0].gameData.Inventory.Find(x => x.MaxLength == 3);
                playerStates[0].gameData.Inventory.Remove(selectedConnector);
                hasStandard = true;

            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (selectedConnector != null)
                {
                    playerStates[0].gameData.SpecialConnector.Add(selectedConnector);
                    foreach (Tile t in selectedConnector.GetTiles())
                    {
                        t.SelectedBy = null;
                    }
                    playerStates[0].gameData.tilesChosen.Clear();
                    playerStates[0].AbortConnection(currentConnection);
                }
                selectedConnector = playerStates[0].gameData.SpecialConnector.Find(x => x.IsSolar);
                hasSolar = true;
                playerStates[0].gameData.SpecialConnector.Remove(selectedConnector);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                if (selectedConnector != null)
                {
                    playerStates[0].gameData.SpecialConnector.Add(selectedConnector);
                    foreach (Tile t in selectedConnector.GetTiles())
                    {
                        t.SelectedBy = null;
                    }
                    playerStates[0].gameData.tilesChosen.Clear();
                    playerStates[0].AbortConnection(currentConnection);
                }
                selectedConnector = playerStates[0].gameData.SpecialConnector.Find(x => x.IsHeat);
                hasHeat = true;
                playerStates[0].gameData.SpecialConnector.Remove(selectedConnector);
            }
            if (Input.GetMouseButtonDown(0) && (selectedConnector == null || selectedConnector.MaxLength > selectedConnector.getLength()) && !(EventSystem.current.IsPointerOverGameObject()))
            {
                Tile t = chooseTile();
                if (t != null)
                {
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
            TileManager.tiles[10][10].OwnedBy = playerStates[0];
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

    bool isNormalConnectionEnd = false;
    bool isHeatConnectionEnd = false;
    bool isSolarConnectionEnd = false;
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
            if (tileTouched == null)
            {
                return null;
            }

            if (selectedConnector.getLength() < 2 || Connector.IsValidLengthThree(selectedConnector.GetTiles()[0], selectedConnector.GetTiles()[1], tileTouched))
            {
                if (tileManager.isOccupied(tileTouched) && !tileManager.IsHeatPipe(tileTouched) && !tileManager.IsSolarPanel(tileTouched))
                {
                    isNormalConnectionEnd = true;
                }
                else if (tileManager.IsHeatPipe(tileTouched))
                {
                    isHeatConnectionEnd = true;
                }
                else if (tileManager.IsSolarPanel(tileTouched))
                {
                    isSolarConnectionEnd = true;
                }
                else if (!tileManager.isOccupied(tileTouched))
                {
                    isNormalConnectionEnd = false;
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


