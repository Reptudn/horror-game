using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using TMPro;

public class PlayerObjectController : NetworkBehaviour
{
    [SyncVar] public int ConnectionId;
    [SyncVar] public int PlayerId;
    [SyncVar] public ulong PlayerSteamId;
    [SyncVar(hook = nameof(PlayerNameUpdate))] public string PlayerName;
    [SyncVar(hook = nameof(PlayerStatusUpdate))] public bool PlayerStatus;

    private NexNetworkManager manager;
    private NexNetworkManager Manager
    {
        get
        {
            if (manager != null)
            {
                return manager;
            }
            return manager = NexNetworkManager.singleton as NexNetworkManager;
        }
    }

    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnStartAuthority()
    {
        Cmd_SetPlayerName(SteamFriends.GetPersonaName().ToString());
        gameObject.name = "LocalGamePlayer";
        LobbyController.Instance.FindLocalPlayer();
        LobbyController.Instance.UpdateLobbyName();
    }

    public override void OnStartClient()
    {
        Manager.Players.Add(this);
        LobbyController.Instance.UpdateLobbyName();
        LobbyController.Instance.UpdatePlayerList();
    }

    public override void OnStopClient()
    {
        Manager.Players.Remove(this);
        LobbyController.Instance.UpdatePlayerList();
    }

    [Command]
    private void Cmd_SetPlayerName(string PlayerName)
    {
        this.PlayerNameUpdate(this.PlayerName, PlayerName);
    }

    [Command]
    private void Cmd_SetPlayerStatus()
    {
        this.PlayerStatusUpdate(this.PlayerStatus, !this.PlayerStatus);
    }

    public void ChangeStatus()
    {
        if (hasAuthority)
        {
            Cmd_SetPlayerStatus();
        }
    }

    public void PlayerNameUpdate(string OldValue, string NewValue)
    {

        if (isServer)
        {
            this.PlayerName = NewValue;
        }
        if (isClient)
        {
            LobbyController.Instance.UpdatePlayerList();
        }

    }

    private void PlayerStatusUpdate(bool OldValue, bool NewValue)
    {
        if (isServer)
        {
            this.PlayerStatus = NewValue;
        }
        if (isClient)
        {
            LobbyController.Instance.UpdatePlayerList();
        }
    }


    public void CanStartGame(string SceneName)
    {


        if (hasAuthority && LobbyController.Instance.CheckIfAllReady())
        {
            Cmd_CanStartGame(SceneName);
        }
    }

    [Command]
    public void Cmd_CanStartGame(string SceneName)
    {
        Manager.StartGame(SceneName);
    }

}
