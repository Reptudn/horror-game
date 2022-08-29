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

    public void StartGame(string SceneName)
    {
        ServerChangeScene(SceneName);
    }

    public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
    {
        LobbyController.Instance.LoadScene();
        base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);
    }

    public override void OnServerChangeScene(string newSceneName)
    {
        Debug.Log("OnServerChangeScene");
        base.OnServerChangeScene(newSceneName);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        Debug.Log("OnServerSceneChanged");
        base.OnServerSceneChanged(sceneName);
    }


}
