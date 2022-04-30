using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public struct TutorialStateConfig
{
    public PlayerState PlayerStateClass;
    public TileManager TileManagerClass;
}



public class TutorialState : GameState
{
    private Image tutBox;

    public GameObject popups;
    private int index;

    private string[] tutorialMessage;

    [HideInInspector]
    public TextMeshProUGUI textTitle;


    private bool go;
    private bool autoAdvance;
    private int temporaryRoutines;

    new private void Awake()
    {
        onAwake();
    }

    private void Start()
    {
        Debug.Log("STARTTUTORIAL");

        if (TileManager.tiles[0][0].OwnedBy == null)
        {
            TileManager.tiles[0][0].OwnedBy = playerStates[0];
        }

        tutBox = GameObject.Find("TutorialBox").GetComponent<Image>();

        if (tutBox.enabled == true)
        {
            text = GameObject.Find("txtBody").GetComponent<TextMeshProUGUI>();
            textTitle = GameObject.Find("txtTitle").GetComponent<TextMeshProUGUI>();
            textTitle.text = "Tutorial:";
        }

        StartCoroutine(TutorialRoutine());
    }

    IEnumerator TutorialRoutine()
    {
        changeTutorialMessage();
        go = true;
        autoAdvance = false;
        temporaryRoutines = 0;
        while (true)
        {
            if (go && (autoAdvance || nextMsg() ))
            {
                autoAdvance = false;
                index++;
                changeTutorialMessage();
            }

            if (index == 49)
            {
                DisablePopup();
                yield break;
            }
            yield return 0;
        }
    }

