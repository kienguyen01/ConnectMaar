using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

[System.Serializable]
public struct ConnectorConfig
{
    public ConnectorManager ConnectorManagerClass;
}

[System.Serializable]
public struct PlayerInfo
{
    public string name;
    public float points;
}

[System.Serializable]
public class PlayerGameData
{
    public bool isTurn;
    public float totalPoint;
    public int handSize;
    public bool hasSolarInNetwork;
    public bool hasHeatInNetwork;
    public List<Tile> tilesTaken;
    public Stack<Tile> tilesChosen;
    public List<Connection> connectionsDone;
    public List<Connector> Inventory;
    public List<Connector> SpecialConnector;
    public Connector SelectedConnector;
    public List<Node> nodesOwned;
    public Color PlayerColour;
}

public class PlayerState : MonoBehaviour
{
    public ConnectorConfig config;
    public Player playerClass;
    public PlayerCamera cameraClass;

    

    [HideInInspector]
    public PlayerGameData gameData;
    
    //public string name;
    
    private PlayerCamera pCamera;

    [HideInInspector]
    public ConnectorManager connectorManager;

    [HideInInspector]
    public float points = 0;
    public UnityAction<float> OnPointGot;

    public void Awake()
    {
        gameData.isTurn = false;
        gameData.tilesChosen = new Stack<Tile>();
        //Assert.IsNotNull(playerClass);
        //Assert.IsNotNull(cameraClass);

        //player = CreatePlayer(); 
        //camera = CreateCamera(player.gameObject);

        //player = CreatePlayer();
        //camera = CreateCamera(player.gameObject);

        //Assert.IsNotNull(config.PlayerStateClass);
        //Assert.IsTrue(PlayerStarts.Length > 0);

        gameData.PlayerColour = Color.red;

        Debug.Log($"ConnectorManager - {(config.ConnectorManagerClass ? "true" : "false")}");
        if (config.ConnectorManagerClass)
        {
            connectorManager = this.gameObject.AddComponent<ConnectorManager>();
        }
        gameData.handSize = 4;
        gameData.nodesOwned.Add(this.gameObject.AddComponent<Node>());
        gameData.nodesOwned.Add(this.gameObject.AddComponent<Node>());
        gameData.totalPoint = 50;
    }
    private void Update()
    {

    }

    public void EndTurn()
    {
        Debug.Log("clicked end button");
        this.gameData.isTurn = false;
        foreach (Tile tile in this.gameData.tilesChosen)
        {
            if(!(tile.Structure.GetType() == typeof(Node)))
            {
                tile.OwnedBy = this;
            }
            tile.SelectedBy = null;
            if (tile.IsScrambleForHeat)
            {
                this.AddHeatPipeConnector()
                    .AddHeatPipeConnector()
                    .AddHeatPipeConnector()
                    .AddHeatPipeConnector();
            }
            else if (tile.IsScrambleForSolar)
            {
                this
                    .AddSolarConnector()
                    .AddSolarConnector()
                    .AddSolarConnector()
                    .AddSolarConnector();
            }
        }
        this.gameData.tilesTaken.AddRange(this.gameData.tilesChosen);
        this.gameData.tilesChosen = new Stack<Tile>();
    }


    public void RefillHand()
    {
        for (int i = gameData.Inventory.Count; i < gameData.handSize; i++)
        {
            switch (Random.Range(1, 4))
            {
                case 1:
                    gameData.Inventory.Add(this.gameObject.AddComponent<StandardConnector>());
                    Debug.Log("---1---");
                    break;
                case 2:
                    gameData.Inventory.Add(this.gameObject.AddComponent<StandardConnector2>());
                    Debug.Log("---2---");
                    break;
                case 3:
                    gameData.Inventory.Add(this.gameObject.AddComponent<StandardConnector3>());
                    Debug.Log("---3---");
                    break;
                default: break;
            }
        }
        updateInventoryUI();
    }
    
