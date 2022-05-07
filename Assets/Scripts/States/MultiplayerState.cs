using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerState : GameState
{

    public PlayerState player1;
    public PlayerState player2;

    public PlayerState currentPlayer;

    private void Start()
    {
        Debug.Log("Muyltiplayer");
        if (PhotonNetwork.IsMasterClient)
        {
            SetPlayers();
        }
    }

    void SetPlayers()
    {
        //player1.pho
    }

}
