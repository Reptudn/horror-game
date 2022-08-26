using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class NexNetworkManager : NetworkManager
{
    
    [SerializeField] private PlayerObjectController GamePlayerPrefab;
    public List<PlayerObjectController> Players { get; } = new List<PlayerObjectController>();

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {

        PlayerObjectController PlayerInstance = Instantiate(GamePlayerPrefab);
        PlayerInstance.ConnectionId = conn.connectionId;
        PlayerInstance.PlayerId = Players.Count + 1;
        PlayerInstance.PlayerSteamId = (ulong) SteamMatchmaking.GetLobbyMemberByIndex((CSteamID) SteamLobby.Instance.LobbyId, Players.Count);

        NetworkServer.AddPlayerForConnection(conn, PlayerInstance.gameObject);

    }

}
