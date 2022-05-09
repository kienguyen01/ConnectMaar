using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
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
}

[Serializable]
public struct EndTurnData
{
    public List<string> tilesChosen;
    public float totalPoint;
}

public class MultiplayerState : GameState
{
    MapData mapData;
    EndTurnData endTurnData;
    /*public MultiplayerPlayerState player1;
    public MultiplayerPlayerState player2;*/

    //public PlayerState currentPlayer;

    new public static MultiplayerState instance;

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

        Debug.Log("Multiplayer");
        if (PhotonNetwork.IsMasterClient)
        {
            addSpecialTiles();
            photonView.RPC("CreateMap", RpcTarget.All, SerializeData(mapData));
            SetPlayers();
        }
    }

    [PunRPC]
    void DebugTest()
    {
        Debug.LogError(player1 ? "current assigned" : "current empty");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || turnCheck)
        {
            CheckEndTurn();
        }

        if (player1.gameData.isTurn)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0) || clearBtn)
            {
                clearAllSelected(player1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1) || p1)
            {
                SelectSingleConnector(player1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) || p2)
            {
                SelectDoubleConnector(player1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) || p3)
            {
                SelectTripleConnector(player1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) || solarCheck)
            {
                SelectSolarConnector(player1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5) || heatCheck)
            {
                SelectHeatConnector(player1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6) || nodeCheck)
            {
                SelectNodeConnector(player1);
            }

            if (Input.GetMouseButtonDown(0))
            {
                HandleClickCheck();
            }
            
        }

        if (Input.GetMouseButtonDown(0) && !selectedConnector)
            GetInfoCard(player1);
    }

    new protected bool CheckEndTurn()
    {
        photonView.RPC("RPC_Log", RpcTarget.All, $"{PhotonNetwork.LocalPlayer.UserId} --- local player: {player1.gameData.isTurn} - foreign player: {player2.gameData.isTurn}");

        List<Tile> chosenTiles = new List<Tile>(player1.gameData.tilesChosen);
        bool returnObj = base.CheckEndTurn();

        if (returnObj)
        {
            endTurnData.tilesChosen = new List<string>();

            endTurnData.totalPoint = player1.gameData.totalPoint;

            foreach (var item in chosenTiles)
            {
                endTurnData.tilesChosen.Add(item.X.ToString().PadLeft(3, '0') + "|" + item.Y.ToString().PadLeft(3, '0'));
            }

            photonView.RPC("SendTiles", RpcTarget.Others, SerializeData(endTurnData));

            photonView.RPC("EndTurn", RpcTarget.Others);
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

        Debug.LogError($"{player1.gameData.isTurn}");
        Debug.LogError($"{player2.gameData.isTurn}");

        /*
            player1.RefillHand();
            player2.RefillHand();
        */

        photonView.RPC("RPC_HandRefill", RpcTarget.All);

        photonView.RPC("RPC_Log", RpcTarget.All, $"{PhotonNetwork.LocalPlayer.UserId} --- local player: {player1.gameData.isTurn} - foreign player: {player2.gameData.isTurn}");

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

        if (player1 && ((MultiplayerPlayerState)player1).photonPlayer.IsMasterClient)
        {
            TileManager.tiles[14][22].OwnedBy = player2;
            TileManager.tiles[10][16].OwnedBy = player1;
        }
        else
        {
            TileManager.tiles[14][22].OwnedBy = player1;
            TileManager.tiles[10][16].OwnedBy = player2;
        }

    }

    [PunRPC]
    void SetFirstTurn()
    {
        if(((MultiplayerPlayerState)player1).photonPlayer.IsMasterClient)
        {
            player1.gameData.isTurn = true;
            player2.gameData.isTurn = false;
        }
        else
        {
            player1.gameData.isTurn = false;
            player2.gameData.isTurn = true;
        }

        /*if(player1 == MultiplayerPlayerState.me)
        {
            MultiplayerPlayerState.me.BeginTurn();
        }*/
    }

    [PunRPC]
    void EndTurn()
    {
        player1.gameData.isTurn = true;
        player2.gameData.isTurn = false;
        Debug.LogError("turn changed");
        Debug.LogError(player1.gameData.isTurn ? "p1" : "p2");
    }

    [PunRPC]
    void CreateMap(byte[] transferObject)
    {
        MapData mapData = DeserializeData<MapData>(transferObject);
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
                tileManager.instantiateSpecialTile(tile, mapData.churches, mapData.solars, mapData.stadiums, mapData.houses, mapData.scrabbleSolar, mapData.scrabbleHeats, mapData.heats);
            }
        }
    }

    [PunRPC]
    void SendTiles(byte[] transferObject)
    {
        EndTurnData _endTurnData = DeserializeData<EndTurnData>(transferObject);

        foreach(string tileLocation in _endTurnData.tilesChosen)
        {
            var tileCoords = tileLocation.Split('|');
            Tile tile = TileManager.tiles[Convert.ToInt32(tileCoords[0])][Convert.ToInt32(tileCoords[1])];

            if (!(tile.Structure.GetType() == typeof(Node)))
            {
                tile.OwnedBy = player2;
            }
            tile.SelectedBy = null;
           // AssignScrabbleTileRewards(tile);
        }
        player2.gameData.tilesTaken.AddRange(player2.gameData.tilesChosen);
        player2.gameData.totalPoint = _endTurnData.totalPoint;
    }
     
