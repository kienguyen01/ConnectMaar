using Google.Cloud.Translation.V2;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public Image tutBox;

    public GameObject popups;
    private int index = -1;

    private string[] tutorialMessage;

    public TextMeshProUGUI textTitle;

    public TextMeshProUGUI text;


    public TextAsset dataFile;

    private string[] dataLines;



    private bool go;
    private bool autoAdvance;
    private int temporaryRoutines;

    private GoogleTranslate translateService;

    new private void Awake()
    {
        translateService = new GoogleTranslate();

        onAwake();
        createPlayer();
    }

    private void Start()
    {
        player1.IsTutorial = true;
        Debug.Log("STARTTUTORIAL");

        if (TileManager.tiles[0][0].OwnedBy == null)
        {
            player1.gameData.PlayerColour = Color.white;
            TileManager.tiles[0][0].OwnedBy = player1;
            
        }

        //tutBox = GameObject.Find("TutorialBox").GetComponent<Image>();
        tutBox.enabled = true;
        text.enabled = true;
        textTitle.enabled = true;
        
       /* if (tutBox.enabled == true)
        {
            //text = GameObject.Find("txtBody").GetComponent<TextMeshProUGUI>();
            //textTitle = GameObject.Find("txtTitle").GetComponent<TextMeshProUGUI>();
            //textTitle.text = "Tutorial:";
        }*/
        getInstructions();
        StartCoroutine(TutorialRoutine());
    }

    IEnumerator TutorialRoutine()
    {
        //changeTutorialMessage();
        go = true;
        autoAdvance = false;
        temporaryRoutines = 0;
        while (true)
        {
            if (go && (autoAdvance || nextMsg()))
            {
                autoAdvance = false;
                index++;
                changeTutorialMessage();
            }

            if (index == 56)
            {
                DisablePopup();
                yield break;
            }
            yield return 0;
        }
    }

    private string doTranslation(string _Text)
    {
        string locale = PlayerPrefs.GetString("Selected_Language");
        if (locale != "en")
        {
            _Text = translateService.TranslateText(LanguageCodes.English, LanguageCodes.Dutch, _Text);
        }
        return _Text;
    }

    private void changeTutorialMessage()
    {
        if (text)
        {
            switch (index)//TODO Ideally, move every text into an array and just change with index value
            {
                case 0:
                    {   
                        text.text = doTranslation(dataLines[0]);
                        player1.clearHand()
                               .refilSpecificHand(1, 1, 2);
                        break;
                    }
                case 1:
                    text.text = doTranslation(dataLines[1]);
                    break;
                case 2:
                    text.text = doTranslation(dataLines[2]);
                    break;
                case 3:
                    text.text = doTranslation(dataLines[3]);
                    break;
                case 4:
                    text.text = doTranslation(dataLines[4]);
                    break;  
                case 5:
                    text.text = doTranslation(dataLines[5]);
                    break;
                case 6:
                    text.text = dataLines[6];
                    break;
                case 7:
                    text.text = dataLines[7];
                    break;
                case 8:
                    text.text = dataLines[8];
                    break;
                case 9:
                    text.text = dataLines[9];
                    EnableHighlightChanager(0, 1);
                    EnableHighlightChanager(1, 1);
                    EnableHighlightChanager(2, 1);
                    break;
                case 10:
                    text.text = dataLines[10];
                    DisablePopup();
                    turnCheck = true;
                    StartCoroutine(TileCheckRoutine(2,1));
                    break;
                case 11:
                    turnCheck = true;
                    EnablePopup();
                    text.text = dataLines[11];
                    break;
                case 12:
                    text.text = dataLines[12];
                    //StartCoroutine(TileCheckRoutine(6, 2)); //Remove if time left
                    break;
                case 13:
                    //EnablePopup();
                    text.text = dataLines[13];
                    break;
                case 14:
                    text.text = dataLines[14];
                    break;
                case 15:
                    text.text = dataLines[15];
                    break;
                case 16:
                    text.text = dataLines[16];
                    break;
                case 17:
                    player1.clearHand()
                           .refilSpecificHand(1, 2, 1);
                    text.text = dataLines[17];
                    EnableHighlightChanager(3, 2);
                    EnableHighlightChanager(4, 2);
                    EnableHighlightChanager(5, 2);
                    EnableHighlightChanager(6, 2);
                    break;
                case 18:
                    DisablePopup();
                    //turnCheck = true;
                    StartCoroutine(TileCheckRoutine(6, 2));
                    break;
                case 19:
                    turnCheck = true;
                    EnablePopup();
                    break;
                case 20:
                    //StartCoroutine(TileCheckRoutine(6, 2)); //remove
                    text.text = dataLines[18];
                    break;
                case 21:
                    EnablePopup();
                    text.text = dataLines[19];
                    break;
                case 22:
                    text.text = dataLines[20];
                    player1.clearHand();
                    break;
                case 23:
                    text.text = dataLines[21];
                    break;
                case 24:
                    text.text = dataLines[22];
                    break;
                case 25:
                    text.text = dataLines[23];
                    break;
                case 26:
                    text.text = dataLines[24];
                    EnableHighlightChanager(6, 1);
                    EnableHighlightChanager(7, 0);
                    break;
                case 27:
                    startAiMoves();
                    player2.gameData.totalPoint = 47;
                    DisablePopup();
                    StartCoroutine(TileCheckRoutine(7, 0));
                    //turnCheck = true;
                    break;
                case 28:
                    turnCheck = true;
                    EnablePopup();
                    break;
                case 29:
                    StartCoroutine(SolarHeatCheckRoutine());
                    break;
                case 30:
                    text.text = dataLines[25];
                    player1.clearHand()
                           .refilSpecificHand(1, 1, 2);
                    break;
                case 31:
                    text.text = dataLines[26];
                    break;
                case 32:
                    text.text = dataLines[27];
                    break;
                case 33:
                    text.text = "Let us use our newly acquired solar energy in our grid to power a key location in Alkmaar  ";
                    break;
                case 34:
                    text.text = "First, please tap on the stadium at the bottom of the map";
                    break;
                case 35:
                    DisablePopup();
                    StartCoroutine(OpenInfoCardCheckRoutine());
                    break;
                case 36:
                    EnablePopup();
                    text.text = "That was the info-Card for the AFAS Stadium. Each card has some fun facts about the location on the left-hand side.";
                    break;
                case 37:
                    text.text = "The right-hand side shows what renewable building you need to have in your grid in order to claim this stadium and the rewards you get";
                    break;
                case 38:
                    text.text = "For AFAS stadium to be connected to our grid you need a Solar panel in your grid. As a reward, you'll get an extra double connector next turn";
                    break;
                case 39:
                    //text.text = "Start your turn";
                    //StartCoroutine(TurnCheckRoutine());
                    player1.clearHand()
                           .refilSpecificHand(1, 1, 2);
                    break;
                case 40:
                    text.text = "Make a connection from a building in your grid to the stadium";
                    break;
                case 41:
                    player1.gameData.totalPoint = 45;
                    DisablePopup();
                    //StartCoroutine(TileCheckRoutine(1, 7));
                    addTile(3, 13);
                    addTile(4, 13);
                    break;
                case 42:
                    player1.gameData.totalPoint = 35;
                    EnablePopup();
                    text.text = "The AFAS stadium is now using sustainable energy. Congratulations!";
                    break;
                case 43:
                    text.text = "You are now supplying renewable energy to AFAS Stadium and would you look at that, the emission bar has decreased drastically because of that.";
                    player1.clearHand()
                           .refilSpecificHand(4, 0, 0);
                    break;
                case 44:
                    text.text = "It looks like the second player has been making moves around Alkmaar while you were busy";
                    break;
                case 45:
                    text.text = "Let's claim that house above shall we?";
                    break;
                case 46:
                    text.text = "Start your turn and build as close as you can to that house";
                    break;
                case 47:
                    {
                        addTile(5, 12);
                        addTile(5, 11);
                        addTile(6, 10);
                        player2.gameData.totalPoint = 42;
                        DisablePopup();

                        StartCoroutine(TilesChosenCheckRoutine(3));
                    }
                    break;
                case 48:
                    EnablePopup();
                    text.text = "looks like you dont have enough connectors to reach the house. Let's use the last item in an inventory, this is called a node. Place it down at the end of the tile";
                    break;
                case 49:
                    DisablePopup();
                    StartCoroutine(NodesOwnedCheckRoutine(0));
                    break;
                case 50:
                    EnablePopup();
                    text.text = "Nodes allow you to end your connection and continue the next turn";
                    break;
                case 51:
                    text.text = "But be careful you are only granted one per game and it comes as a cost";
                    break;
                case 52:
                    text.text = "Other players can also use your node to end their connection midway as well and continue their turn, So you'll have to be strategic about their placement";
                    break;
                case 53:
                    text.text = "Now place use your last connector to place over the node to end your turn ";
                    break;
                case 54:
                    text.text = "Connect to a house closest to you starting from your node to complete the tutorial";
                    break;
                case 55:
                    {
                        addTile(6, 9);
                        addTile(7, 9);
                        addTile(8, 8);
                        player2.gameData.totalPoint = 42;
                        DisablePopup();
                        StartCoroutine(TileCheckRoutine(8, 5));
                        StartCoroutine(TileCheckRoutine(8, 8));
                        StartCoroutine(TileCheckRoutine(13, 10));
                        StartCoroutine(TileCheckRoutine(6, 10));
                        break;
                    }
                case 56:
                    EnablePopup();
                    text.text = "Congratulations on finishing the tutorial! Continue making Alkmaar a greener place!";
                    break;
            }
        }
    }

    private void getInstructions()
    {

        dataLines = dataFile.text.Split('\n');
        string[] lines = new string[dataLines.Length];

    }

    private IEnumerator TurnCheckRoutine(bool YourTurn = true, bool AutoAdvance = true)
    {
        go = false;
        while (true)
        {
            if (player1.gameData.IsTurn == YourTurn)
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
            if (TileManager.tiles[x][y].OwnedBy == player1)
            {
                player1.gameData.totalPoint = player1.gameData.totalPoint - 3;
                if (temporaryRoutines <= 1)
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

    private void EnableHighlightChanager(int x, int y)
    {
            if (TileManager.tiles[x][y].OwnedBy != player1)
            {
                TileManager.tiles[x][y].gameObject.AddComponent<Outline>()
                .OutlineColor = Color.red;
            }
    }
    private void DisableHighlightChanager(int x, int y)
    {
        if (TileManager.tiles[x][y].OwnedBy != player1 )
        {
            Outline o = TileManager.tiles[x][y].gameObject.GetComponent<Outline>();

            Destroy(o);
/*            TileManager.tiles[x][y].gameObject.AddComponent<Outline>()
            .OutlineColor = Color.clear;*/
        }
    }

    private IEnumerator SolarHeatCheckRoutine(bool solar = true)
    {
        go = false;
        while (true)
        {
            if ((solar) ? player1.gameData.hasSolarInNetwork : player1.gameData.hasHeatInNetwork)
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
            if (player1.gameData.tilesChosen.Count == count)
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
            if (player1.gameData.nodesOwned.Count == count)
            {
                go = true;
                autoAdvance = true;
                yield break;
            }
            yield return 0;
        }
    }

    private IEnumerator OpenInfoCardCheckRoutine()
    {
        go = false;
        bool opened = false;
        while (true)
        {
            if (TileManager.pH.isOpen())
            {
                opened = true;
                yield return 0;
            }

            if (!TileManager.pH.isOpen() && opened)
            {
                go = true;
                autoAdvance = true;
                yield break;
            }
            else
            {
                yield return 0;
            }
        }
    }

    public void DisablePopup()
    {
        text.enabled = false;
        tutBox.enabled = false;
        textTitle.enabled = false;
    }

    public void EnablePopup()
    {
        text.enabled = true;
        tutBox.enabled = true;
        textTitle.enabled = true;
        textTitle.text = "Tutorial:";
    }

    public void startAiMoves()
    {
        /*Make ai have connectionsns to the two houses closed to him */
        player2.gameData.PlayerColour = Color.black;
        addTile(0, 14);
        addTile(0, 13);
        addTile(1, 13);
        addTile(2, 13);
    }

    private void addTile(int x, int y)
    {
        player2.gameData.tilesChosen.Add(TileManager.tiles[x][y]);
        TileManager.tiles[x][y].OwnedBy = player2;
    }

    private bool nextMsg()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject tileObjectTouched = hitInfo.collider.transform.gameObject;
            tutBox = tileObjectTouched.GetComponent<Image>();
            if (tutBox == null)
            {
                tutBox = tileObjectTouched.GetComponentInParent<Image>();
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            tutBox = GameObject.Find("TutorialBox").GetComponent<Image>();
            return true;

        }
        return false;




        /*        if (Input.GetMouseButtonDown(0) && !(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)))
                {
                    return true;
                }
                return false;*/
    }

    /*
    private void checkTileTaken(int x, int y)
    {
        if (TileManager.tiles[x][y].OwnedBy == player1)
        {
            player1.gameData.totalPoint = player1.gameData.totalPoint - 3;
            go = true;
        }
    }*/


}