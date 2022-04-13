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
    public int pointGranted;
    public int handSize;
    public int numNode;
    public bool hasSolarInNetwork;
    public bool hasHeatInNetwork;
    public List<Tile> tilesTaken;
    public List<Tile> tilesChosen;
    public List<Connection> connectionsDone;
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
            connectorManager = Instantiate(config.ConnectorManagerClass);
        }
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
}
