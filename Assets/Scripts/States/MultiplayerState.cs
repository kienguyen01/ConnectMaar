using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MultiplayerState : GameState
{
    List<(List<string> Houses, List<string> Stadiums, List<string> Churches, List<string> Solars, List<string> Heats, List<string> ScrabblesSolar, List<string> ScrabblesHeat)> randomizedTiles;
    public MultiplayerPlayerState player1;
    public MultiplayerPlayerState player2;

    public PlayerState currentPlayer;

    public static MultiplayerState instance;



    private void Start()
    {
        
       
        Debug.Log("Multiplayer");
        if (PhotonNetwork.IsMasterClient)
        {
            randomizedTiles = new List<(List<string> Houses, List<string> Stadiums, List<string> Churches, List<string> Solars, List<string> Heats, List<string> ScrabblesSolar, List<string> ScrabblesHeat)>
            {(
            tileManager.allHouses,
            tileManager.allStadiums,
            tileManager.allChurches,
            tileManager.allSolar,
            tileManager.allHeat,
            tileManager.scrambleSolar,
            tileManager.scrambleHeat
            )};
            addSpecialTiles();
            photonView.RPC("CreateMap", RpcTarget.All, randomizedTiles);
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
    void CreateMap(List<(List<string> Houses, List<string> Stadiums, List<string> Churches, List<string> Solars, List<string> Heats, List<string> ScrabblesSolar, List<string> ScrabblesHeat)> randomTiles)
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
                tileManager.instantiateSpecialTile(tile, randomTiles[0].Churches, randomTiles[0].Solars, randomTiles[0].Stadiums, randomTiles[0].Houses, randomTiles[0].ScrabblesSolar, randomTiles[0].ScrabblesHeat, randomTiles[0].Heats);
            }
        }
    }
       
    void addSpecialTiles()
    {
        randomizedTiles[0].Stadiums.Add(randomizeTile(9, 17, 9, 17));

        randomizedTiles[0].Houses.Add(randomizeTile(9, 16, 5, 12));
        randomizedTiles[0].Houses.Add(randomizeTile(12, 16, 5, 12));
        randomizedTiles[0].Houses.Add(randomizeTile(17, 12, 5, 5));
        randomizedTiles[0].Houses.Add(randomizeTile(19, 19, 2, 1));
        randomizedTiles[0].Houses.Add(randomizeTile(19, 19, 2, 1));
        randomizedTiles[0].Houses.Add(randomizeTile(19, 19, 2, 1));
        randomizedTiles[0].Houses.Add(randomizeTile(12, 16, 12, 16));
        randomizedTiles[0].Houses.Add(randomizeTile(10, 16, 10, 16));
        randomizedTiles[0].Houses.Add(randomizeTile(15, 15, 15, 15));


        randomizedTiles[0].ScrabblesSolar.Add(randomizeTile(19, 19, 2, 1));
        randomizedTiles[0].ScrabblesSolar.Add(randomizeTile(19, 19, 2, 1));
        randomizedTiles[0].ScrabblesSolar.Add(randomizeTile(19, 19, 2, 1));

        randomizedTiles[0].Churches.Add(randomizeTile(5, 14, 5, 14));

        randomizedTiles[0].Solars.Add(randomizeTile(19, 19, 2, 1));
        randomizedTiles[0].Solars.Add(randomizeTile(19, 19, 2, 1));
        randomizedTiles[0].Solars.Add(randomizeTile(19, 19, 2, 1));
        randomizedTiles[0].Solars.Add(randomizeTile(19, 19, 2, 1));
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
