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


public class TutorialState :  GameState
{
    public TutorialManager TutorialManagerClass;
    private TutorialManager TutorialManager;

    public GameObject popups;
    private int index;

    private string[] tutorialMessage;

    Button button;


    public override void startPoint()
    {
        if(tileManager.tiles[0][0].OwnedBy == null)
        {
            tileManager.tiles[0][0].OwnedBy = playerStates[0];
        }
    }


    public override void TutorialStart()
    {
        text = GameObject.Find("TutorialMessgae").GetComponent<TextMeshProUGUI>();
        if (text != null)
        {
            if ( index == 0)
            {
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
                nextMsg();
            }
            else if (index == 16)
            {
                text.text = "Special connectors are used to add special renewable resource in your grid such as heatpumps and solar pannels";
                nextMsg();
            }
            else if (index == 17)
            {
                text.text = "The special connector you have is used for the";
                nextMsg();
            }
        }
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
        if (tileManager.tiles[x][y].OwnedBy == playerStates[0])
        {
            index++;
        }
    }


}
