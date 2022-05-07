using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class MultiplayerPlayerState : PlayerState
{

    public Photon.Realtime.Player photonPlayer;





    public static MultiplayerPlayerState me;
    public static MultiplayerPlayerState enemy;

    //Maybe delete just for testing
    public TMP_Text playerText;
    public Image playerEmissionbar;
    float lerpSpeed; //making sure increase and decrease is smooth
    float currentEmission, maxEmission;




    [PunRPC]
    private void Initialize(Photon.Realtime.Player player)
    {
        photonPlayer = player;

        if (player.IsLocal)
        {
            me = this;
            TileManager.tiles[12][16].OwnedBy = me;
            me.gameData.PlayerColour = Color.blue;

            //me.RefillHand();
        }
        else
        {
            enemy = this;
            TileManager.tiles[12][15].OwnedBy = enemy;
            me.gameData.PlayerColour = Color.black;

        }
    }

    void StartingPoint()
    {

    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if(Input.GetMouseButtonDown(0) && MultiplayerState.instance.currentPlayer == this)
        {

        }
    }

    public void BeginTurn()
    {
        me.gameData.isTurn = true;
    }
    
    [PunRPC]
    void reduceEmmision(int connCount)
    {
        float amount = gameData.totalPoint -= 5 / connCount;
        if(gameData.totalPoint <= 0)
        {
            photonView.RPC("Win", RpcTarget.All);
        }
        else
        {
            photonView.RPC("UpdateEmissionBar",RpcTarget.All, amount);
        }

    }

    [PunRPC]
    void UpdateEmissionBar(float ammount)
    {
        currentEmission = gameData.totalPoint;
        playerText.text = currentEmission + "%"; //displaying the percentage
                                                 //check if currentEmission > maxEmission, then currentemission = maxEmission (currentEmission !> 100)
        if (currentEmission > maxEmission)
        {
            currentEmission = maxEmission;
        }

        lerpSpeed = 3f * Time.deltaTime; //can be changed to increase or decrease lerp speed

        playerEmissionbar.fillAmount = Mathf.Lerp(playerEmissionbar.fillAmount, currentEmission / maxEmission, lerpSpeed);
    }

    [PunRPC]
    void Win()
    {

    }
}
