using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerState : GameState
{

    public MultiplayerPlayerState player1;
    public MultiplayerPlayerState player2;

    public PlayerState currentPlayer;

    private void Start()
    {
        Debug.Log("Multiplayer");
        if (PhotonNetwork.IsMasterClient)
        {
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
        }
        else
        {
            currentPlayer = currentPlayer == player1 ? player2 : player1;
        }

        if(currentPlayer == MultiplayerPlayerState.me)
        {
            MultiplayerPlayerState.me.BeginTurn();
        }
    }
       
    public MultiplayerPlayerState GetOtherPlayer(MultiplayerPlayerState player)
    {
        return player == player1 ? player2 : player1;
    }
}
