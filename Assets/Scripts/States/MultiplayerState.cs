using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiplayerState : GameState, INetworkRunnerCallbacks
{
    private NetworkRunner _runner;

    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.GetComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneObjectProvider = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            PlayerCount = 2
        });
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_FinalizeConnection(PlayerState player, Connection conn)
    {
        player.FinalizeConnection(conn);
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public RpcInvokeInfo RPC_EndTurn(PlayerState player)
    {
        if(player.MultiplayerSessionID == _runner.LocalPlayer.PlayerId)
        {
            //playerStates[0].EndTurn();
            Debug.LogError($"RPC LOCAL --- PlayerID: {player.MultiplayerSessionID}");
        }
        else
        {
            Debug.LogError($"RPC Foreign --- PlayerID: {player.MultiplayerSessionID}");
            //dothingswhenYourTurn begins
        }
        return default;
    }

    new public void ClickChecks()
    {
        base.ClickChecks();

        //other code;
    }

    new protected void TurnEndCheck()
    {
        base.TurnEndCheck();

        RPC_EndTurn(this.playerStates[0]);
    }

    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                StartGame(GameMode.Shared);
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                StartGame(GameMode.Shared);
            }
        }
    }

    //[SerializeField] private NetworkPrefabRef _playerPrefab;
    private List<PlayerRef> Players = new List<PlayerRef>();

    void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.LogError($"{player.PlayerId} has just joined");
    }

    void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.LogError($"{player.PlayerId} has just left");
    }

    void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input)
    {
        //throw new NotImplementedException();

        Debug.LogError($"RPC INFO: {RPC_EndTurn(playerStates[0])}");
        Debug.LogError($"Lobby INFO: {_runner.SessionInfo}");

        if (playerStates[0].gameData.isTurn)
        {
            Debug.LogError("0 did something");
        }


        var data = new NetworkInputData();

        if (Input.GetKey(KeyCode.G))
        {
            data.message = "G pressed";
        }

        //input.Set(data);
    }

    void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        //throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        //throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner)
    {
        Debug.LogWarning($"Server Joined -- {runner.SessionInfo.Name}");

        playerStates[0].MultiplayerSessionID = runner.LocalPlayer.PlayerId;
    }

    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner)
    {
        //throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        //throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        //throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        //throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        //throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        //throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        //throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        //throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner)
    {
        //throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner)
    {
        //throw new NotImplementedException();
    }
}