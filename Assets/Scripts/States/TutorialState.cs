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
    public TutorialManager TutorialManagerClass;
    private TutorialManager TutorialManager;
    private Image tutBox;

    public GameObject popups;
    private int index;
    bool onetime = false;

    private string[] tutorialMessage;

    [HideInInspector]
    public TextMeshProUGUI textTitle;


    public override void startPoint()
    {
        if (TileManager.tiles[0][0].OwnedBy == null)
        {
            TileManager.tiles[0][0].OwnedBy = playerStates[0];
        } 
    }



    public override void TutorialStart()
    {
        tutBox = GameObject.Find("TutorialBox").GetComponent<Image>();

        if (tutBox.enabled == true)
        {
            text = GameObject.Find("txtBody").GetComponent<TextMeshProUGUI>();
            textTitle = GameObject.Find("txtTitle").GetComponent<TextMeshProUGUI>();
            textTitle.text = "Tutorial:";

        }

        if (text != null)
        {
            if (index == 0)
            {
                


                GameObject varGameObject = GameObject.Find("Plane");
                varGameObject.GetComponent<PhoneCameraMovement>().enabled = false;
                text.text = "Welcome to the Tutorial! In this tutorial we will teach you how to play ConnectMaar. Tap on Screen to start.";
                playerStates[0].clearHand();
                playerStates[0].refilSpecificHand(1, 1, 2);
                nextMsg();
            }
            else if (index == 1)
            {
                text.text = "This is a tutorial to show you the basic gameplay of our game: ConnectMaar. Tap to continue.";
                nextMsg();
            }
            else if (index == 2)
            {
                text.text = "On the left side of your screen you can see your inventory.";
                nextMsg();
            }
            else if (index == 3)
            {

                text.text = "In your inventory you have Items called Connectors. They are your main tool to make Alkmaar a greener and more sustainable place";
                nextMsg();
            }
            else if (index == 4)
            {
                text.text = "Right now in your inventory you have a single connector, a double connector and a triple connector";
                nextMsg();

            }
            else if (index == 5)
            {
                text.text = "By connecting buildings to renewable energy sources in the most efficient way possible, you'll reduce the emission level at the top of the screen!";
                nextMsg();
            }
            else if (index == 6)
            {
                text.text = "Let's begin using renewable energy to make Alkmaar a Greener city!";
                nextMsg();
            }
            else if (index == 7)
            {
                text.text = "Start your turn by pressing the start turn button.";
                if (playerStates[0].gameData.isTurn == true)
                {
                    index++;
                }
            }
            else if (index == 8)
            {
                text.text = "Click on the 3 length connector on the left side of the screen and then place them one at a time on the grid starting from your windturbine";
                nextMsg();

            }
            else if (index == 9)
            {
                DisablePopup();
                checkTileTaken(2, 1);

            }
            else if (index == 10)
            {
                EnablePopup();
                text.text = "When making connections your emission bar will decrease. The more connectors you used, the less efficient the connection will be";
                //playerStates[0].gameData.totalPoint = 47;
                nextMsg();
            }
            else if (index == 11)
            {
                text.text = "Congratulations, you have started to make Alkmaar a greener city!";
                playerStates[0].clearHand();
                playerStates[0].refilSpecificHand(0, 2, 2);
                nextMsg();
            }
            else if (index == 12)
            {
                text.text = "Will you look at that. Your emission bar has descreased by 3 points";
                nextMsg();
            }
            else if (index == 13)
            {
                text.text = "1 connector = 3 points,  2 Connectors =  2 points,  3 Connectors = 1 point";
                nextMsg();
            }
            else if (index == 14)
            {
                text.text = "At the end of each turn your inventory of connectors will be filled up again";
                nextMsg();
            }
            else if (index == 15)
            {
                text.text = "Start your turn again to continue";
                if (playerStates[0].gameData.isTurn == true)
                {
                    index++;
                }
            }
            else if (index == 16)
            {
                text.text = "Use the newly acquired connectors to make a connection to the house nearby using one 3 connector and a single connector";
                nextMsg();
            }
            else if (index == 17)
            {
                DisablePopup();
                checkTileTaken(6, 2);

            }
            else if (index == 18)
            {
                EnablePopup();
                //playerStates[0].gameData.totalPoint = 45;
                text.text = "Your emission levels have further reduced by 2 points. This is because you used two connections instaed of a single one";
                nextMsg();
            }
            else if (index == 19)
            {
                text.text = "It looks like you built over special tiles and gained " + playerStates[0].gameData.SpecialConnector.Count + " special connectors";
                playerStates[0].clearHand();
                nextMsg();
            }
            else if (index == 20)
            {
                text.text = "Special connectors are used to connect to special renewable resource in your grid such as heatpumps and solar panels. This will be useful in a bit.";
                nextMsg();
            }
            else if (index == 21)
            {
                text.text = "Since you just connected to a solar tile, the special connector you have can only be used to connect to a solar pannel.";
                nextMsg();
            }
            else if (index == 22)
            {
                text.text = "Special connectors like these can only be placed on one grid one at a time";
                nextMsg();
            }
            else if (index == 23)
            {
                text.text = "Please use the special solar connector to connect to the solar panel ahead.";
                nextMsg();
            }
            else if (index == 24)
            {
                if (!onetime)
                {
                    startAiMoves();
                    onetime = true;
                    nextMsg();
                }
                playerStates[1].gameData.totalPoint = 47;
                DisablePopup();
                if (playerStates[0].gameData.hasSolarInNetwork)
                {
                    index++;
                }
            }
            else if (index == 25)
            {
                EnablePopup();
                text.text = "You have now added a solar pannel to your connection. Congratulations! this will be useful later";
                playerStates[0].refilSpecificHand(0, 2, 2);
                nextMsg();
            }

            else if (index == 26)
            {
                text.text = "Lets put that solar panel to good use in a bit shall we. But first please tap on the stadium at the bottom of the map";
                nextMsg();
            }
            else if (index == 27)
            {
                text.text = "That was the AFAS Stadium";
                nextMsg();
            }
            //
            else if (index == 28)
            {
                text.text = "That was a Key Location info-Card. Each card has some fun facts about the location on the left hand side.";
                nextMsg();
            }
            else if (index == 29)
            {
                text.text = "The right hand side shows what renewable building you need to have in your grid in order to claim this stadium and rewards you get";
                nextMsg();
            }
            else if (index == 30)
            {
                text.text = "For AFAS stadium to be connected to our grid by you need a Solar pannel in your grid. As a reward, you'll get an extra double connector next turn";
                nextMsg();
            }
            else if (index == 31)
            {
                text.text = "Start your turn";
                CheckPlayerTurn();
                playerStates[0].clearHand();
                playerStates[0].refilSpecificHand(1, 1, 2);
            }
            else if (index == 32)
            {
                text.text = "Make a connection from a building in your grid to the stadium";
                nextMsg();
            }
            else if (index == 33)
            {
                addTile(3, 13);
                addTile(4, 13);
                playerStates[1].gameData.totalPoint = 45;
                DisablePopup();
                checkTileTaken(1, 7);
            }
            else if (index == 34)
            {
                playerStates[0].gameData.totalPoint = 35;
                EnablePopup();
                text.text = "The AFAS stadium is now using sustainable energy. Congrats";
                nextMsg();
            }

            else if (index == 35)
            {
                text.text = "You are now supplying renewable energy to AFAS Stadium and would you look at that, the emission bar has decreased drastically because of that.";
                playerStates[0].clearHand();
                playerStates[0].refilSpecificHand(4, 0, 0);
                nextMsg();
            }
            else if (index == 36)
            {
                text.text = "It looks like the second player has been making moves around Alkmaar while you were busy";
                nextMsg();
            }
            else if (index == 37)
            {
                text.text = "Lets claim that house above shall we?";
                nextMsg();
            }
            else if (index == 38)
            {
                text.text = "Start your turn and build as close as you can to that house";
                nextMsg();
            }
            else if (index == 39)
            {
                addTile(5, 12);
                addTile(5, 11);
                addTile(6, 10);
                playerStates[1].gameData.totalPoint = 42;
                DisablePopup();
                if (playerStates[0].gameData.tilesChosen.Count == 3)
                {
                    index++;
                }
            }
            else if (index == 40)
            {
                EnablePopup();
                text.text = "looks like you dont have enough connectors to reach the house. Let's use the last item in an inventory, this is called a node. Place it down at the end of the tile";
                nextMsg();
            }
            else if (index == 41)
            {
                DisablePopup();
                if (playerStates[0].gameData.nodesOwned.Count == 0)
                {
                    index++;
                }
            }
            else if (index == 42)
            {
                EnablePopup();
                text.text = "Nodes allow you to end your connection and continue the next turn";
                nextMsg();
            }
            else if (index == 43)
            {
                text.text = "But be careful you are only granted one per game and it comes as a cost";
                nextMsg();
            }
            else if (index == 44)
            {
                text.text = "Other players can also use your node to end their connection midway as well and continue their turn, So you'll have to be strategic about their placement";
                nextMsg();
            }
            else if (index == 45)
            {
                text.text = "Now place use your last connector to place over the node to end your turn ";
                nextMsg();
            }
            else if (index == 46)
            {
                text.text = "Connect to a house closest to you starting from your node to complete the tutorial";
                nextMsg();
            }
            else if (index == 47)
            {
                addTile(6, 9);
                addTile(7, 9);
                addTile(8, 8);
                playerStates[1].gameData.totalPoint = 42;
                DisablePopup();
                checkTileTaken(8, 5);
                checkTileTaken(8, 8);
                checkTileTaken(13, 10);
                checkTileTaken(6, 10);
            }
            else if (index == 48)
            {
                EnablePopup();
                text.text = "Congrats on finsihing the tutorial. Continue making Alkmaar a Greener place!";
                nextMsg();
            }


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

    public IEnumerator Wait(float delayInSecs)
    {
        yield return new WaitForSeconds(delayInSecs);
    }

    private void nextMsg()
    {
        if (Input.GetMouseButtonDown(0) && (EventSystem.current.IsPointerOverGameObject()))
        {
            index++;
        }
    }

    private void checkTileTaken(int x, int y)
    {
        if (TileManager.tiles[x][y].OwnedBy == playerStates[0])
        {
            playerStates[0].gameData.totalPoint = playerStates[0].gameData.totalPoint - 3;
            index++;
        }
    }

    private void CheckPlayerTurn()
    {
        if (playerStates[0].gameData.isTurn == true)
        {
            index++;
        }
    }


}

//insufficient length of connector used but still can end turn

//choose a random tiles next to one in the connection is allowed

//if you choose 3, after you placed all 3 you can't undo no more, not sure if it is a problem 
//at all


