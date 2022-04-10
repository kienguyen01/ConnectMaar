using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public struct GameStateConfig
{
    public PlayerState PlayerStateClass;
    public TileManager TileManagerClass;
}

public class GameState : MonoBehaviour
{
    public GameStateConfig config;
    public GameObject[] PlayerStarts;

    [HideInInspector]
    public List<PlayerState> playerStates;

    [HideInInspector]
    public TileManager tileManager;

    public static GameState instance { get; private set; }

    public void Awake()
    {
        Assert.IsNull(instance);

        instance = this;

        //Assert.IsNotNull(config.PlayerStateClass);
        //Assert.IsTrue(PlayerStarts.Length > 0);

        Debug.Log(config.TileManagerClass ? "true" : "false");
        if (config.TileManagerClass)
        {
            tileManager = Instantiate(config.TileManagerClass);

        }
    }
}
