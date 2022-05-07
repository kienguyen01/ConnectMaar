using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetManager : MonoBehaviourPunCallbacks
{
    //Singleton
    public static NetManager instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //Conect to master server
        PhotonNetwork.ConnectUsingSettings();
    }

    /*Check if connection to photon is sucessful
     * public override void OnConnectedToMaster()
    {
        Debug.Log("Connected");
    }*/

       

    public void CreateOrJoinRoom()
    {
        if(PhotonNetwork.CountOfRooms > 0)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            RoomOptions options = new RoomOptions();
            //Set max players of room to 2 
            options.MaxPlayers = 2;
            //Chreate room
            PhotonNetwork.CreateRoom(null, options);

        }
    }

    [PunRPC]
    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(sceneName);
    }

    





}
