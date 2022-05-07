using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerState : GameState
{

    public MultiplayerPlayerState player1;
    public MultiplayerPlayerState player2;

    public PlayerState currentPlayer;

    public static MultiplayerState instance;

    private void Start()
    {
        Debug.Log("Multiplayer");
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("CreateMap", RpcTarget.AllBuffered);
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
    void CreateMap()
    {
        tileManager.CreateMultiplayerMap();
    }
       
    public MultiplayerPlayerState GetOtherPlayer(MultiplayerPlayerState player)
    {
        return player == player1 ? player2 : player1;
    }
}
