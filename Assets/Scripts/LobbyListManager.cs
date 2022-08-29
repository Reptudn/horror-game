using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class LobbyListManager : MonoBehaviour
{

    public static LobbyListManager Instance;
    
    public GameObject LobbyList;
    public GameObject LobbyItemPrefab;

    public List<GameObject> Lobbies = new List<GameObject>();


    void Awake()
    {
        if (Instance == null) { Instance = this; }
    }

    public void GetLobbies()
    {

        if (Lobbies.Count > 0){ DestroyLobbies(); }
        SteamLobby.Instance.GetLobbiesList();

    }

    public void DestroyLobbies()
    {

        foreach (GameObject Lobby in Lobbies)
        {

            Destroy(Lobby);

        }
        Lobbies.Clear();

    }

    public void DisplayLobbies(List<CSteamID> LobbyIds, LobbyDataUpdate_t result)
    {

        for (int i = 0; i < LobbyIds.Count; i++)
        {

            if (LobbyIds[i].m_SteamID == result.m_ulSteamIDLobby)
            {
                GameObject LobbyItem = Instantiate(LobbyItemPrefab);
                LobbyItem.GetComponent<LobbyDataEntry>().LobbyId = (CSteamID) LobbyIds[i].m_SteamID;
                LobbyItem.GetComponent<LobbyDataEntry>().LobbyName = SteamMatchmaking.GetLobbyData((CSteamID) LobbyIds[i].m_SteamID, "name");
                LobbyItem.GetComponent<LobbyDataEntry>().MaxPlayers = SteamMatchmaking.GetLobbyMemberLimit((CSteamID) LobbyIds[i].m_SteamID);
                LobbyItem.GetComponent<LobbyDataEntry>().Players = SteamMatchmaking.GetNumLobbyMembers((CSteamID) LobbyIds[i].m_SteamID);
                LobbyItem.GetComponent<LobbyDataEntry>().OwnerId = SteamMatchmaking.GetLobbyOwner((CSteamID) LobbyIds[i].m_SteamID);

                LobbyItem.GetComponent<LobbyDataEntry>().SetLobbyData();

                LobbyItem.transform.SetParent(LobbyList.transform);
                LobbyItem.transform.localScale = Vector3.one;

                Lobbies.Add(LobbyItem);

            }
        }
    }
}
