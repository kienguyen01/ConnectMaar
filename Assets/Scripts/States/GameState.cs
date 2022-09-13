using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using mixpanel;
using Photon.Pun;
using System;
using UnityEngine.Localization.Settings;

[System.Serializable]
public struct GameStateConfig
{
    public PlayerState PlayerStateClass;
    public TileManager TileManagerClass;
    public Timer timer;
}

public class GameState : MonoBehaviourPunCallbacks
{
    public GameStateConfig config;

    [HideInInspector]
    public PlayerState player1;
    [HideInInspector]
    public PlayerState player2;


    [HideInInspector]
    public TileManager tileManager;

    public static GameState instance { get; private set; }
    public Connector SelectedConnector { get => selectedConnector; private set => selectedConnector = value; }

    protected bool allSolar = true;
    protected bool allHeat = true;
    protected bool hasStandard = false;
    protected bool hasHeat = false;
    protected bool hasSolar = false;

    TextMeshProUGUI BtnClicked;
    TextMeshProUGUI EndBtnMsg;


    protected bool p1;
    protected bool p2;
    public bool p3;
    public bool turnCheck;
    protected bool solarCheck;
    protected bool heatCheck;
    protected bool clearBtn;
    protected bool nodeCheck;
    /*protected Connection SolarConnection;
    protected Connection HeatConnection;*/
    protected bool isNormalConnectionEnd = false;
    protected bool isHeatConnectionEnd = false;
    protected bool isSolarConnectionEnd = false;

    public int Connector1Count;
    public int Connector2Count;
    public int Connector3Count;


    [HideInInspector]
    public TextMeshProUGUI text;


    public static PopupHandler pH;

    //public Timer turnTime;

    Value props;

    protected Node playerNode;
    protected bool placingNode;

    protected List<Connector> turnConnectors;
    protected Connector selectedConnector;
    /*protected Connection currentConnection;
    protected List<Connection> turnConnections;*/


    public void Awake()
    {
        onAwake();
    }

    protected void onAwake()
    {

        Assert.IsNull(instance);
        instance = this;  
        // Track with event-name and property

        GameState.Track("Run application", ("Test", Application.isEditor), ("DeviceName", SystemInfo.deviceName));


        //addEventHandlers();
        GameObject ExStadium = GameObject.Find("ExitBtnStadiumAFAS");
        ExStadium.GetComponent<Button>()
            .onClick.AddListener(
            () => {
                TileManager.pH.canvas = ExStadium.transform.parent.parent.gameObject.GetComponent<Canvas>();
                TileManager.pH.Popup();
            });

        GameObject ExInvesta = GameObject.Find("InvestaExitBtn");
        ExInvesta.GetComponent<Button>()
            .onClick.AddListener(
            () => {
                TileManager.pH.canvas = ExInvesta.transform.parent.parent.gameObject.GetComponent<Canvas>();
                TileManager.pH.Popup();
            });

        GameObject ExBloemwijk = GameObject.Find("BloemwijkExitBtn");
        ExBloemwijk.GetComponent<Button>()
            .onClick.AddListener(
            () => {
                TileManager.pH.canvas = ExBloemwijk.transform.parent.parent.gameObject.GetComponent<Canvas>();
                TileManager.pH.Popup();
            });

        GameObject ExChurch = GameObject.Find("ExitBtnChurch");
        ExChurch.GetComponent<Button>().onClick.AddListener(
            () => {
                TileManager.pH.canvas = ExChurch.transform.parent.parent.gameObject.GetComponent<Canvas>();
                TileManager.pH.Popup();
            });

        GameObject ExDeMeent = GameObject.Find("ExitBtnDeMeent");
        ExDeMeent.GetComponent<Button>().onClick.AddListener(
            () => {
                TileManager.pH.canvas = ExDeMeent.transform.parent.parent.gameObject.GetComponent<Canvas>();
                TileManager.pH.Popup();
            });
        GameObject ExDalton = GameObject.Find("ExitBtnDalton");
        ExDalton.GetComponent<Button>().onClick.AddListener(
            () => {
                TileManager.pH.canvas = ExDalton.transform.parent.parent.gameObject.GetComponent<Canvas>();
                TileManager.pH.Popup();
            });
        //Assert.IsNotNull(config.PlayerStateClass);
        //Assert.IsTrue(PlayerStarts.Length > 0);

        pH = this.gameObject.AddComponent(typeof(PopupHandler)) as PopupHandler;

        Debug.Log($"TileManager - {(config.TileManagerClass ? "true" : "false")}");
        Debug.Log($"PlayerState in GameState - {(config.PlayerStateClass ? "true" : "false")}");

        if (config.TileManagerClass)
        {
            tileManager = Instantiate(config.TileManagerClass);
        }
        createPlayer();
        //turnConnections = new List<Connection>();
        turnConnectors = new List<Connector>();

        Connector1Count = 0;
        Connector2Count = 0;
        Connector3Count = 0;


    }

