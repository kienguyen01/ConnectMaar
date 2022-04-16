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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("isTurn" + playerStates[0].gameData.isTurn.ToString());
            if (playerStates[0].gameData.isTurn)
            {
                playerStates[0].gameData.isTurn = false;
            }
            else
            {
                playerStates[0].gameData.isTurn = true;
            }
        }
        if (playerStates[0].gameData.isTurn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                chooseTile();
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            tileManager.endButtonOnClick(playerStates[0]);
        }
    }


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

            if(!tileManager.isOccupied(tileTouched))
                tileTouched.onSelected(playerStates[0]);
            return tileTouched;
        }

        return null;
    }
}