/*    public void AssignScrabbleTileRewards(Tile tile)
    {
        if (tile.IsScrambleForHeat)
        {
            this.AddHeatPipeConnector()
                .AddHeatPipeConnector()
                .AddHeatPipeConnector()
                .AddHeatPipeConnector();
        }
        else if (tile.IsScrambleForSolar)
        {
            this
                .AddSolarConnector()
                .AddSolarConnector()
                .AddSolarConnector()
                .AddSolarConnector();
        }
    }*/

    void addSpecialTiles()
    {
      //  List<>
        //mapData.houses.Add(PickRandomTiles());

        mapData.stadiums.Add(randomizeTile(9, 17, 9, 17));

        mapData.houses.Add(randomizeTile(9, 16, 5, 12));
        mapData.houses.Add(randomizeTile(12, 16, 5, 12));
        mapData.houses.Add(randomizeTile(17, 12, 5, 5));
        mapData.houses.Add(randomizeTile(19, 19, 2, 1));
        mapData.houses.Add(randomizeTile(19, 19, 2, 1));
        mapData.houses.Add(randomizeTile(19, 19, 2, 1));
        mapData.houses.Add(randomizeTile(12, 16, 12, 16));
        mapData.houses.Add(randomizeTile(10, 16, 10, 16));
        mapData.houses.Add(randomizeTile(14, 22, 14, 22));

        mapData.scrabbleSolar.Add(randomizeTile(19, 19, 2, 1));
        mapData.scrabbleSolar.Add(randomizeTile(19, 19, 2, 1));
        mapData.scrabbleSolar.Add(randomizeTile(19, 19, 2, 1));

        mapData.churches.Add(randomizeTile(5, 14, 5, 14));

        mapData.solars.Add(randomizeTile(19, 19, 2, 1));
        mapData.solars.Add(randomizeTile(19, 19, 2, 1));
        mapData.solars.Add(randomizeTile(19, 19, 2, 1));
        mapData.solars.Add(randomizeTile(19, 19, 2, 1));
    }

     


    public string PickRandomTiles(List<string> tiles)
    {
        int x = Random.Range(1, 3);

      //  string output = tiles[x].ToString().PadLeft(3, '');
        return null;
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

    public static byte[] SerializeData<T>(T data)
    {
        Console.WriteLine("Serializing an instance of the object.");
        byte[] bytes;
        using (var stream = new MemoryStream())
        {
            var serializer = new DataContractSerializer(typeof(T));
            serializer.WriteObject(stream, data);
            bytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(bytes, 0, (int)stream.Length);
        }
        return bytes;
    }

    public static T DeserializeData<T>(byte[] data)
    {
        Console.WriteLine("Deserializing an instance of the object.");

        T deserializedThing = default(T);

        using (var stream = new MemoryStream(data))
        using (var reader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas()))
        {
            var serializer = new DataContractSerializer(typeof(T));

            // Deserialize the data and read it from the instance.
            deserializedThing = (T)serializer.ReadObject(reader, true);
        }
        return deserializedThing;
    }
}
