using Google.Cloud.Translation.V2;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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
        //(int x, int y)[] coordinates = { (8, 5), (10, 0) };


    }

    IEnumerator TutorialRoutine()
    {
        //changeTutorialMessage();
        text.text = doTranslation(text.text);
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

    List<(int x, int y)> validHouses = new List<(int x, int y)>();

    private void changeTutorialMessage()
    {
        List<(int x, int y)> aimoves = new List<(int x, int y)>();
        if (text)
        {
            switch (index)//TODO Ideally, move every text into an array and just change with index value
            {
                case 0:
                    {
                        aimoves.Add((0, 14));

                        startAiMoves(aimoves);

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
                    text.text = doTranslation(dataLines[6]);
                    break;
                case 7:
                    text.text = doTranslation(dataLines[7]);
                    break;
                case 8:
                    text.text = doTranslation(dataLines[8]);
                    break;
                case 9:
                    text.text = doTranslation(dataLines[9]);
                    EnableHighlightChanager(0, 1);
                    EnableHighlightChanager(1, 1);
                    EnableHighlightChanager(2, 1);
                    break;
                case 10:
                    DisablePopup();
                    turnCheck = true;
                    StartCoroutine(TileCheckRoutine(2,1));
                    break;
                case 11:
                    turnCheck = true;
                    EnablePopup();
                    text.text = doTranslation(dataLines[10]);
                    break;
                case 12:
                    text.text = doTranslation(dataLines[11]);
                    //StartCoroutine(TileCheckRoutine(6, 2)); //Remove if time left
                    break;
                case 13:
                    //EnablePopup();
                    text.text = doTranslation(dataLines[12]);
                    break;
                case 14:
                    text.text = doTranslation(dataLines[13]);
                    break;
                case 15:
                    text.text = doTranslation(dataLines[14]);
                    break;
                case 16:
                    DisablePopup();
                    player1.clearHand()
                           .refilSpecificHand(1, 2, 1);
                    EnableHighlightChanager(3, 2);
                    EnableHighlightChanager(4, 2);
                    EnableHighlightChanager(5, 2);
                    EnableHighlightChanager(6, 2);
                    break;
                case 17:
                    //turnCheck = true;
                    StartCoroutine(TileCheckRoutine(6, 2));
                    break;
                case 18:
                    turnCheck = true;
                    EnablePopup();
                    text.text = doTranslation(dataLines[15]);
                    break;
                case 19:
                    //StartCoroutine(TileCheckRoutine(6, 2)); //remove
                    break;
                case 20:
                    //EnablePopup();
                    text.text = doTranslation(dataLines[16]);
                    break;
                case 21:
                    text.text = doTranslation(dataLines[17]);
                    player1.clearHand();
                    break;
                case 22:
                    text.text = doTranslation(dataLines[18]);
                    break;
                case 23:
                    DisablePopup();
                    EnableHighlightChanager(6, 1);
                    EnableHighlightChanager(7, 0);
                    break;
                case 24:
                    aimoves.Clear();

                    aimoves.Add((0, 13));
                    aimoves.Add((1, 13));
                    aimoves.Add((2, 13));

                    startAiMoves(aimoves);

                    player2.gameData.totalPoint = 47;
                    StartCoroutine(TileCheckRoutine(7, 0));
                    //turnCheck = true;
                    break;
                case 25:
                    turnCheck = true;
                    EnablePopup();
                    break;
                case 26:
                    StartCoroutine(SolarHeatCheckRoutine());
                    break;
                case 27:
                    text.text = doTranslation(dataLines[19]);
                    player1.clearHand()
                           .refilSpecificHand(1, 1, 2);
                    break;
                case 28:
                    text.text = doTranslation(dataLines[20]);
                    break;
                case 29:
                    text.text = doTranslation(dataLines[21]);
                    break;
                case 30:
                    text.text = doTranslation(dataLines[22]);
                    break;
                case 31:
                    text.text = doTranslation(dataLines[23]);
                    break;
                case 32:
                    text.text = doTranslation(dataLines[24]);
                    break;
                case 33:
                    DisablePopup();
                    StartCoroutine(OpenInfoCardCheckRoutine());
                    break;
                case 34:
                    EnablePopup();
                    text.text = doTranslation(dataLines[25]);
                    break;
                case 35:
                    text.text = doTranslation(dataLines[26]);
                    break;
                case 36:
                    text.text = doTranslation(dataLines[27]);
                    break;
                case 37:
                    //text.text = "Start your turn";
                    //StartCoroutine(TurnCheckRoutine());
                    player1.clearHand()
                           .refilSpecificHand(1, 1, 2);
                    break;
                case 38:
                    text.text = doTranslation(dataLines[28]);
                    break;
                case 39:
                    DisablePopup();
                    StartCoroutine(TileCheckRoutine(1, 7));
                    addTile(3, 13);
                    addTile(4, 13);
                    break;
                case 40:
                    //player1.gameData.totalPoint = 35;
                    player1.gameData.totalPoint = player1.gameData.totalPoint - 10;
                    EnablePopup();
                    aimoves.Add((3, 13));
                    aimoves.Add((4, 13));
                    startAiMoves(aimoves);
                    text.text = doTranslation(dataLines[29]);
                    break;
                case 41:
                    text.text = doTranslation(dataLines[30]);
                    player1.clearHand()
                           .refilSpecificHand(4, 0, 0);
                    break;
                case 42:
                    text.text = doTranslation(dataLines[31]);
                    break;
                case 43:
                    text.text = doTranslation(dataLines[32]);
                    break;
                case 44:
                    text.text = doTranslation(dataLines[33]);
                    break;
                case 45:
                    text.text = doTranslation(dataLines[34]);
                    break;
                case 46:
                    text.text = doTranslation(dataLines[35]);
                    break;
                case 47:
                    text.text = doTranslation(dataLines[36]);
                    break;
                case 48:
                    text.text = doTranslation(dataLines[37]);
                    break;
                case 49:
                    DisablePopup();
                    //(int x, int y)[] coordinates = { (8, 5), (10, 0), (8,8), (6,10)};
                    validHouses.Add((8, 5));
                    validHouses.Add((10, 0));
                    validHouses.Add((8, 8));
                    validHouses.Add((6, 10));
                    validHouses.Add((11, 6));
                    turnCheck = true;
                    StartCoroutine(MultiTileCheckRoutine(validHouses));
                    break;
                case 50:
                    turnCheck = true;
                    if (TileManager.tiles[11][6].OwnedBy == player1)
                    {
                        SceneManager.LoadScene("WINNER");
                    }
                    else
                    {
                        if (TileManager.tiles[6][10].OwnedBy == null)
                        {
                            aimoves.Clear();
                            aimoves.Add((5, 12));
                            aimoves.Add((5, 11));
                            aimoves.Add((6, 10));
                            startAiMoves(aimoves);
                            validHouses.Remove((6, 10));
                        }
                        player1.clearHand()
                            .refilSpecificHand(1, 1, 2);
                    }
                    break;
                case 51:
                    StartCoroutine(MultiTileCheckRoutine(validHouses));
                    break;
                case 52:
                    if (TileManager.tiles[11][6].OwnedBy == player1)
                    {
                        SceneManager.LoadScene("Winnner");
                    } else
                    {
                        if (TileManager.tiles[13][10].OwnedBy == null)
                        {
                            aimoves.Clear();
                            aimoves.Add((6, 11));
                            aimoves.Add((7, 11));
                            aimoves.Add((8, 11));
                            aimoves.Add((9, 11));
                            aimoves.Add((10, 11));
                            aimoves.Add((11, 11));
                            aimoves.Add((12, 10));
                            aimoves.Add((13, 10));
                            startAiMoves(aimoves);
                        }
                        player1.clearHand()
                                    .refilSpecificHand(1, 1, 2);
                    }
                    turnCheck = true;
                    break;
                case 53:
                    StartCoroutine(MultiTileCheckRoutine(validHouses));
                    break;
                case 54:
                    if (TileManager.tiles[11][6].OwnedBy == player1 || TileManager.tiles[11][6].OwnedBy == player2)
                    {
                        SceneManager.LoadScene("Winnner");
                    }
                    else
                    {
                        if (TileManager.tiles[11][6].OwnedBy == null)
                        {
                            aimoves.Clear();
                            aimoves.Add((12, 9));
                            aimoves.Add((12, 8));
                            aimoves.Add((11, 7));
                            aimoves.Add((11, 6));
                            aimoves.Add((11, 5));
                            aimoves.Add((11, 7));
                            aimoves.Add((12, 6));
                            startAiMoves(aimoves);
                        }
                    }
                    break;
                case 55:
                    EnablePopup();
                    text.text = doTranslation(dataLines[38]);
                    break;
                case 56:
                    SceneManager.LoadScene("Menu");
                    break;
            }
        }
    }

    private void getInstructions()
    {

        dataLines = dataFile.text.Split('\n');
        string[] lines = new string[dataLines.Length];

    }


    private IEnumerator MultiTileCheckRoutine(List<(int x, int y)> coordinates)
    {
        go = false;
        while (true)
        {
            foreach (var item in coordinates)
            {
                if (TileManager.tiles[item.x][item.y].OwnedBy == player1)
                {
                    go = true;
                    autoAdvance = true;
                    validHouses.Remove((item.x, item.y));
                    yield break;

                }

            }
            yield return 0;

        }
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

    public void startAiMoves(List<(int x, int y)> coordinates)
    {
        /*Make ai have connectionsns to the two houses closed to him */
        player2.gameData.PlayerColour = Color.black;

        foreach(var item in coordinates)
        {
            addTile(item.x, item.y);
        }
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