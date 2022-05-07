using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

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
public class MultiplayerState : GameState
{
    MapData mapData;
    public MultiplayerPlayerState player1;
    public MultiplayerPlayerState player2;

    public PlayerState currentPlayer;

    public static MultiplayerState instance;



    private void Start()
    {
        mapData = new MapData();
       
        Debug.Log("Multiplayer");
        if (PhotonNetwork.IsMasterClient)
        {
            addSpecialTiles();
            photonView.RPC("CreateMap", RpcTarget.All, mapData);
            SetPlayers();
        }
    }

    void SetPlayers()
    {
        player1.photonView.TransferOwnership(1);
        player2.photonView.TransferOwnership(2);

        player1.photonView.RPC("Initialize", RpcTarget.AllBuffered, PhotonNetwork.CurrentRoom.GetPlayer(1));
        player2.photonView.RPC("Initialize", RpcTarget.AllBuffered, PhotonNetwork.CurrentRoom.GetPlayer(2));

        Debug.LogError($"{player1.photonPlayer.NickName}");
        Debug.LogError($"{player2.photonPlayer.NickName}");

        photonView.RPC("SetNextTurn", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void SetNextTurn()
    {
        if(currentPlayer == null)
        {
            currentPlayer = player1;
            player1.gameData.isTurn = true;
        }
        else if(currentPlayer == player1)
        {
            currentPlayer = player2;
            player2.gameData.isTurn = true;
            player1.gameData.isTurn = false;
        }else if(currentPlayer == player2)
        {
            currentPlayer = player1;
            player1.gameData.isTurn = true;
            player2.gameData.isTurn = false;
        }

        if(currentPlayer == MultiplayerPlayerState.me)
        {
            MultiplayerPlayerState.me.BeginTurn();
        }
    }

    [PunRPC]
    void CreateMap(MapData mapData)
    {
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
       
    void addSpecialTiles()
    {
        mapData.stadiums.Add(randomizeTile(9, 17, 9, 17));

        mapData.houses.Add(randomizeTile(9, 16, 5, 12));
        mapData.houses.Add(randomizeTile(12, 16, 5, 12));
        mapData.houses.Add(randomizeTile(17, 12, 5, 5));
        mapData.houses.Add(randomizeTile(19, 19, 2, 1));
        mapData.houses.Add(randomizeTile(19, 19, 2, 1));
        mapData.houses.Add(randomizeTile(19, 19, 2, 1));
        mapData.houses.Add(randomizeTile(12, 16, 12, 16));
        mapData.houses.Add(randomizeTile(10, 16, 10, 16));
        mapData.houses.Add(randomizeTile(15, 15, 15, 15));


        mapData.scrabbleSolar.Add(randomizeTile(19, 19, 2, 1));
        mapData.scrabbleSolar.Add(randomizeTile(19, 19, 2, 1));
        mapData.scrabbleSolar.Add(randomizeTile(19, 19, 2, 1));

        mapData.churches.Add(randomizeTile(5, 14, 5, 14));

        mapData.solars.Add(randomizeTile(19, 19, 2, 1));
        mapData.solars.Add(randomizeTile(19, 19, 2, 1));
        mapData.solars.Add(randomizeTile(19, 19, 2, 1));
        mapData.solars.Add(randomizeTile(19, 19, 2, 1));
    }




    public string randomizeTile(int xMax, int yMax, int xMin, int yMin)
    {
        int X = Random.Range(xMax, xMin);
        int Y = Random.Range(yMax, yMin);

        string output = X.ToString().PadLeft(3, '0') + "|" + Y.ToString().PadLeft(3, '0');

        return output;
    }
    public MultiplayerPlayerState GetOtherPlayer(MultiplayerPlayerState player)
    {
        return player == player1 ? player2 : player1;
    }
}
