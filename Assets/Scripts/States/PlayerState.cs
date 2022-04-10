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
    public List<Tile> tilesTaken;

}


public class PlayerState : MonoBehaviour
{

    public ConnectorConfig config;
    public Player playerClass;
    public PlayerCamera cameraClass;

    public string name;
    
    private Player player;
    private PlayerCamera camera;

    [HideInInspector]
    public ConnectorManager connectorManager;

    [HideInInspector]
    public float points = 0;
    public UnityAction<float> OnPointGot;

    public void Awake()
    {
        Assert.IsNotNull(playerClass);
        Assert.IsNotNull(cameraClass);

        //player = CreatePlayer();
        //camera = CreateCamera(player.gameObject);

        //Assert.IsNotNull(config.PlayerStateClass);
        //Assert.IsTrue(PlayerStarts.Length > 0);

        Debug.Log(config.ConnectorManagerClass ? "true" : "false");
        if (config.ConnectorManagerClass)
        {
            connectorManager = Instantiate(config.ConnectorManagerClass);
        }
    }

    private Player CreatePlayer()
    {
        Player Player = Instantiate(playerClass);
        Player.Owner = this;

        return Player;
    }
    private PlayerCamera CreateCamera(GameObject Target)
    {
        PlayerCamera Camera = Instantiate(cameraClass, player.gameObject.transform);
        Camera.Target = Target;

        return Camera;
    }

    private void Update()
    {

    }
}
