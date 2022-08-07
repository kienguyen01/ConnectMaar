using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[Serializable]
public struct MapData
{
    public List<string> houses;
    public List<string> stadiums;
    public List<string> churches;
    public List<string> solars;
    public List<string> heats;
    public List<string> scrabbleSolar;
    public List<string> scrabbleHeats;
    public List<string> windTurbines;
}

[Serializable]
public struct EndTurnData
{
    public List<string> tilesChosen;
    public float totalPoint;
    public List<string> nodesPlaced;
}





public class MultiplayerState : GameState
{
    List<string> factPopups;
    MapData mapData;
    EndTurnData endTurnData;
    
    /*public MultiplayerPlayerState player1;
    public MultiplayerPlayerState player2;*/

    //public PlayerState currentPlayer;

    new public static MultiplayerState instance;

    [SerializeField]
    TextMeshProUGUI playey1Name;
    [SerializeField]
    TextMeshProUGUI BuildingsCount;
    [SerializeField]
    TextMeshProUGUI ConnectorsCount;


    private void Start()
    {
        mapData = new MapData();
        endTurnData = new EndTurnData();
        mapData.houses = new List<string>();
        mapData.stadiums = new List<string>();
        mapData.churches = new List<string>();
        mapData.solars = new List<string>();
        mapData.heats = new List<string>();
        mapData.scrabbleSolar = new List<string>();
        mapData.scrabbleHeats = new List<string>();
        mapData.windTurbines = new List<string>();

        //timer.GetComponent<Renderer>().enabled = true;
        //turnTime = new Timer();
        instantiatePopup();
        Debug.Log("Multiplayer");
        if (PhotonNetwork.IsMasterClient)
        {
            addSpecialTiles();
            photonView.RPC("CreateMap", RpcTarget.All, ObjectToByteArray(mapData));
            SetPlayers();
        }

        // name.text = PhotonNetwork.LocalPlayer.NickName;
        playey1Name.text = PhotonNetwork.LocalPlayer.NickName;
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " THIS WORKS");

    }

    [PunRPC]
    void DebugTest()
    {
        Debug.LogError(player1 ? "current assigned" : "current empty");
    }

    [PunRPC]
    void WinCondition()
    {
        if (player1.gameData.totalPoint == 0)
        {
            NetManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "Winnner");
        }
    }

    private void Update()
    {
        

        if (player1.gameData.IsTurn)
        {
            if (Input.GetKeyDown(KeyCode.Space) || turnCheck)
            {
                    CheckEndTurn();
            }
            if (Input.GetKeyDown(KeyCode.Alpha0) || clearBtn)
            {
                clearAllSelected();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1) || p1)
            {
                SelectSingleConnector();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) || p2)
            {
                SelectDoubleConnector();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) || p3)
            {
                SelectTripleConnector();
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) || solarCheck)
            {
                SelectSolarConnector();
            }
            if (Input.GetKeyDown(KeyCode.Alpha5) || heatCheck)
            {
                SelectHeatConnector();
            }
            if (Input.GetKeyDown(KeyCode.Alpha6) || nodeCheck)
            {
                SelectNodeConnector();
            }

            if (Input.GetMouseButtonDown(0))
            {
                HandleClickCheck();
            }
            
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            string factPopup = "Fact" + Random.Range(1, 4).ToString();

            foreach(string fact in factPopups)
            {
                pH.canvas = GameObject.Find(fact).GetComponent<Canvas>();
                if (pH.isOpen())
                {
                    pH.canvas.enabled = false;
                }
            }

            pH.canvas = GameObject.Find(factPopup).GetComponent<Canvas>();
            pH.Popup();
        }
        if (Input.GetMouseButtonDown(0) && !selectedConnector)
            GetInfoCard(player1);
    }

    new protected bool CheckEndTurn()
    {
        photonView.RPC("RPC_Log", RpcTarget.All, $"{PhotonNetwork.LocalPlayer.UserId} --- local player: {player1.gameData.IsTurn} - foreign player: {player2.gameData.IsTurn}");

        List<Tile> chosenTiles = new List<Tile>(player1.gameData.tilesChosen);

        bool returnObj = base.CheckEndTurn();

        if (returnObj)
        {
            endTurnData.tilesChosen = new List<string>();
            endTurnData.nodesPlaced = new List<string>();
            endTurnData.totalPoint = player1.gameData.totalPoint;
            int count = 0;

            foreach (var item in chosenTiles)
            {
                endTurnData.tilesChosen.Add(item.X.ToString().PadLeft(3, '0') + "|" + item.Y.ToString().PadLeft(3, '0'));
                if (item.Structure.GetType() == typeof(Node))
                {
                    endTurnData.nodesPlaced.Add(item.X.ToString().PadLeft(3, '0') + "|" + item.Y.ToString().PadLeft(3, '0'));
                }
            }
            foreach(var item in chosenTiles)
            {
                if(item.Structure.GetType() == typeof(House))
                {
                    count++;
                }
            }
            BuildingsCount.text = "Buildings Owned: " + count;
            ConnectorsCount.text = "Connector1: " + Connector1Count + " \n Connector2: " + Connector2Count + " \n Connector3: " + Connector3Count;

            photonView.RPC("SendTiles", RpcTarget.Others, ObjectToByteArray(endTurnData));

            photonView.RPC("EndTurn", RpcTarget.Others);

            photonView.RPC("WinCondition", RpcTarget.All);

        }
        return returnObj;
    }

    void SetPlayers()
    {
        /*player1.photonView.TransferOwnership(1);
        player2.photonView.TransferOwnership(2);*/

        photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
        photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.PlayerListOthers[0]);

        Debug.LogError($"{((MultiplayerPlayerState)player1).photonPlayer.NickName}");
        Debug.LogError($"{((MultiplayerPlayerState)player2).photonPlayer.NickName}");

        photonView.RPC("SetFirstTurn", RpcTarget.All);

        Debug.LogError($"{player1.gameData.IsTurn}");
        Debug.LogError($"{player2.gameData.IsTurn}");

        /*
            player1.RefillHand();
            player2.RefillHand();
        */

        photonView.RPC("RPC_HandRefill", RpcTarget.All);

        photonView.RPC("RPC_Log", RpcTarget.All, $"{PhotonNetwork.LocalPlayer.UserId} --- local player: {player1.gameData.IsTurn} - foreign player: {player2.gameData.IsTurn}");

        /*
        player1.EndTurn();
        RPCSendTakenTiles();
        */
    }

    [PunRPC]
    void RPC_HandRefill()
    {
        player1.RefillHand();
    }

    [PunRPC]
    void RPC_Log(String s)
    {
        Debug.Log(s);
    }

    [PunRPC]
    private void Initialize(Photon.Realtime.Player player)
    {
        if (player.IsLocal)
        {
            ((MultiplayerPlayerState)player1).photonPlayer = player;
            player1.gameData.PlayerColour = Color.blue;
        }
        else
        {
            ((MultiplayerPlayerState)player2).photonPlayer = player;
            player2.gameData.PlayerColour = Color.black;
        }

        if (player1 && (((MultiplayerPlayerState)player1).photonPlayer != null && ((MultiplayerPlayerState)player1).photonPlayer.IsMasterClient))
        {
            TileManager.tiles[14][22].OwnedBy = player2;
            TileManager.tiles[12][11].OwnedBy = player1;
        }
        else
        {
            TileManager.tiles[14][22].OwnedBy = player1;
            TileManager.tiles[12][11].OwnedBy = player2;
        }
    }

    [PunRPC]
    void SetFirstTurn()
    {
        if(((MultiplayerPlayerState)player1).photonPlayer.IsMasterClient)
        {
            player1.gameData.IsTurn = true;
            player2.gameData.IsTurn = false;
        }
        else
        {
            player1.gameData.IsTurn = false;
            player2.gameData.IsTurn = true;
        }
        player1.name = PhotonNetwork.LocalPlayer.NickName;

        /*if(player1 == MultiplayerPlayerState.me)
        {
            MultiplayerPlayerState.me.BeginTurn();
        }*/
    }

    [PunRPC]
    void EndTurn()
    {
        player1.gameData.IsTurn = true;
        player2.gameData.IsTurn = false;
        Debug.LogError("turn changed");
        Debug.LogError(player1.gameData.IsTurn ? "p1" : "p2");
    }

    [PunRPC]
    void CreateMap(byte[] transferObject)
    {
        MapData mapData = (MapData)ByteArrayToObject(transferObject);
        tileManager.scrabbleSolar = mapData.scrabbleSolar;
        for (int x = 0; x < 30; x++)
        {
            List<Tile> tileRow = new List<Tile>();

            for (int y = 0; y < 30; y++)
            {
                tileRow.Add((tileManager.GenerateTilesMap(x, y)));
            }

            TileManager.tiles.Add(tileRow);
        }
        foreach (List<Tile> tileRow in TileManager.tiles)
        {
            foreach (Tile tile in tileRow)
            {
                tileManager.instantiateSpecialTile(tile, mapData.churches, mapData.solars, mapData.stadiums, mapData.houses, mapData.scrabbleSolar, mapData.windTurbines, mapData.scrabbleHeats, mapData.heats);
            }
        }
    }

    [PunRPC]
    void SendTiles(byte[] transferObject)
    {
        EndTurnData _endTurnData = (EndTurnData)ByteArrayToObject(transferObject);


        foreach (string tileLocation in _endTurnData.nodesPlaced)
        {
            var tileCoords = tileLocation.Split('|');
            int local_X = Convert.ToInt32(tileCoords[0]);
            int local_Y = Convert.ToInt32(tileCoords[1]);

            Node nodeP = Instantiate(tileManager.nodePrefab, (local_Y % 2 == 0) ? new Vector3(local_X * 1.04f, 0.2f, local_Y * 0.9f) : new Vector3(local_X * 1.04f + 0.498f, 0.2f, local_Y * 0.9f - 0.084f), Quaternion.identity);
            TileManager.tiles[local_X][local_Y].AddStructure<Node>(nodeP);
        }

        foreach (string tileLocation in _endTurnData.tilesChosen)
        {
            var tileCoords = tileLocation.Split('|');
            Tile tile = TileManager.tiles[Convert.ToInt32(tileCoords[0])][Convert.ToInt32(tileCoords[1])];

            if (!(tile.Structure.GetType() == typeof(Node)))
            {
                tile.OwnedBy = player2;
            }
            tile.SelectedBy = null;
            AssignScrabbleTileRewards(tile);
        }


        player2.gameData.tilesTaken.AddRange(player2.gameData.tilesChosen);
        player2.gameData.totalPoint = _endTurnData.totalPoint;
    }

    public void AssignScrabbleTileRewards(Tile tile)
    {
        if (tile.IsScrambleForHeat)
        {
            player1
                .AddHeatPipeConnector()
                .AddHeatPipeConnector();

        }
        else if (tile.IsScrambleForSolar)
        {
            player1
                .AddSolarConnector()
                .AddSolarConnector();
        }
    }

    void addSpecialTiles()
    {
        PickRandomTiles();
    }

    public void PickRandomTiles()
    {
        // Template         houses.Add("|,|,|");
        string output;
        List<string> houses = new List<string>();
        houses.Add("2|4,5|4,4|8");
        houses.Add("8|6,10|3,10|8");
        houses.Add("9|12,8|16,6|11");
        houses.Add("3|25,5|23,4|27");
        houses.Add("8|19,9|23,0|25");
        houses.Add("12|2,12|5,14|4");
        houses.Add("16|17,18|9,16|10");
        houses.Add("16|28,18|26,15|27");
        houses.Add("2|26,11|28,11|24");
        houses.Add("18|16,18|19,18|13");
        houses.Add("27|26,26|25,23|27");
        houses.Add("19|23,21|20,23|22");
        houses.Add("27|18,28|14,25|16");
        houses.Add("21|14,22|12,21|17");
        houses.Add("27|9,27|6,24|7");
        houses.Add("28|1,25|1,27|4");
        
        houses.Add("2|15,4|12,1|2");
        houses.Add("15|26,13|25,12|29");
        houses.Add("11|23,12|20,15|16");
        houses.Add("2|8,3|12,2|14");

        houses.Add("1|20,3|20,5|18");
        houses.Add("25|28,22|28,7|22");
        houses.Add("15|12,9|9,11|14");
        houses.Add("18|3,13|9,9|1");


        List<string> windTurbines = new List<string>();
        windTurbines.Add("14|22,14|22,14|22");
        windTurbines.Add("12|11,12|11,12|11");

        int x = Random.Range(1, 3);
        foreach(string hex in houses)
        {
            string[] y = hex.Split(',');

            for (int i = 0; i < y.Length; i++)
            {
                if(i == x)
                {
                    string[] h = y[i].Split('|');
                    output = h[0].ToString().PadLeft(3, '0') + "|" + h[1].ToString().PadLeft(3, '0');
                    mapData.houses.Add(output);
                }
            }
        }
        foreach (string hex in windTurbines)
        {
            string[] y = hex.Split(',');

            for (int i = 0; i < y.Length; i++)
            {
                if (i == x)
                {
                    string[] h = y[i].Split('|');
                    output = h[0].ToString().PadLeft(3, '0') + "|" + h[1].ToString().PadLeft(3, '0');
                    mapData.windTurbines.Add(output);
                }
            }
        }
        KeyLocation();

    }

    public void KeyLocation()
    {
        mapData.stadiums.Add(randomizeTile(9, 17, 9, 17));
        mapData.stadiums.Add(randomizeTile(21, 5, 21, 5));
        mapData.stadiums.Add(randomizeTile(24, 19, 24, 19));
        mapData.churches.Add(randomizeTile(7, 6, 7, 6));

        //School
        //

         mapData.solars.Add(randomizeTile(19, 19, 19, 19));
         mapData.solars.Add(randomizeTile(27, 9, 27, 9));

        mapData.scrabbleSolar.Add(randomizeTile(17, 17, 17, 17));
        mapData.scrabbleSolar.Add(randomizeTile(16, 18, 16, 18));
        mapData.scrabbleSolar.Add(randomizeTile(17, 6, 17, 6));
        mapData.scrabbleSolar.Add(randomizeTile(13, 17, 13, 17));

        mapData.scrabbleSolar.Add(randomizeTile(5, 27, 5, 27));

        mapData.scrabbleSolar.Add(randomizeTile(9, 26, 9, 26));
        mapData.scrabbleSolar.Add(randomizeTile(25, 14, 25, 14));

        mapData.scrabbleSolar.Add(randomizeTile(6, 3, 6, 3));

        /*mapData.solars.Add(randomizeTile(19, 19, 2, 1));
        mapData.solars.Add(randomizeTile(19, 19, 2, 1));*/

        //mapData.stadiums.Add(randomizeTile(9, 17, 9, 17));
        tileManager.scrabbleSolar = mapData.scrabbleSolar;

    }



    public string randomizeTile(int xMax, int yMax, int xMin, int yMin)
    {
        int X = Random.Range(xMax, xMin);
        int Y = Random.Range(yMax, yMin);

        string output = X.ToString().PadLeft(3, '0') + "|" + Y.ToString().PadLeft(3, '0');

        return output;
    }

    public PlayerState GetOtherPlayer(PlayerState player)
    {
        return player == player1 ? player2 : player1;
    }

    public static byte[] ObjectToByteArray(object obj)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    public static object ByteArrayToObject(byte[] arrBytes)
    {
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = binForm.Deserialize(memStream);
            return obj;
        }
    }


    void instantiatePopup()
    {
        factPopups = new List<string>();
        factPopups.Add("Fact1");
        factPopups.Add("Fact2");
        factPopups.Add("Fact3");
    }
}
