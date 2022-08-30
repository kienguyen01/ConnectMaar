using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;


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

    public void CreateOrJoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void CreateRoom()
    {
        bool result;

        do {
            result = PhotonNetwork.CreateRoom(CreateRoomName());

        } while (!result);
    }

    private string CreateRoomName()
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var stringChars = new char[8];
        var random = new System.Random();

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        var finalString = new string(stringChars);

        return finalString;
    }



    public void JoinInvRoom(string name)
    {
        PhotonNetwork.JoinRoom(name);
    }

    [PunRPC]
    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(sceneName);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions options = new RoomOptions();
        //Set max players of room to 2 
        options.MaxPlayers = 2;
        //Chreate room
        PhotonNetwork.CreateRoom(null, options);
    }
}
