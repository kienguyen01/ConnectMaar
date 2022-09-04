using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviourPunCallbacks
{
    [Header("Screen")]
    public GameObject mainScreen;
    public GameObject lobbyScreen;

    [Header("Main Screen")]
    public Button playButton;

    [Header("Lobby Screen")]
    public TextMeshProUGUI player1NameText;
    public TextMeshProUGUI player2NameText;
    public TextMeshProUGUI gameStartingText;

    [Header("Matchmaking")]
    public TextMeshProUGUI MatchmakingStatusText;

    private bool masterConn;
    private bool matchMakingStatus;

    

    private void Start()
    {
        //Payer did not connect 
        playButton.interactable = false;
        gameStartingText.gameObject.SetActive(false);
        matchMakingStatus = false;
    }

    public override void OnConnectedToMaster()
    {

        playButton.interactable = true;
        if (masterConn)
        {
            matchMakingStatus = true;
            NetManager.instance.CreateOrJoinRoom();
            masterConn = false;
            
        }
       // masterConn = false;
    }

    public void SetScreen(GameObject screen)
    {
        //Disable asll screen
        mainScreen.SetActive(false);
        lobbyScreen.SetActive(false);

        //Enable requested screen
        screen.SetActive(true);

    }

    public void OnUpdatePlayerInput(TMP_InputField nameInput)
    {
        PhotonNetwork.NickName = nameInput.text;
    }

    public void OnplayButton()
    {
        Debug.Log("This is the Instance room name: " + ProcessDeepLinkMngr.Instance.roomName);
        if(ProcessDeepLinkMngr.Instance.active == false)
        {
            NetManager.instance.CreateRoom();
            //NetManager.instance.CreateOrJoinRoom();
        }
        else
        {
            NetManager.instance.JoinInvRoom(ProcessDeepLinkMngr.Instance.roomName);
        }
    }

/*    public void OnplayButton()
    {
            NetManager.instance.CreateOrJoinRoom();
    }*/

    public override void OnJoinedRoom()
    {
        SetScreen(lobbyScreen);
        if (!matchMakingStatus)
        {
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }
        //update the ui of all players in the loby
        photonView.RPC("updateLobbyUI", RpcTarget.All);
    }

    public void CreateInviteLink()
    {
        new NativeShare().SetSubject("Game Invite").SetText(player1NameText.text + " would like to invite you to a game").SetUrl("https://connectmaar?" + PhotonNetwork.CurrentRoom.Name).Share();
        // GUIUtility.systemCopyBuffer = "https://connectmaar?"+PhotonNetwork.CurrentRoom.Name;
    }

    [PunRPC]
    void updateLobbyUI()
    {
        player1NameText.text = PhotonNetwork.CurrentRoom.GetPlayer(1).NickName;
        player2NameText.text = PhotonNetwork.PlayerList.Length == 2?PhotonNetwork.CurrentRoom.GetPlayer(2).NickName : "...";

        Debug.Log(PhotonNetwork.CurrentRoom.Name);

        

        //Set the game startign text
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            gameStartingText.gameObject.SetActive(true);

            bool _success = PhotonNetwork.SetMasterClient(PhotonNetwork.CurrentRoom.GetPlayer(1));
            Debug.LogWarning($"Local player Nickname: {PhotonNetwork.LocalPlayer.NickName}, Actor Number: {PhotonNetwork.LocalPlayer.ActorNumber}, IsMasyer: {PhotonNetwork.LocalPlayer.IsMasterClient}, UserID: {PhotonNetwork.LocalPlayer.UserId}");


            //Masterclient allow to call to try and start the game function
            if (PhotonNetwork.IsMasterClient && _success)
            {
                Invoke("TryStartGame", 4.0f);
            }
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        updateLobbyUI();
    }

    void TryStartGame()
    {
        if(PhotonNetwork.PlayerList.Length == 2)
        {
            NetManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "MultiplayerMap");
        }
        else
        {
            gameStartingText.gameObject.SetActive(false);
        }
    }   

    [PunRPC]
    public void OnLeaveButton()
    {
        matchMakingStatus = false;
        PhotonNetwork.LeaveRoom();
        SetScreen(mainScreen);
    }

    public void MatchmakingStatus()
    {
        Debug.LogError(PhotonNetwork.CurrentRoom);

        

        if (PhotonNetwork.CurrentRoom.IsVisible == false)
        {
            Debug.LogError("BEFORE: "+PhotonNetwork.CurrentRoom);
            //masterConn = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LeaveRoom(false);
            masterConn=true;
            //SceneManager.LoadScene("Lobby");

            MatchmakingStatusText.text = "Searching";
            //NetManager.instance.JoinInvRoom();
        }
        else
        {
            PhotonNetwork.CurrentRoom.IsVisible = false;
            matchMakingStatus = false;
            MatchmakingStatusText.text = "Matchmake";
        }
    }





}