    private void changeTutorialMessage()
    {
        if (text)
        {
            switch (index)//TODO Ideally, move every text into an array and just change with index value
            {
                case 0:
                    {
                        GameObject varGameObject = GameObject.Find("Plane");
                        varGameObject.GetComponent<PhoneCameraMovement>().enabled = false;
                        text.text = "Welcome to the Tutorial! In this tutorial we will teach you how to play ConnectMaar. Tap on Screen to start.";
                        playerStates[0].clearHand();
                        playerStates[0].refilSpecificHand(1, 1, 2);
                        break;
                    }
                case 1:
                    text.text = "This is a tutorial to show you the basic gameplay of our game: ConnectMaar. Tap to continue.";
                    break;
                case 2:
                    text.text = "On the left side of your screen you can see your inventory.";
                    break;
                case 3:
                    text.text = "In your inventory you have Items called Connectors. They are your main tool to make Alkmaar a greener and more sustainable place";
                    break;
                case 4:
                    text.text = "Right now in your inventory you have a single connector, a double connector and a triple connector";
                    break;
                case 5:
                    text.text = "By connecting buildings to renewable energy sources in the most efficient way possible, you'll reduce the emission level at the top of the screen!";
                    break;
                case 6:
                    text.text = "Let's begin using renewable energy to make Alkmaar a Greener city!";
                    break;
                case 7:
                    text.text = "Start your turn by pressing the start turn button.";
                    StartCoroutine(TurnCheckRoutine());
                    break;
                case 8:
                    text.text = "Click on the length-3 connector on the left side of the screen and then place it on the grid starting from your wind turbine to the nearest house";
                    break;
                case 9:
                    DisablePopup();
                    StartCoroutine(TileCheckRoutine(2, 1));
                    break;
                case 10:
                    EnablePopup();
                    text.text = "When making connections your emission bar will decrease. The more connectors you used, the less efficient the connection will be";
                    break;
                case 11:
                    text.text = "Congratulations, you have started to make Alkmaar a greener city!";
                    playerStates[0].clearHand();
                    break;
                case 12:
                    text.text = "Would you look at that, Your emission bar has descreased by 3 points";
                    break;
                case 13:
                    text.text = "1 connector = 3 points,  2 Connectors =  2 points,  3 Connectors = 1 point";
                    break;
                case 14:
                    playerStates[0].refilSpecificHand(1, 1, 2);
                    text.text = "At the end of each turn your inventory of connectors will be filled up again";
                    break;
                case 15:
                    text.text = "Start your turn again to continue";
                    StartCoroutine(TurnCheckRoutine());
                    break;
                case 16:
                    text.text = "Use the newly acquired connectors to make a connection to the house nearby using one 3 connector and a single connector";
                    break;
                case 17:
                    DisablePopup();
                    StartCoroutine(TileCheckRoutine(6, 2));
                    break;
                case 18:
                    EnablePopup();
                    text.text = "Your emission levels have further reduced by 2 points. This is because you used two connections instead of a single one";
                    break;
                case 19:
                    text.text = "It looks like you built over special tiles and gained " + playerStates[0].gameData.SpecialConnector.Count + " special connectors";
                    playerStates[0].clearHand();
                    break;
                case 20:
                    text.text = "Special connectors are used to connect to special renewable resource in your grid such as heatpumps and solar panels. This will be useful in a bit.";
                    break;
                case 21:
                    text.text = "Since you just connected to a solar tile, the special connector you have can only be used to connect to a solar pannel.";
                    break;
                case 22:
                    text.text = "Special connectors like these can only be placed on one grid one at a time";
                    break;
                case 23:
                    text.text = "Please use the special solar connector to connect to the solar panel ahead.";
                    break;
                case 24:
                    {
                        startAiMoves();
                        playerStates[1].gameData.totalPoint = 47;
                        DisablePopup();

                        StartCoroutine(SolarHeatCheckRoutine());
                        break;
                    }
                case 25:
                    EnablePopup();
                    text.text = "You have now added a solar pannel to your connection. Congratulations! this will be useful later";
                    playerStates[0].refilSpecificHand(0, 2, 2);
                    break;
                case 26:
                    text.text = "First, please tap on the stadium at the bottom of the map";
                    //@TODO ADD check if stadium was opened and closed
                    break;
                case 27:
                    text.text = "That was the AFAS Stadium";
                    break;
                case 28:
                    text.text = "That was a Key Location info-Card. Each card has some fun facts about the location on the left hand side.";
                    break;
                case 29:
                    text.text = "The right hand side shows what renewable building you need to have in your grid in order to claim this stadium and rewards you get";
                    break;
                case 30:
                    text.text = "For AFAS stadium to be connected to our grid by you need a Solar pannel in your grid. As a reward, you'll get an extra double connector next turn";
                    break;
                case 31:
                    text.text = "Start your turn";
                    StartCoroutine(TurnCheckRoutine());
                    playerStates[0].clearHand();
                    playerStates[0].refilSpecificHand(1, 1, 2);
                    break;
                case 32:
                    text.text = "Make a connection from a building in your grid to the stadium";
                    break;
                case 33:
                    addTile(3, 13);
                    addTile(4, 13);
                    playerStates[1].gameData.totalPoint = 45;
                    DisablePopup();
                    StartCoroutine(TileCheckRoutine(1, 7));
                    break;
                case 34:
                    playerStates[0].gameData.totalPoint = 35;
                    EnablePopup();
                    text.text = "The AFAS stadium is now using sustainable energy. Congratulations!";
                    break;
                case 35:
                    text.text = "You are now supplying renewable energy to AFAS Stadium and would you look at that, the emission bar has decreased drastically because of that.";
                    playerStates[0].clearHand();
                    playerStates[0].refilSpecificHand(4, 0, 0);
                    break;
                case 36:
                    text.text = "It looks like the second player has been making moves around Alkmaar while you were busy";
                    break;
                case 37:
                    text.text = "Lets claim that house above shall we?";
                    break;
                case 38:
                    text.text = "Start your turn and build as close as you can to that house";
                    break;
                case 39:
                    {
                        addTile(5, 12);
                        addTile(5, 11);
                        addTile(6, 10);
                        playerStates[1].gameData.totalPoint = 42;
                        DisablePopup();

                        StartCoroutine(TilesChosenCheckRoutine(3));
                    }
                    break;
                case 40:
                    EnablePopup();
                    text.text = "looks like you dont have enough connectors to reach the house. Let's use the last item in an inventory, this is called a node. Place it down at the end of the tile";
                    break;
                case 41:
                    DisablePopup();
                    StartCoroutine(NodesOwnedCheckRoutine(0));
                    break;
                case 42:
                    EnablePopup();
                    text.text = "Nodes allow you to end your connection and continue the next turn";
                    break;
                case 43:
                    text.text = "But be careful you are only granted one per game and it comes as a cost";
                    break;
                case 44:
                    text.text = "Other players can also use your node to end their connection midway as well and continue their turn, So you'll have to be strategic about their placement";
                    break;
                case 45:
                    text.text = "Now place use your last connector to place over the node to end your turn ";
                    break;
                case 46:
                    text.text = "Connect to a house closest to you starting from your node to complete the tutorial";
                    break;
                case 47:
                    {
                        addTile(6, 9);
                        addTile(7, 9);
                        addTile(8, 8);
                        playerStates[1].gameData.totalPoint = 42;
                        DisablePopup();
                        StartCoroutine(TileCheckRoutine(8, 5));
                        StartCoroutine(TileCheckRoutine(8, 8));
                        StartCoroutine(TileCheckRoutine(13, 10));
                        StartCoroutine(TileCheckRoutine(6, 10));
                        break;
                    }
                case 48:
                    EnablePopup();
                    text.text = "Congratulations on finsihing the tutorial! Continue making Alkmaar a Greener place!";
                    break;
            }
        }
    }

