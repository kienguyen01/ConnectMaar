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

        Debug.Log($"TileManager - {(config.TileManagerClass ? "true" : "false")}");
        Debug.Log($"PlayerState in GameState - {(config.PlayerStateClass ? "true" : "false")}");

        if (config.TileManagerClass)
        {
            tileManager = Instantiate(config.TileManagerClass);
        }

        createPlayer();
    }

    private void createPlayer()
    {
        playerStates.Add(Instantiate(config.PlayerStateClass));
        playerStates.Add(Instantiate(config.PlayerStateClass));
    }

    //only for local player => should not be updated
    //coroutine for a local player instead when playerTurn == true
    //as soon as the turn end => kill coroutine
    //listener in playerstate to determine the turn
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            chooseTile();
        }
    }


    /// <summary>
    /// Function that selects one tile when tapped
    /// </summary>
    /// <returns></returns>
    //passing parameter of playerstate
    Tile chooseTile()
    {
        Tile tileTouched;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        RaycastHit hitInfo;


        if (Physics.Raycast(ray, out hitInfo))
        {

            GameObject tileObjectTouched = hitInfo.collider.transform.gameObject;
            tileTouched = tileObjectTouched.GetComponent<Tile>();

            tileTouched.onSelected(playerStates[0]);

            playerStates[0].gameData.tilesChosen.Add(tileTouched);
            return tileTouched;
        }

        return null;
    }
}
