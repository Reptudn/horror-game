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

    }

    public void HostLobby()
    {

        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, manager.maxConnections);

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
    }

    public void JoinLobby(CSteamID LobbyId)
    {
        SteamMatchmaking.JoinLobby(LobbyId);
    }

}
