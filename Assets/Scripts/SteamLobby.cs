using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Steamworks;

public class SteamLobby : MonoBehaviour
{

    // Global Instance
    public static SteamLobby Instance;

    // Lobby Filter
    public static string LobbyIdentifier = "EPIS_GAME1864";

    // Callbacks
    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> JoinRequest;
    protected Callback<LobbyEnter_t> LobbyEntered;
    protected Callback<LobbyMatchList_t> LobbyList;
    protected Callback<LobbyDataUpdate_t> LobbyDataUpdated;
    protected Callback<LobbyChatUpdate_t> LobbyChatUpdate;

    // Variables
    public ulong LobbyId;
    [Range(1, 150)]
    public int ServerListLength = 50;
    public List<CSteamID> LobbyIds = new List<CSteamID>();
    private const string HostAddressKey = "HostAddress";
    private NexNetworkManager manager;

    // Gameobjects
    public GameObject MainScreen;
    public GameObject LobbyScreen;
    public GameObject LobbyListScreen;

    [Range(1, 100)]
    public int PlayerPerLobby = 16;

    // Enums
    public enum EChatMemberStateChange {
        k_EChatMemberStateChangeEntered = 0x0001,
        k_EChatMemberStateChangeLeft = 0x0002,
        k_EChatMemberStateChangeDisconnected = 0x0004,
        k_EChatMemberStateChangeKicked = 0x0008,
        k_EChatMemberStateChangeBanned = 0x0010,
    }

    private void Start() 
    {
        if (!SteamManager.Initialized) { return; }
        if (Instance == null) { Instance = this; }

        manager = GetComponent<NexNetworkManager>();

        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        JoinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

        LobbyList = Callback<LobbyMatchList_t>.Create(OnGetLobbyList);
        LobbyDataUpdated = Callback<LobbyDataUpdate_t>.Create(OnGetLobbyData);
        LobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(OnLobbyChatUpdate);

    }

    public void HostLobby()
    {

        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, PlayerPerLobby);

    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK) { return; }
        Debug.Log("Lobby created.");

        manager.StartHost();

        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey, SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name", LobbyIdentifier + SteamFriends.GetPersonaName().ToString() + "'s Lobby");

    }

    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        Debug.Log("Request To Join Lobby");
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {

        MainScreen.SetActive(false);
        LobbyScreen.SetActive(true);
        LobbyListScreen.SetActive(false);
        LobbyId = callback.m_ulSteamIDLobby;

        // Client Only

        if (NetworkServer.active) { return; }

        manager.networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);
        manager.StartClient();

    }

    public void GetLobbiesList()
    {
        if (LobbyIds.Count > 0) { LobbyIds.Clear(); }

        Debug.Log("Getting Lobbies");
        SteamMatchmaking.AddRequestLobbyListResultCountFilter(60);
        SteamMatchmaking.AddRequestLobbyListStringFilter("name", LobbyIdentifier, ELobbyComparison.k_ELobbyComparisonEqualToOrGreaterThan);
        SteamMatchmaking.RequestLobbyList();

    }

    private void OnGetLobbyList(LobbyMatchList_t result)
    {
        if (LobbyListManager.Instance.Lobbies.Count > 0) { LobbyListManager.Instance.DestroyLobbies(); }

        for (int i = 0; i < result.m_nLobbiesMatching; i++)
        {
            CSteamID LobbyId = SteamMatchmaking.GetLobbyByIndex(i);
            LobbyIds.Add(LobbyId);
            SteamMatchmaking.RequestLobbyData(LobbyId);
        }

    }

    private void OnGetLobbyData(LobbyDataUpdate_t result)
    {
        LobbyListManager.Instance.DisplayLobbies(LobbyIds, result);
    }

    private void OnLobbyChatUpdate(LobbyChatUpdate_t update)
    {
        switch(update.m_rgfChatMemberStateChange)
        {
            case (uint) SteamLobby.EChatMemberStateChange.k_EChatMemberStateChangeLeft:

                LobbyController.Instance.UpdatePlayerList();
                break;
        }
    }

    public void LeaveLobby(CSteamID LobbyId)
    {
        MainScreen.SetActive(true);
        LobbyScreen.SetActive(false);
        LobbyListScreen.SetActive(false);
        
        SteamMatchmaking.LeaveLobby(LobbyId);
    }

    public void LeaveLobby()
    {
        LeaveLobby((CSteamID) LobbyId);
        LobbyController.Instance.Leave();
    }

    public void JoinLobby(CSteamID LobbyId)
    {
        SteamMatchmaking.JoinLobby(LobbyId);
    }

}
