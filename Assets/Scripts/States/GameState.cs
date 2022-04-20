using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    bool allSolar = true;
    bool allHeat = true;
    bool hasStandard = false;
    bool hasHeat = false;
    bool hasSolar = false;

    Button Pipe1;
    Button Pipe2;
    Button Pipe3;
    Button solarB;
    Button heatB;
    Button EndTurn;

    bool p1;
    bool p2;
    bool p3;
    public bool turnCheck;
    bool solarCheck;
    bool heatCheck;

    [HideInInspector]
    public TextMeshProUGUI text;


    public void Awake()
    {
        Assert.IsNull(instance);
        instance = this;


        addEventHandlers();

        //Assert.IsNotNull(config.PlayerStateClass);
        //Assert.IsTrue(PlayerStarts.Length > 0);

        Debug.Log($"TileManager - {(config.TileManagerClass ? "true" : "false")}");
        Debug.Log($"PlayerState in GameState - {(config.PlayerStateClass ? "true" : "false")}");

        if (config.TileManagerClass)
        {
            tileManager = Instantiate(config.TileManagerClass);
        }
        createPlayer();
        turnConnections = new List<Connection>();

    }

    public virtual void TutorialStart()
    {
    }

    private void addEventHandlers()
    {
        Pipe1 = GameObject.Find("firstBtn").GetComponent<Button>();
        Pipe2 = GameObject.Find("secondBtn").GetComponent<Button>();
        Pipe3 = GameObject.Find("thirdBtn").GetComponent<Button>();
        EndTurn = GameObject.Find("EndTurnBtn").GetComponent<Button>();
        solarB = GameObject.Find("fourthBtn").GetComponent<Button>();
        heatB = GameObject.Find("fifthBtn").GetComponent<Button>();

        Pipe1.onClick.AddListener(FirstpipeCheck);
        Pipe2.onClick.AddListener(SecondpipeCheck);
        Pipe3.onClick.AddListener(ThirdpipeCheck);
        EndTurn.onClick.AddListener(endTurnCheck);
        solarB.onClick.AddListener(SolarCheck);
        heatB.onClick.AddListener(HeatCheck);

    }



    public void FirstpipeCheck()
    {
        Debug.Log("FIRST PIPE HAS BEEN PICKED");
        p1 ^= true;
    }

    public void SecondpipeCheck()
    {
        p2 ^= true;
    }
    public void ThirdpipeCheck()
    {
        p3 ^= true;
    }
    public void endTurnCheck()
    {
        turnCheck ^= true;
    }
    public void SolarCheck()
    {
        solarCheck ^= true;
    }
    public void HeatCheck()
    {
        heatCheck ^= true;
    }


    Node playerNode;
    bool placingNode;

    Connector selectedConnector;
    Connection currentConnection;
    List<Connection> turnConnections;

    private void createPlayer()
    {
        playerStates.Add(Instantiate(config.PlayerStateClass));
        playerStates.Add(Instantiate(config.PlayerStateClass));
        playerStates[0].RefillHand();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || turnCheck)
        {
            turnCheck = false;
            if (selectedConnector == null || selectedConnector.getLength() >= selectedConnector.MaxLength)
            {
                if (playerStates[0].gameData.isTurn)
                {
                    foreach (Connection conn in turnConnections)
                    {
                        foreach (Connector connector in conn.Connectors)
                        {
                            if (conn == SolarConnection && !connector.GetType().Equals(typeof(SolarPanelConnector)))
                            {
                                allSolar = false;
                            }
                            if (conn == HeatConnection && !connector.GetType().Equals(typeof(HeatPipeConnector)))
                            {
                                allHeat = false;
                            }
                        }
                        if ((isSolarConnectionEnd && allSolar) || (isHeatConnectionEnd && allHeat) || (hasStandard && !hasHeat && !hasSolar && isNormalConnectionEnd))
                        {
                            playerStates[0].RefillHand();
                            playerStates[0].gameData.isTurn = false;
                            playerStates[0].EndTurn();

                            foreach (Connection c in turnConnections)
                            {
                                playerStates[0].FinalizeConnection(c);
                            }
                            if (isSolarConnectionEnd && allSolar)
                            {
                                playerStates[0].gameData.hasSolarInNetwork = true;
                            }
                            if (isHeatConnectionEnd && allHeat)
                            {
                                playerStates[0].gameData.hasHeatInNetwork = true;
                            }
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
                    turnConnections = new List<Connection>();
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
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                playerStates[0].AbortConnection(currentConnection);
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha1) || p1)
            {
                p1 = false;
                if (selectedConnector != null)
                {
                    playerStates[0].gameData.tilesChosen.Clear();
                    playerStates[0].AbortConnector(currentConnection, false);
                    selectedConnector = null;
                }
                else
                {
                    selectedConnector = playerStates[0].gameData.Inventory.Find(x => x.MaxLength == 1);
                    playerStates[0].gameData.Inventory.Remove(selectedConnector);
                    hasStandard = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) || p2)
            {
                p2 = false;
                if (selectedConnector != null)
                {
                    playerStates[0].gameData.tilesChosen.Clear();
                    playerStates[0].AbortConnector(currentConnection, false);
                    selectedConnector = null;
                }
                else
                {
                    selectedConnector = playerStates[0].gameData.Inventory.Find(x => x.MaxLength == 2);
                    playerStates[0].gameData.Inventory.Remove(selectedConnector);
                    hasStandard = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) || p3)
            {
                p3 = false;
                if (selectedConnector != null)
                {
                    playerStates[0].gameData.tilesChosen.Clear();
                    playerStates[0].AbortConnector(currentConnection, false);
                    selectedConnector = null;
                }
                else
                {
                    selectedConnector = playerStates[0].gameData.Inventory.Find(x => x.MaxLength == 3);
                    playerStates[0].gameData.Inventory.Remove(selectedConnector);
                    hasStandard = true;
                }

            }
            if (Input.GetKeyDown(KeyCode.Alpha4) || solarCheck)
            {
                solarCheck = false;
                if (selectedConnector != null)
                {
                    playerStates[0].gameData.tilesChosen.Clear();
                    playerStates[0].AbortConnector(currentConnection, false);
                    selectedConnector = null;
                }
                else
                {
                    selectedConnector = playerStates[0].gameData.SpecialConnector.Find(x => x.IsSolar);
                    hasSolar = true;
                    playerStates[0].gameData.SpecialConnector.Remove(selectedConnector);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha5) || heatCheck)
            {
                heatCheck = false;
                if (selectedConnector != null)
                {
                    playerStates[0].gameData.tilesChosen.Clear();
                    playerStates[0].AbortConnector(currentConnection, false);
                    selectedConnector = null;
                }
                else
                {
                    selectedConnector = playerStates[0].gameData.SpecialConnector.Find(x => x.IsHeat);
                    hasHeat = true;
                    playerStates[0].gameData.SpecialConnector.Remove(selectedConnector);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                if (placingNode)
                {
                    playerStates[0].gameData.nodesOwned.Add(playerNode);
                    playerNode = null;
                    placingNode = false;
                }
                else
                {
                    if (playerStates[0].gameData.nodesOwned.Count > 0)
                    {
                        playerNode = playerStates[0].gameData.nodesOwned[0];
                        playerStates[0].gameData.nodesOwned.Remove(playerNode);
                        placingNode = true;
                    }
                }
            }

            if (!(Input.GetMouseButtonDown(0) && !selectedConnector))
            {
                if (!placingNode && Input.GetMouseButtonDown(0) && (selectedConnector == null || selectedConnector.MaxLength > selectedConnector.getLength()) && !(EventSystem.current.IsPointerOverGameObject()))
                {
                    Tile t = chooseTile();
                    if (t != null)
                    {
                        if (t.SelectedBy != null && selectedConnector != null && !(t == selectedConnector.GetLastTile()))
                        {
                            if (t.IsSpecial() && isNormalConnectionEnd)
                            {
                                foreach (Tile tile in tileManager.getSpecialNeighbours(t))
                                {
                                    selectedConnector.AddTile(t);
                                }
                                if (currentConnection == null)
                                {
                                    currentConnection = playerStates[0].StartConnection();
                                    turnConnections.Add(currentConnection);
                                }
                                currentConnection.Connectors.Add(selectedConnector);
                                selectedConnector = null;
                            }
                            else
                            {
                                selectedConnector.AddTile(t);

                                if (selectedConnector.MaxLength == selectedConnector.getLength())
                                {
                                    if (currentConnection == null)
                                    {
                                        currentConnection = playerStates[0].StartConnection();
                                        turnConnections.Add(currentConnection);
                                    }
                                    currentConnection.Connectors.Add(selectedConnector);
                                    selectedConnector = null;
                                }
                            }
                        }
                        else
                        {
                            selectedConnector.RemoveTile(t);
                        }
                    }
                }
            }
            else if(Input.GetMouseButtonDown(0) && placingNode)
            {
                Tile t = PlaceNode();

                if(!(t == null))
                {
                    placingNode = false;
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && !selectedConnector)
            GetInfoCard();
        TutorialStart();
        startPoint();
    }


    public virtual void startPoint()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            TileManager.tiles[12][16].OwnedBy = playerStates[0];
        }
    }


    Connection SolarConnection;
    Connection HeatConnection;
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
            if(tileTouched == null)
            {
                tileTouched = tileObjectTouched.GetComponentInParent<Tile>();
                if (tileTouched == null)
                {
                    return null;
                }
            }

            if (selectedConnector.getLength() < 2 || (selectedConnector.GetTiles().Contains(tileTouched)) || Connector.IsValidLengthThree(selectedConnector.GetTiles()[0], selectedConnector.GetTiles()[1], tileTouched))
            {
                if (selectedConnector.MaxLength == (selectedConnector.getLength() + 1) && tileManager.isOccupied(tileTouched) && !tileManager.IsHeatPipe(tileTouched) && !tileManager.IsSolarPanel(tileTouched))
                { 
                    isNormalConnectionEnd = true;
                    currentConnection = new Connection();
                    turnConnections.Add(currentConnection);
                }
                else if (tileManager.IsHeatPipe(tileTouched))
                {
                    isHeatConnectionEnd = true;
                    HeatConnection = currentConnection;
                }
                else if (tileManager.IsSolarPanel(tileTouched))
                {
                    isSolarConnectionEnd = true;
                    SolarConnection = currentConnection;
                    currentConnection = new Connection();
                    turnConnections.Add(currentConnection);
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

    Tile GetInfoCard()
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
                tileTouched = tileObjectTouched.GetComponentInParent<Tile>();
                if (tileTouched == null)
                {
                    return null;
                }
            }

            if (tileTouched.IsSpecial())
            {
                tileTouched.GetSpecialOriginTile().openInfoCard(playerStates[0]);
            }


            return tileTouched;
        }
        return null;
    }

    Tile PlaceNode()
    {
        Tile tileTouched;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject tileObjectTouched = hitInfo.collider.transform.gameObject;
            tileTouched = tileObjectTouched.GetComponent<Tile>();

            foreach (Tile t in tileManager.getNeigbours(tileTouched))
            {
                if (t.Structure.GetType() != typeof(EmptyStructure))
                {
                    return null;
                }
            }

            if (tileTouched.SelectedBy != null || tileTouched.OwnedBy != null || tileTouched.occupied || tileTouched.IsSpecial())
            {
                return null;
            }

            Node nodeP = Instantiate(tileManager.nodePrefab, (tileTouched.Y % 2 == 0) ? new Vector3(tileTouched.X * 1.04f, 0.2f, tileTouched.Y * 0.9f) : new Vector3(tileTouched.X * 1.04f + 0.498f, 0.2f, tileTouched.Y * 0.9f - 0.084f), Quaternion.identity);
            tileTouched.AddStructure<Tile>(nodeP);
            playerNode = null;
            return tileTouched;
        }
        return null;
    }
}

//insufficient length of connector used but still can end turn

//choose a random tiles next to one in the connection is allowed

//if you choose 3, after you placed all 3 you can't undo no more, not sure if it is a problem 
//at all