    public void refilSpecificHand(int one,int two, int three)
    {
        if (gameData.Inventory.Count < gameData.handSize)
        {
            for (int i = 0; i < one; i++)
            {
                gameData.Inventory.Add(this.gameObject.AddComponent<StandardConnector>());
            }
            for (int i = 0; i < two; i++)
            {
                gameData.Inventory.Add(this.gameObject.AddComponent<StandardConnector2>());
            }
            for (int i = 0; i < three; i++)
            {
                gameData.Inventory.Add(this.gameObject.AddComponent<StandardConnector3>());
            }
        }


        updateInventoryUI();

    }

    public void clearHand()
    {
        gameData.Inventory.RemoveAll(x => x.MaxLength == 1);
        gameData.Inventory.RemoveAll(x => x.MaxLength == 2);
        gameData.Inventory.RemoveAll(x => x.MaxLength == 3);

        updateInventoryUI();

    }

    public void updateInventoryUI()
    {
        int firstPipeCount = 0;
        int secondPipeCount = 0;
        int thirdPipeCount = 0;
        int solarCount = 0;
        int heatCount = 0;
        int nodeCount = 0;
        foreach (StandardConnector item in gameData.Inventory)
        {
            if (item.MaxLength == 1)
            {
                firstPipeCount++;
            }
            if (item.MaxLength == 2)
            {
                secondPipeCount++;
            }
            if (item.MaxLength == 3)
            {
                thirdPipeCount++;
            }
        }

        foreach (Connector x in gameData.SpecialConnector)
        {
            if (x.IsHeat)
            {
                heatCount++;
            }
            if (x.IsSolar)
            {
                solarCount++;   
            }
        }
        foreach (Node n in gameData.nodesOwned)
        {
            nodeCount++;
        }
        TMP_Text firstPipe = GameObject.Find("firstBtn(X3)").GetComponent<TMP_Text>();
        firstPipe.text = ("x" + firstPipeCount);
        TMP_Text secondPipe = GameObject.Find("secondBtn(X1)").GetComponent<TMP_Text>();
        secondPipe.text = ("x" + secondPipeCount);
        TMP_Text thirdPipe = GameObject.Find("thirdBtn(X1)").GetComponent<TMP_Text>();
        thirdPipe.text = ("x" + thirdPipeCount);
        TMP_Text heatPipe = GameObject.Find("fifthBtn(X3)").GetComponent<TMP_Text>();
        heatPipe.text = ("x" + heatCount);
        TMP_Text solarPipe = GameObject.Find("fourthBtn(X1)").GetComponent<TMP_Text>();
        solarPipe.text = ("x" + solarCount);
        TMP_Text node = GameObject.Find("sixthBtn(X2)").GetComponent<TMP_Text>();
        node.text = ("x" + nodeCount);
    }

    public PlayerState AddSolarConnector()
    {
        gameData.SpecialConnector.Add(this.gameObject.AddComponent<SolarPanelConnector>());
        Debug.Log("---solar---");
        return this;
    }

    public PlayerState AddHeatPipeConnector()
    {
        gameData.SpecialConnector.Add(this.gameObject.AddComponent<HeatPipeConnector>());
        Debug.Log("---heatpipe---");
        return this;
    }

    public Connection StartConnection()
    {
        Connection c = new Connection();
        //Connection c = ;
        //gameData.connectionsDone.Add(c);
        return c;
    }

    public PlayerState AbortConnection(Connection conn)
    {
        foreach (Connector c in conn.Connectors)
        {
            AbortConnector(c, true);
        }
        Destroy(conn);


        return this;
    }

    public PlayerState AbortConnector(Connector c, bool Hard = false)
    {
        if (Hard || c.MaxLength > c.getLength())
        {
            foreach (Tile t in c.GetTiles())
            {
                gameData.tilesChosen.Pop();
            }

            if (!c.IsSpecial)
                this.gameData.Inventory.Add(c.ResetConnector());
            else
                this.gameData.SpecialConnector.Add(c.ResetConnector());
        }

        return this;
    }

    public PlayerState FinalizeConnection(Connection conn)
    {
        gameData.connectionsDone.Add(conn);
        return this;
    }
}
