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
    public int pointGranted;
    public int handSize;
    public bool hasSolarInNetwork;
    public bool hasHeatInNetwork;
    public List<Tile> tilesTaken;
    public Stack<Tile> tilesChosen;
    public List<Connection> connectionsDone;
    public List<Connector> Inventory;
    public List<Connector> SpecialConnector;
    public Connector SelectedConnector;
    public List<Node> nodeOwned;
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

        Debug.Log($"ConnectorManager - {(config.ConnectorManagerClass ? "true" : "false")}");
        if (config.ConnectorManagerClass)
        {
            connectorManager = this.gameObject.AddComponent<ConnectorManager>();
        }
        gameData.handSize = 4;
        gameData.nodeOwned.Add(this.gameObject.AddComponent<Node>());

    }

    /*private Player CreatePlayer()
    {
        Player Player = Instantiate(playerClass);
        Player.Owner = this;

        return Player;

    }*/

    /*
    private PlayerCamera CreateCamera(GameObject Target)
    {
        PlayerCamera Camera = Instantiate(cameraClass, TODO);
        Camera.Target = Target;

        return Camera;
    }*/

    private void Update()
    {

    }

    public void EndTurn()
    {
        Debug.Log("clicked end button");
        this.gameData.tilesTaken.AddRange(this.gameData.tilesChosen);
        this.gameData.tilesChosen = new Stack<Tile>();
        this.gameData.isTurn = false;
        foreach (Tile tile in this.gameData.tilesTaken)
        {
            if(!(tile.Structure.GetType() == typeof(Node)))
            {
                tile.OwnedBy = this;
            }
            tile.SelectedBy = null;
        }
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

    public void updateInventoryUI()
    {
        int firstPipeCount = 0;
        int secondPipeCount = 0;
        int thirdPipeCount = 0;
        foreach (StandardConnector item in gameData.Inventory)
        {
            if (item.GetType().Equals(typeof(StandardConnector)))
            {
                firstPipeCount++;
            }
            if (item.GetType().Equals(typeof(StandardConnector2)))
            {
                secondPipeCount++;
            }
            if (item.GetType().Equals(typeof(StandardConnector3)))
            {
                thirdPipeCount++;
            }
        }
        TMP_Text firstPipe = GameObject.Find("firstBtn(X3)").GetComponent<TMP_Text>();
        firstPipe.text = ("x" + firstPipeCount);
        TMP_Text secondPipe = GameObject.Find("secondBtn(X1)").GetComponent<TMP_Text>();
        secondPipe.text = ("x" + secondPipeCount);
        TMP_Text thirdPipe = GameObject.Find("thirdBtn(X1)").GetComponent<TMP_Text>();
        thirdPipe.text = ("x" + thirdPipeCount);
    }

    public void AddSolarConnector()
    {
        gameData.SpecialConnector.Add(this.gameObject.AddComponent<SolarPanelConnector>());
        gameData.SpecialConnector.Add(this.gameObject.AddComponent<SolarPanelConnector>());
        gameData.SpecialConnector.Add(this.gameObject.AddComponent<SolarPanelConnector>());
        gameData.SpecialConnector.Add(this.gameObject.AddComponent<SolarPanelConnector>());
        Debug.Log("---solar---");
    }

    public void AddHeatPipeConnector()
    {
        gameData.SpecialConnector.Add(this.gameObject.AddComponent<HeatPipeConnector>());
        Debug.Log("---heatpipe---");
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
        Destroy(conn);
        //gameData.connectionsDone.Remove(conn);
        return this;
    }

    public PlayerState FinalizeConnection(Connection conn)
    {
        gameData.connectionsDone.Add(conn);
        return this;
    }
}
