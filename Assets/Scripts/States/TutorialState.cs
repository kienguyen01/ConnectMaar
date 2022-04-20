using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

[System.Serializable]
public struct TutorialStateConfig
{
    public PlayerState PlayerStateClass;
    public TileManager TileManagerClass;
}



public class TutorialState :  GameState
{
    public TutorialManager TutorialManagerClass;
    private TutorialManager TutorialManager;

    public GameObject popups;
    private int index;

    private string[] tutorialMessage;


    public override void startPoint()
    {
        if(TileManager.tiles[0][0].OwnedBy == null)
        {
            TileManager.tiles[0][0].OwnedBy = playerStates[0];
        }
    }



    public override void TutorialStart()
    {
        text = GameObject.Find("TutorialMessgae").GetComponent<TextMeshProUGUI>();
        if (text != null)
        {
            if ( index == 0)
            {
                GameObject varGameObject = GameObject.Find("Plane");
                varGameObject.GetComponent<PhoneCameraMovement>().enabled = false;
                text.text = "Welcome to the Tutorial Player 1. In this tutorial we will teach you how to play ConnectMaar. Tap on Screen to start.";
                playerStates[0].clearHand();
                playerStates[0].refilSpecificHand(1, 1, 2);
                nextMsg();
            }
            else if (index == 1)
            {
                text.text = "This is a tutorial to teach you the basic gameplay of our game. Tap for next.";
                nextMsg();
            }
            else if(index == 2)
            {
                text.text = "On the left hand side of your screen you can see the inventory screen";
                nextMsg();
            }
            else if (index == 3)
            {
                text.text = "In your inventory you have Items called connectors. They are your main tool in making Alkmaar a greener place";
                nextMsg();
            }
            else if (index == 4)
            {
                text.text ="Right now in your inventory you have a single connector,a double connector and a tripple connector";
                nextMsg();

            }
            else if (index == 5)
            {
                text.text = "Depending on the length and number of connectors you use in your connection the amount your emmision bar decreses will change";
                nextMsg();
            }
            else if (index == 6)
            {
                text.text = "Ok lets begin using renewable energy to make alkmaar a better place ";
                nextMsg();
            }
            else if (index == 7)
            {
                text.text = "Start your turn by pressing the start turn button.";
                if (playerStates[0].gameData.isTurn ==  true)
                {
                    index++;
                }
            }
            else if(index == 8)
            {
                text.text = "Claim the house closest to you using the least amount of pipes and end your turn";
                checkTileTaken(2, 1);
            }
            else if(index == 9)
            {
                text.text = "Congratulations you have started to make Alkmaar a greener place";
                playerStates[0].clearHand();
                playerStates[0].refilSpecificHand(1, 1, 2);

                nextMsg();
            }
            else if (index == 10)
            {
                text.text = "Will you look at that. Your emmision bar has descressed by 3 points";

                nextMsg();
            }
            else if(index == 11)
            {
                text.text = "1 connector == 1point,  2 Connectors ==  2 points,  3 Connectors = 3 points";
                nextMsg();
            }
            else if (index == 12)
            {
                text.text = "At the end of each turn your hand size of pipes will be filled up again";
                nextMsg();
            }
            else if (index == 13)
            {
                text.text = "Start your turn again to continue";
                if (playerStates[0].gameData.isTurn == true)
                {
                    index++;
                }
            }
            else if(index == 14)
            {
                text.text = "Use the newly aquired pipes to make a connection to the house nearby";
                checkTileTaken(6,2);
            }
            else if (index == 15)
            {
                text.text = "It looks like you built over scrabble tiles and claimed " + playerStates[0].gameData.SpecialConnector.Count + " special connectors";
                playerStates[0].clearHand();
                nextMsg();
            }
            else if (index == 16)
            {
                text.text = "Special connectors are used to add special renewable resource in your grid such as heatpumps and solar pannels";
                nextMsg();
            }
            else if (index == 17)
            {
                text.text = "The special connector you have can only be used to connect to a solar pannel ";
                nextMsg();
            }
            else if (index == 18)
            {
                text.text = "Please use the special solar connector to connect to the solar pannel ahead.";
                if (playerStates[0].gameData.hasSolarInNetwork)
                {
                    index++;
                }
            }
            else if(index == 18)
            {
                text.text = "You have now added a solar pannel to your grid. Congrats this wilol be useful later";
                playerStates[0].refilSpecificHand(1, 1, 2);
                nextMsg();
            }
            else if(index == 19)
            {
                text.text = "I think its time for you to use it but first click on the help icon at the top of the screne";
                /*if ()
                {
                CHECK IF THE STADIUM UI POPUP HA BEEN CLICKED AND THEN AFTERWARDS INCREMENT THE INDEX, UPDATED UI FROM SAMUEL NEEDED AND TESTSED
                }*/
                nextMsg();
            }
            else if (index == 20)
            {
                text.text = "This is the Key Locations card";
                nextMsg();
            }
            else if (index == 21)
            {
                text.text = "Each card has some fun fact abou the location on the left hand side";
                nextMsg();
            }
            else if (index == 22)
            {
                text.text = "This is the Key Locations card. Each card has some fun fact abou the location on the left hand side.";
                nextMsg();
            }
            else if (index == 23)
            {
                text.text = "The right hand side shows what renewable building you need to have in your grid in order to claim this stadium and rewards you get";
                nextMsg();
            }
            else if (index == 24)
            {
                text.text = "For this stadium close by you need a Solar pannel in your grid and you get an extra double connector next turn";
                nextMsg();
            }
            else if (index == 25)
            {
                text.text = "Start your turn";
                CheckPlayerTurn();
            }
            else if (index == 26)
            {
                text.text = "make a connection from building in your grid to the stadium";
                checkTileTaken(2, 7);
            }
            else if (index == 27)
            {
                text.text = "You are now supplying renewable energy to Afas Stadium and would you look at that, the emmision bar has decresed drastically because of that.";
                playerStates[0].clearHand();
                playerStates[0].refilSpecificHand(4, 0, 0);
                nextMsg();
            }
            else if (index == 28)
            {
                text.text = "It looks like the second player has been making moves around Alkmaar while you were busy";
            }



        }
    }

    public void startAiMoves()
    {
        /*Make*/
    }

    public IEnumerator Wait(float delayInSecs)
    {
        yield return new WaitForSeconds(delayInSecs);
    }

    private void nextMsg()
    {
        if (Input.GetMouseButtonDown(0) && !(EventSystem.current.IsPointerOverGameObject()))
        {
            index++;
        }
    }

    private void checkTileTaken(int x, int y)
    {
        if (TileManager.tiles[x][y].OwnedBy == playerStates[0])
        {
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


