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



    bool onePressed = false;
    bool twoPressed = false;
    bool threePressed = false;

    int numOfPress = 0;
    private void createPlayer()
    {
        playerStates.Add(Instantiate(config.PlayerStateClass));
        playerStates.Add(Instantiate(config.PlayerStateClass));
    }

    private void Update()
    {




        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (playerStates[0].gameData.isTurn)
            {
                playerStates[0].RefillHand();
                playerStates[0].gameData.isTurn = false;
            }
            else
            {
                playerStates[0].gameData.isTurn = true;
            }
            Debug.Log("isTurn" + playerStates[0].gameData.isTurn.ToString());
        }
        if (playerStates[0].gameData.isTurn)
        {

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                onePressed = !onePressed;
                twoPressed = false;
                threePressed = false;
                Debug.Log("1 pressed " + onePressed);
                numOfPress = 1;
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                onePressed = false;
                twoPressed = true;
                threePressed = false;
                Debug.Log("2 pressed " + twoPressed);
                numOfPress = 2;

            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                onePressed = false;
                twoPressed = false;
                threePressed = true;
                Debug.Log("2 pressed " + threePressed);
                numOfPress = 1;
            }

            for (int i = 0; i < numOfPress; i++)
            {
                if ((onePressed || twoPressed || threePressed) && Input.GetMouseButtonDown(0))
                {
                    chooseTile();
                }

            }
            //pressed = false;
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            playerStates[0].EndTurn();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            tileManager.tiles[12][16].OwnedBy = playerStates[0];
        }
        
    }

    void selectConnector(int Length)
    {
        //switch (Length)
        //{
        //    case 1:
        //        chooseTile(1);
        //        break;
        //    case 2:
        //        chooseTile(2);
        //        break;
        //    case 3:
        //        chooseTile(3);
        //        break;
        //    default: break;
        //}
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

            if (!tileManager.isOccupied(tileTouched))
                tileTouched.onSelected(playerStates[0]);
            return tileTouched;
        }
        
       

        return null;
    }
}
