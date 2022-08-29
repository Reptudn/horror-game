using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using TMPro;

public class LobbyDataEntry : MonoBehaviour
{
    [Header("Lobby Data")]
    public CSteamID LobbyId;
    public string LobbyName;
    public int MaxPlayers;
    public int Players;
    public CSteamID OwnerId;

    [Header("UI Elements")]
    public TextMeshProUGUI LobbyNameText;
    public TextMeshProUGUI PlayerCountText;

    public void SetLobbyData()
    {

        LobbyNameText.SetText(LobbyName.Length != 0 ? (LobbyName.Split(SteamLobby.LobbyIdentifier).Length > 1 ? LobbyName.Split(SteamLobby.LobbyIdentifier)[1] : LobbyName) : "Unnamed Lobby");
        PlayerCountText.SetText(string.Format("({0}/{1})", Players, MaxPlayers != 0 ? MaxPlayers : "--"));
    }

    public void JoinLobby()
    {

        SteamLobby.Instance.JoinLobby(LobbyId);

    }

}
