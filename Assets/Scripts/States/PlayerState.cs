using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using Photon.Realtime;
using System.Reflection;
using System.Linq;
using System;
using System.Runtime.Serialization;
using mixpanel;

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
    private bool isTurn;
    public float totalPoint;
    public int handSize;
    public bool hasSolarInNetwork;
    public bool hasHeatInNetwork;
    public List<Tile> tilesTaken;
    public List<Tile> tilesChosen;
    //public List<Connection> connectionsDone;
    public List<Connector> ConnectorsPlaced;
    public List<Connector> Inventory;
    public List<Connector> SpecialConnector;
    public Connector SelectedConnector;
    public List<Node> nodesOwned;
    public Color PlayerColour;
    public List<Tile> StructuresClaimed;
    public bool HasPassiveIncome;
    public float PassiveIncome;

    public bool IsTurn { get => isTurn;
        set
        {
            if (value)
            {
                GameState.Track("TurnStart", ("Connections", StructuresClaimed.Count), ("Points", totalPoint), ("Tiles", tilesTaken));
            }
            else
            {
                GameState.Track("TurnEnd", ("Connections", StructuresClaimed.Count), ("Points", totalPoint), ("Tiles", tilesTaken));
            }
            isTurn = value;
        }
    }
}

public class PlayerState : MonoBehaviourPun
{
    public ConnectorConfig config;
    public Player playerClass;
    public PlayerCamera cameraClass;
    public bool IsTutorial;
    

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
        gameData.IsTurn = false;
        gameData.tilesChosen = new List<Tile>();
        gameData.HasPassiveIncome = false;
        gameData.PassiveIncome = 0.0f;
        //Assert.IsNotNull(playerClass);
        //Assert.IsNotNull(cameraClass);

        //player = CreatePlayer(); 
        //camera = CreateCamera(player.gameObject);

        //player = CreatePlayer();
        //camera = CreateCamera(player.gameObject);

        //Assert.IsNotNull(config.PlayerStateClass);
        //Assert.IsTrue(PlayerStarts.Length > 0);

        //gameData.PlayerColour = Color.red;

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

    public virtual void EndTurn()
    {
        if (this.gameData.HasPassiveIncome)
        {
            this.gameData.totalPoint -= this.gameData.PassiveIncome;
        }
        Debug.Log("clicked end button");
        this.gameData.IsTurn = false;
        EndTurnCheck();
        this.gameData.tilesChosen = new List<Tile>();
    }

    public void EndTurnCheck()
    {
        foreach (Tile tile in this.gameData.tilesChosen)
        {
            if (!(tile.Structure.GetType() == typeof(Node)))
            {
                tile.OwnedBy = this;
            }
            tile.SelectedBy = null;
            //AssignScrabbleTileRewards(tile);
        }
        this.gameData.tilesTaken.AddRange(this.gameData.tilesChosen);
    }

/*    public void AssignScrabbleTileRewards(Tile tile)
    {
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
    }*/




    public PlayerState RefillHand()
    {
        if(this.IsTutorial == true)
        {
            return this;
        }
        for (int i = gameData.Inventory.Count; i < gameData.handSize; i++)
        {
            switch (UnityEngine.Random.Range(1, 4))
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



        return this;
    }
    
    public PlayerState refilSpecificHand(int one,int two, int three)
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

        return this;
    }

    public PlayerState clearHand()
    {
        gameData.Inventory.RemoveAll(x => x.MaxLength == 1);
        gameData.Inventory.RemoveAll(x => x.MaxLength == 2);
        gameData.Inventory.RemoveAll(x => x.MaxLength == 3);

        updateInventoryUI();

        return this;
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

    /*public Connection StartConnection()
    {
        Connection c = new Connection();
        c.Connectors = new List<Connector>();
        //Connection c = ;
        //gameData.connectionsDone.Add(c);
        return c;
    }*/

    /*public PlayerState AbortConnection(Connection conn)
    {
        foreach (Connector c in conn.Connectors)
        {
            AbortConnector(c, true);
        }
        Destroy(conn);

        return this;
    }*/

    public PlayerState AbortConnector(Connector c, bool Hard = false)
    {
        if (!c)
            return null;

        if (Hard || c.MaxLength > c.getLength())
        {
            foreach (Tile t in c.GetTiles())
            {
                if(t.Connector.PreviousStep && t.Connector.PreviousStep.Connector)
                    t.Connector.PreviousStep.Connector.UsedForConnector = false;
                t.SelectedBy = null;
                t.Connector.PreviousStep = null;
                t.Connector.Source = null;
                t.Connector = null;
                gameData.tilesChosen.Remove(t);
            }
            if (!c.IsSpecial)
                this.gameData.Inventory.Add(c.ResetConnector());
            else
                this.gameData.SpecialConnector.Add(c.ResetConnector());
        }
        return this;
    }

    /*public PlayerState FinalizeConnection(Connection conn)
    {
        foreach (Connector c in conn.Connectors)
        {
            foreach (Tile t in c.GetTiles())
            {
                AssignScrabbleTileRewards(t);
            }
            
        }

        gameData.connectionsDone.Add(conn);
        return this;
    }*/

    public PlayerState FinalizeConnectors(List<Connector> connectors)
    {
        foreach (Connector c in connectors)
        {
            foreach (Tile t in c.GetTiles())
            {
                AssignScrabbleTileRewards(t);//TODO IF CAN FIND NON-SOURCE BUILDING CONNECTED via recursion
            }

            gameData.ConnectorsPlaced.Add(c);
        }
        return this;
    }

    public void AssignScrabbleTileRewards(Tile tile)
    {
        if (tile.IsScrabbleForHeat)
        {
            this
                .AddHeatPipeConnector()
                .AddHeatPipeConnector();

        }
        else if (tile.IsScrabbleForSolar)
        {
            this
                .AddSolarConnector()
                .AddSolarConnector();
        }
    }
}