    private IEnumerator TurnCheckRoutine(bool YourTurn = true, bool AutoAdvance = true)
    {
        go = false;
        while (true)
        {
            if (playerStates[0].gameData.isTurn == YourTurn)
            {
                go = true;
                autoAdvance = AutoAdvance;
                yield break;
            }
            yield return 0;
        }
    }

    private IEnumerator TileCheckRoutine(int x, int y)
    {
        go = false;
        temporaryRoutines++;
        while (true)
        {
            if (TileManager.tiles[x][y].OwnedBy == playerStates[0])
            {
                playerStates[0].gameData.totalPoint = playerStates[0].gameData.totalPoint - 3;
                if(temporaryRoutines <= 1)
                {
                    go = true;
                    autoAdvance = true;
                }
                temporaryRoutines--;
                yield break;
            }
            yield return 0;
        }
    }

    private IEnumerator SolarHeatCheckRoutine(bool solar = true)
    {
        go = false;
        while (true)
        {
            if ((solar) ? playerStates[0].gameData.hasSolarInNetwork : playerStates[0].gameData.hasHeatInNetwork)
            {
                go = true;
                autoAdvance = true;
                yield break;
            }
            yield return 0;
        }
    }

    private IEnumerator TilesChosenCheckRoutine(int count)
    {
        go = false;
        while (true)
        {
            if (playerStates[0].gameData.tilesChosen.Count == count)
            {
                go = true;
                autoAdvance = true;
                yield break;
            }
            yield return 0;
        }
    }

    private IEnumerator NodesOwnedCheckRoutine(int count)
    {
        go = false;
        while (true)
        {
            if (playerStates[0].gameData.nodesOwned.Count == count)
            {
                go = true;
                autoAdvance = true;
                yield break;
            }
            yield return 0;
        }
    }

    public void DisablePopup()
    {
        text.enabled = false;
        tutBox.enabled = false;
        textTitle.enabled = false;
        GameObject varGameObject = GameObject.Find("Plane");
        varGameObject.GetComponent<PhoneCameraMovement>().enabled = true;
    }

    public void EnablePopup()
    {
        text.enabled = true;
        tutBox.enabled = true;
        textTitle.enabled = true;
        textTitle.text = "Tutorial:";
        GameObject varGameObject = GameObject.Find("Plane");
        varGameObject.GetComponent<PhoneCameraMovement>().enabled = false;
    }

    public void startAiMoves()
    {
        /*Make ai have connectionsns to the two houses closed to him */
        playerStates.Add(Instantiate(config.PlayerStateClass));
        playerStates[1].gameData.PlayerColour = Color.black;
        addTile(0, 14);
        addTile(0, 13);
        addTile(1, 13);
        addTile(2, 13);
    }

    private void addTile(int x, int y)
    {
        playerStates[1].gameData.tilesChosen.Push(TileManager.tiles[x][y]);
        TileManager.tiles[x][y].OwnedBy = playerStates[1];
    }

    private bool nextMsg()
    {
        if (Input.GetMouseButtonDown(0) && (EventSystem.current.IsPointerOverGameObject()))
        {
            return true;
        }
        return false;
    }

    private void checkTileTaken(int x, int y)
    {
        if (TileManager.tiles[x][y].OwnedBy == playerStates[0])
        {
            playerStates[0].gameData.totalPoint = playerStates[0].gameData.totalPoint - 3;
            go = true;
        }
    }
}