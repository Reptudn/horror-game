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
    public GameObject LobbyCameraAnchor;
    public GameObject MainCameraAnchor;


    // Camera Animation

    private Vector3 CurrentEndPosition;
    private Quaternion CurrentEndOrientation;

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

    public void HostPrivateLobby()
    {

        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePrivate, PlayerPerLobby);

    }

    public void HostFriendsOnlyLobby()
    {

        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, PlayerPerLobby);

    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK) { return; }
        Debug.Log("Lobby created.");

        manager.StartHost();

        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey, SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name", SteamFriends.GetPersonaName().ToString() + "'s Lobby");
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "identifier", LobbyIdentifier);

    }

    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        Debug.Log("Request To Join Lobby");
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {

        MainScreen.SetActive(false);
        StartCoroutine(TransitionCameraTo(LobbyCameraAnchor.transform.position, LobbyCameraAnchor.transform.rotation));
        /*LobbyScreen.SetActive(true);*/
        
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
        SteamMatchmaking.AddRequestLobbyListStringFilter("identifier", LobbyIdentifier, ELobbyComparison.k_ELobbyComparisonEqual);
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

        StartCoroutine(TransitionCameraTo(MainCameraAnchor.transform.position, MainCameraAnchor.transform.rotation));
        
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


    private IEnumerator TransitionCameraTo(Vector3 EndPos, Quaternion EndRot)
    {

        CurrentEndPosition = EndPos;
        CurrentEndOrientation = EndRot;

        float ElapsedTime = 0;
        float WaitTime = 20f;
 
        while (ElapsedTime < WaitTime && CurrentEndPosition == EndPos && CurrentEndOrientation == EndRot)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, EndPos, (ElapsedTime / WaitTime));
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, EndRot, (ElapsedTime / WaitTime));
            ElapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return null;

    }

}