    private void Update()
    {
        TurnMsg();

        CheckEndTurn();

        if (player1.gameData.IsTurn)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0) || clearBtn)
            {
                clearAllSelected();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1) || p1)
            {
                Debug.Log("select 1");
                SelectSingleConnector();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) || p2)
            {
                Debug.Log("select 2");
                SelectDoubleConnector();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) || p3)
            {
                Debug.Log("select 3");
                SelectTripleConnector();
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) || solarCheck)
            {
                SelectSolarConnector();
            }
            if (Input.GetKeyDown(KeyCode.Alpha5) || heatCheck)
            {
                SelectHeatConnector();
            }
            if (Input.GetKeyDown(KeyCode.Alpha6) || nodeCheck)
            {
                SelectNodeConnector();
            }

            if (Input.GetMouseButtonDown(0))
            {
                HandleClickCheck();
            }
        }

        if (Input.GetMouseButtonDown(0) && !selectedConnector)
            GetInfoCard(player1);

        startPoint();
    }

    public static void Track(string eventName, params (string key, object value)[] props)
    {
        Value eventProperty = new Value();
        foreach (var item in props)
        {
            eventProperty[item.key] = item.value.ToString();
        }
        Mixpanel.Track(eventName, eventProperty);
    }


    public void FirstpipeCheck()
    {
        p1 ^= true;
        if (p1 == true)
        {
            //BtnClicked.text = "1 Connector";
            Debug.Log("1 -- Pipe");
        }

    }
    public void NodeCheck()
    {
        nodeCheck ^= true;
    }

    public void clearBtnCheck()
    {
        clearBtn ^= true;
    }

    public void SecondpipeCheck()
    {
        p2 ^= true;
        if (p2 == true)
        {
            //BtnClicked.text = "2 Connector";
            Debug.Log("2 -- Pipe");
        }
    }

    public void ThirdpipeCheck()
    {
        p3 ^= true;
        if (p3 == true)
        {
            //BtnClicked.text = "3 Connector";
            Debug.Log("3 -- Pipe");
        }
    }

    /*public void SolarCheck()
    {
        solarCheck ^= true;
        if (solarCheck == true)
        {
            SelectedConnectorText.text = "Solar Coneector";
        }
    }*/

    /*public void HeatCheck()
    {
        heatCheck ^= true;
        if (heatCheck == true)
        {
            SelectedConnectorText.text = "Heat Connector";
        }
    }*/


    protected void createPlayer()
    {
        if(player1 == null)
        {
            player1 = Instantiate(config.PlayerStateClass)
                      .RefillHand();
        }
        else if (player2 == null)
        {
            player2 = Instantiate(config.PlayerStateClass)
                      .RefillHand();
        }
    }

    private void TurnMsg()
    {
        EndBtnMsg = GameObject.Find("EndBtnMsg").GetComponent<TextMeshProUGUI>();
        if (player1.gameData.IsTurn == true)
        {
            if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
            {
                EndBtnMsg.text = "Einde Draai";
            }
            else
            {
                EndBtnMsg.text = "End  Turn";
            }
            //EndBtnMsg.text = "End  Turn";


        }
        else
        {
            if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
            {
                EndBtnMsg.text = "Start Draai";
            }
            else
            {
                EndBtnMsg.text = "Start Turn";
            }
                //EndBtnMsg.text = "Start Turn";
        }
    }

    protected void HandleClickCheck()
    {
        if (selectedConnector)
        {
            if (!placingNode && Input.GetMouseButtonDown(0) && (selectedConnector == null || selectedConnector.MaxLength > selectedConnector.getLength()) && !(EventSystem.current.IsPointerOverGameObject()))
            {
                var result = chooseTile(player1);
                GameState.Track("Click", ("SelectedConnector", (!selectedConnector) ? "Not Selected" : selectedConnector.GetType().ToString()));
                TrackTile(result.Tile, player1);
                if (result.Valid)
                {
                    if (result.Tile.SelectedBy != null && selectedConnector != null && !(result.Tile == selectedConnector.GetLastTile()))
                    {
                        if (result.Tile.IsSpecial())
                        {
                            foreach (Tile tile in tileManager.getSpecialNeighbours(result.Tile))
                            {
                                selectedConnector.AddTile(result.Tile);
                            }
                            turnConnectors.Add(selectedConnector);
                            selectedConnector = null;
                        }
                        else
                        {
                            selectedConnector.AddTile(result.Tile);

                            if (selectedConnector.MaxLength == selectedConnector.getLength())
                            {
                                turnConnectors.Add(selectedConnector);
                                selectedConnector = null;
                            }
                        }
                    }
                    else
                    {
                        selectedConnector.RemoveTile(result.Tile);
                    }
                }
            }
        }
        else if (placingNode)
        {
            CheckPlaceNode();
        }
    }

    private static void TrackTile(Tile t, PlayerState p)
    {
        if (t)
        {
            GameState.Track("TileTouched", ("X", t.X), ("Y", t.Y), ("StructureType", t.Structure.GetType()), ("isSelected", t.SelectedBy ? "Selected" : "Not Selected"), ("OwnedBy", (t.OwnedBy == p) ? "Local Player" : "Other"));
        }
    }

    protected void CheckPlaceNode()
    {
        Tile t = PlaceNode();

        if (!(t == null))
        {
            placingNode = false;
        }
    }

    protected bool CheckEndTurn()
    {
        bool returnObj = false;
        //Debug.LogError("asdasdfgjkl");

        if (Input.GetKeyDown(KeyCode.Space) || turnCheck || (config.timer ? config.timer.isOver : true))
        {
            //turnTime.endTurn = false;
            //turnTime.SetDuration(10).Begin();
            
            turnCheck = false;
            if (selectedConnector == null || selectedConnector.getLength() >= selectedConnector.MaxLength)
            {
                if (player1.gameData.IsTurn)
                {
                    returnObj = true;
                    foreach (Connector ctr in turnConnectors)
                    {
                        foreach (Tile t in ctr.GetTiles())
                        {
                            //check if t is scrablle and find a finished connection

                            CheckScrabbleRewards(t);
                        }

                        player1.RefillHand();
                        player1.gameData.IsTurn = false;
                        player1.EndTurn();

                        if (isSolarConnectionEnd && allSolar)
                        {
                            player1.gameData.hasSolarInNetwork = true;
                        }
                        if (isHeatConnectionEnd && allHeat)
                        {
                            player1.gameData.hasHeatInNetwork = true;
                        }
                        //int connectorCount = turnConnections.Count; Makes no sense with new connection system and made no sense even with the previous one
                        
                        //Point Modifier checks
                        if (ctr.GetLastTile())
                        {
                            if (ctr.GetLastTile().Structure.GetType().Equals(typeof(House)))
                            {
                                float multiplier = 1.0f;
                                if(ctr.Source.Structure.GetType().Equals(typeof(SpecialBuilding)))
                                    multiplier = 1.5f;

                                player1.gameData.totalPoint -= 2 * multiplier /*/ connectorCount*/; //TODO: CHANGE PLACEHOLDER VALUE
                                                                                       //Debug.LogError(connectorCount);
                                Debug.LogError(player1.gameData.totalPoint);
                            }
                            else if (ctr.GetLastTile().Structure.GetType().Equals(typeof(SpecialBuilding)))
                            {
                                ((SpecialBuilding)ctr.GetLastTile().Structure).GetSpecialBonus();
                                player1.gameData.totalPoint -= 5 /*/ conn.Connectors.Count*/; //TODO: CHANGE PLACEHOLDER VALUE
                            }
                            else if (ctr.GetLastTile().Structure.GetType().Equals(typeof(Node)))
                            {
                                //empty because no points are awarded on node connection
                            }
                            else if (ctr.GetLastTile().Structure.GetType().Equals(typeof(SolarPanel)) || ctr.GetLastTile().Structure.GetType().Equals(typeof(HeatPump)))
                            {
                                //empty because no points are awarded on Energy source connection
                            }
                        }
                        
                        player1.FinalizeConnectors(turnConnectors);
                    }
                    hasHeat = false;
                    hasSolar = false;
                    allSolar = true;
                    allHeat = true;
                    hasStandard = false;
                    isHeatConnectionEnd = false;
                    isSolarConnectionEnd = false;
                    /*currentConnection = null;
                    turnConnections = new List<Connection>();*/

                    isNormalConnectionEnd = false;
                }
                else
                {
                    player1.gameData.IsTurn = true;
                }

            }
        }
        return returnObj;
    }

    private List<Tile> visitedForScrabble;
    private bool foundScrabbleEnd;
    private Tile originalScrabble;
    private bool CheckScrabbleRewards(Tile t, bool looping = false)
    {
        if (!looping)
        {
            originalScrabble = t;
            foundScrabbleEnd = false;
            visitedForScrabble = new List<Tile>();

            if (!t.IsScrabbleForHeat && !t.IsScrabbleForSolar)
            {
                return false;
            }
        }
        if (foundScrabbleEnd)
            return true;

        visitedForScrabble.Add(t);

        if (t.Structure && (t.Structure is House or SpecialBuilding) && t != originalScrabble.Connector.Source )
        {
            foundScrabbleEnd = true;
            return true;
        }
        if (!looping && (t.IsScrabbleForHeat || t.IsScrabbleForSolar)){
            foreach (var neighbour in tileManager.getNeigbours(t))
            {
                if (neighbour.Connector && !visitedForScrabble.Contains(neighbour))
                {
                    if (CheckScrabbleRewards(neighbour, true))
                    {
                        //shouldn't be possible
                    }
                }
            }
        }
        else if(looping)
        {
            foreach (var neighbour in tileManager.getNeigbours(t))
            {
                if (neighbour.Connector && neighbour.Connector.PreviousStep != t && !visitedForScrabble.Contains(neighbour))
                {
                    if (CheckScrabbleRewards(neighbour, true))
                    {
                        if (originalScrabble.IsScrabbleForHeat)
                        {
                            player1.AddHeatPipeConnector();
                            player1.AddHeatPipeConnector();
                        }
                        else if (originalScrabble.IsScrabbleForSolar)
                        {
                            player1.AddSolarConnector();
                            player1.AddSolarConnector();
                        }
                    }
                }
            }
        }
        else
        {
            return false;
        }
        return false;
    }

    public virtual void startPoint()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            TileManager.tiles[12][16].OwnedBy = player1;
        }
    }

    public virtual void skipTurn()
    {
        clearAllSelected();
        player1.gameData.IsTurn = false;
    }

    /// <summary>
    /// Function that selects one tile when tapped
    /// </summary>
    /// <returns></returns>
    protected (bool Valid, Tile Tile, Tile Origin) chooseTile(PlayerState player, Connector _connector = null)
    {
        Tile tileTouched;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        //test
        player = player1;

        selectedConnector = _connector ?? selectedConnector;

        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject tileObjectTouched = hitInfo.collider.transform.gameObject;
            tileTouched = tileObjectTouched.GetComponent<Tile>();
            //Debug.LogError(tileTouched.X + " " + tileTouched.Y);

            if (tileTouched == null)
            {
                tileTouched = tileObjectTouched.GetComponentInParent<Tile>();
                if (tileTouched == null)
                {
                    return (false, null, null);
                }
            }

            if (!selectedConnector)
            {
                return (false, null, null);
            }

            if (selectedConnector.getLength() < 2 || (selectedConnector.GetTiles().Contains(tileTouched)) || Connector.IsValidLengthThree(selectedConnector.GetTiles()[0], selectedConnector.GetTiles()[1], tileTouched))
            {
                if (tileTouched.OwnedBy == null)
                {
                    if (selectedConnector.MaxLength == (selectedConnector.getLength() + 1) && tileManager.isOccupied(tileTouched) && !tileManager.IsHeatPipe(tileTouched) && !tileManager.IsSolarPanel(tileTouched))
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
                    tileTouched.onSelected(player);
                }
            }
            return (true, tileTouched, null);
        }
        return (false, null, null);
    }

    protected Tile GetInfoCard(PlayerState player)
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
                Debug.Log("Tile touched " + tileTouched);
                Debug.Log("Player touched " + player.name);

                tileTouched.GetSpecialOriginTile().openInfoCard(player);
            }

            return tileTouched;
        }
        return null;
    }

    protected Tile PlaceNode()
    {
        Tile tileTouched;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject tileObjectTouched = hitInfo.collider.transform.gameObject;
            tileTouched = tileObjectTouched.GetComponent<Tile>();
            Debug.LogError(tileTouched.X + " " + tileTouched.Y);

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
            tileTouched.AddStructure<Node>(nodeP);
            playerNode = null;
            return tileTouched;
        }
        return null;
    }

    public void SelectSingleConnector()
    {
        Debug.LogError("select 1");
        if (selectedConnector != null)
        {
            player1.AbortConnector(selectedConnector, false);
            selectedConnector = null;
            SelectSingleConnector();
        }
        else
        {
            selectedConnector = player1.gameData.Inventory.Find(x => x.MaxLength == 1);
            player1.gameData.Inventory.Remove(selectedConnector);
            hasStandard = true;
            Connector1Count++;
        }

        GameState.Track("SingleConnector", ("Selection", ((selectedConnector) ? "Enabled" : "Disabled")));
    }

    public void SelectDoubleConnector()
    {
        Debug.LogError("select 2");

        if (selectedConnector != null)
        {
            player1.AbortConnector(selectedConnector, false);
            selectedConnector = null;
            SelectDoubleConnector();
        }
        else
        {
            selectedConnector = player1.gameData.Inventory.Find(x => x.MaxLength == 2);
            player1.gameData.Inventory.Remove(selectedConnector);
            hasStandard = true;
            Connector2Count++;
        }

        GameState.Track("DoubleConnector", ("Selection", ((selectedConnector) ? "Enabled" : "Disabled")));
    }

    public void SelectTripleConnector()
    {
        Debug.LogError("select 3");

        if (selectedConnector != null)
        {
            player1.AbortConnector(selectedConnector, false);
            selectedConnector = null;
            SelectTripleConnector();
        }
        else
        {
            selectedConnector = player1.gameData.Inventory.Find(x => x.MaxLength == 3);
            player1.gameData.Inventory.Remove(selectedConnector);
            hasStandard = true;
            Connector3Count++;
        }

        GameState.Track("TripleConnector", ("Selection", ((selectedConnector) ? "Enabled" : "Disabled")));
    }

    public void SelectSolarConnector()
    {
        if (selectedConnector != null)
        {
            player1.AbortConnector(selectedConnector, false);
            selectedConnector = null;
            SelectSolarConnector();
        }
        else
        {
            selectedConnector = player1.gameData.SpecialConnector.Find(x => x.IsSolar);
            hasSolar = true;
            player1.gameData.SpecialConnector.Remove(selectedConnector);
        }

        GameState.Track("SolarConnector", ("Selection", ((selectedConnector) ? "Enabled" : "Disabled")));
    }

    public void SelectHeatConnector()
    {
        if (selectedConnector != null)
        {
            player1.AbortConnector(selectedConnector, false);
            selectedConnector = null;
            SelectHeatConnector();
        }
        else
        {
            selectedConnector = player1.gameData.SpecialConnector.Find(x => x.IsHeat);
            hasHeat = true;
            player1.gameData.SpecialConnector.Remove(selectedConnector);
        }

        GameState.Track("HeatConnector", ("Selection", ((selectedConnector) ? "Enabled" : "Disabled")));
    }

    public void SelectNodeConnector()
    {
        //nodeCheck = false;
        if (placingNode)
        {
            player1.gameData.nodesOwned.Add(playerNode);
            playerNode = null;
            placingNode = false;
        }
        else
        {
            if (player1.gameData.nodesOwned.Count > 0)
            {
                if(selectedConnector != null)
                {
                    player1.AbortConnector(selectedConnector, false);
                    selectedConnector = null;
                }
                playerNode = player1.gameData.nodesOwned[0];
                player1.gameData.nodesOwned.Remove(playerNode);
                placingNode = true;
            }
        }

        GameState.Track("Node", ("Selection", ((selectedConnector) ? "Enabled" : "Disabled")));
    }

    public void clearAllSelected()
    {
        GameState.Track("ClearAll");

        //player1.AbortConnection(currentConnection);
        //Debug.LogWarning(selectedConnector);
        foreach (var item in turnConnectors)
        {
            player1.AbortConnector(item, true);
        }

        if(SelectedConnector)
            player1.AbortConnector(SelectedConnector, true);

        /*foreach (Connection conn in turnConnections)
        {
            player1.AbortConnection(conn);
        }*/
    }
}
