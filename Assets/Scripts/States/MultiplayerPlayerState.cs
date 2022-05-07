using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayerPlayerState : PlayerState
{

    public Photon.Realtime.Player photonPlayer;

    public static PlayerState me;
    public static PlayerState enemy;

    [PunRPC]

    private void Initialize(Photon.Realtime.Player player)
    {
        photonPlayer = player;

        if (player.IsLocal)
        {
            me = this;
            me.RefillHand();
        }
        else
        {
            enemy = this;
        }
    }





    
    
        
    
}