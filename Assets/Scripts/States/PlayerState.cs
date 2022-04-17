using System.Collections;
using System.Collections.Generic;
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
    public int numNode;
    public bool hasSolarInNetwork;
    public bool hasHeatInNetwork;
    public List<Tile> tilesTaken;
    public Stack<Tile> tilesChosen;
    public List<Connection> connectionsDone;
    public List<Connector> Inventory;
    public Connector SelectedConnector;
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

        //Assert.IsNotNull(config.PlayerStateClass);
        //Assert.IsTrue(PlayerStarts.Length > 0);

        Debug.Log($"ConnectorManager - {(config.ConnectorManagerClass ? "true" : "false")}");
        if (config.ConnectorManagerClass)
        {
            connectorManager = Instantiate(config.ConnectorManagerClass);
        }
        gameData.handSize = 4;

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
            tile.OwnedBy = this;
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
    }

    public Connection StartConnection()
    {
        Connection c = (Connection)ScriptableObject.CreateInstance("Connection");
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
