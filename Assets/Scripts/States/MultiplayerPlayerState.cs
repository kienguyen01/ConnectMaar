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
    //GameObject PlaneObj;


    public string[] sdtatingPoints;

    //Maybe delete just for testing
    public TMP_Text playerText;
    public Image playerEmissionbar;
    float lerpSpeed; //making sure increase and decrease is smooth
    float currentEmission, maxEmission;

    /*void StartingPoint()
    {
            GameObject plane = PhotonNetwork.Instantiate("Plane",PlaneObj.transform.position,PlaneObj.transform.rotation);

            plane.GetPhotonView().RPC("Initialize", RpcTarget.Others, false);
            plane.GetPhotonView().RPC("Initialize", photonPlayer,true);
    }*/

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        //if(Input.GetMouseButtonDown(0) && MultiplayerState.instance.currentPlayer == this)
        //{
        //    Debug.Log("It is my turn");
        //}
    }

/*    public void SetStartPoint(MultiplayerPlayerState player)
    {
        if(TileManager.tiles[10][16].OwnedBy == null)
        {
            TileManager.tiles[10][16].OwnedBy = player;
            me.gameData.PlayerColour = Color.red;
            //photonView.RPC()
        }
        else
        {
            player.gameData.PlayerColour = Color.blue;
            TileManager.tiles[15][15].OwnedBy = player;
        }
    }*/

    public void BeginTurn()
    {
        
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
