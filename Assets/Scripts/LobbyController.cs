using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{
    
    public static LobbyController Instance;

    // GameObjects
    public TextMeshProUGUI LobbyNameText;
    public Button StatusButton;
    public TextMeshProUGUI StatusButtonText;
    public GameObject LoadingScreen;
    
    // Player Data
    public GameObject PlayerListViewContent;
    public GameObject PlayerListItemPrefab;
    public GameObject LocalPlayerObject;

    // Misc
    public ulong LobbyId;
    public bool PlayerItemCreated = false; 

    private List<PlayerListItem> Players = new List<PlayerListItem>();
    
    public PlayerObjectController LocalPlayerController;

    // Manager
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

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    }

    public void PlayerStatus()
    {
        LocalPlayerController.ChangeStatus();
    }

    public void UpdateStatusButton()
    {

        if (LocalPlayerController.PlayerId == 1){
            StatusButtonText.SetText("Start");
        }
        else if (LocalPlayerController.PlayerStatus)
        {
            StatusButtonText.SetText("Not Ready");
        }
        else
        {
            StatusButtonText.SetText("Ready");
        }

    }

    public void UpdateLobbyName()
    {
        LobbyId = Manager.GetComponent<SteamLobby>().LobbyId;
        LobbyNameText.SetText(SteamMatchmaking.GetLobbyData(new CSteamID(LobbyId), "name").Split(SteamLobby.LobbyIdentifier)[1]);
    }

    public void UpdatePlayerList()
    {

        if (!PlayerItemCreated) { CreatePlayerItem(true); }
        if (Players.Count < Manager.Players.Count) { CreatePlayerItem(false); Debug.Log("CreatePlayerItem"); }
        if (Players.Count > Manager.Players.Count) { RemovePlayerItem(); Debug.Log("RemovePlayerItem"); }
        if (Players.Count == Manager.Players.Count) { UpdatePlayerItem(); Debug.Log("UpdatePlayerItem"); }

    }

    public void FindLocalPlayer()
    {
        LocalPlayerObject = GameObject.Find("LocalGamePlayer");
        LocalPlayerController = LocalPlayerObject.GetComponent<PlayerObjectController>();
    }

    public void CreatePlayerItem(bool isHost)
    {
        foreach (PlayerObjectController Player in Manager.Players)
        {

            if (!Players.Any(b => b.ConnectionId == Player.ConnectionId))
            {

                GameObject PlayerItem = Instantiate(PlayerListItemPrefab) as GameObject;
                PlayerListItem NewPlayerItem = PlayerItem.GetComponent<PlayerListItem>();

                NewPlayerItem.PlayerName = Player.PlayerName;
                NewPlayerItem.ConnectionId = Player.ConnectionId;
                NewPlayerItem.PlayerSteamId = Player.PlayerSteamId;
                NewPlayerItem.PlayerStatus = Player.PlayerStatus;
                NewPlayerItem.SetPlayerValues();

                PlayerItem.transform.SetParent(PlayerListViewContent.transform);
                PlayerItem.transform.localScale = Vector3.one;

                Players.Add(NewPlayerItem);

            }
        }

        PlayerItemCreated = true;

    }

    public void RemovePlayerItem()
    {

        List<PlayerListItem> Remove = new List<PlayerListItem>();

        foreach (PlayerListItem Player in Players)
        {

            if (!Manager.Players.Any(b => b.ConnectionId == Player.ConnectionId))
            {

                Remove.Add(Player);

            }

        }

        if (Remove.Count > 0)
        {

            foreach (PlayerListItem Player in Remove)
            {

                GameObject ObjectToRemove = Player.gameObject;
                Players.Remove(Player);
                Destroy(ObjectToRemove);
                ObjectToRemove = null;

            }
        }
    }

    public void UpdatePlayerItem()
    {

        foreach (PlayerObjectController Player in Manager.Players)
        {

            foreach (PlayerListItem PlayerListItem in Players)
            {

                if (PlayerListItem.ConnectionId == Player.ConnectionId)
                {

                    PlayerListItem.PlayerName = Player.PlayerName;
                    PlayerListItem.PlayerStatus = Player.PlayerStatus;
                    PlayerListItem.PlayerId = Player.PlayerId;
                    PlayerListItem.SetPlayerValues();

                    if (Player == LocalPlayerController)
                    {
                        UpdateStatusButton();
                    }

                }
            }
        }

        CheckIfAllReady();

    }

    public bool CheckIfAllReady()
    {

        bool AllReady = true;
        
        foreach (PlayerObjectController Player in Manager.Players)
        {

            if (!Player.PlayerStatus && Player.PlayerId != 1){

                AllReady = false;
                break;

            }
        }

        if (LocalPlayerController != null && LocalPlayerController.PlayerId == 1)
        {
            if (AllReady)
            {
                StatusButton.interactable = true;
            }
            else
            {
                StatusButton.interactable = false;
            }
        }

        return AllReady;

    }

    public void StartGame(string SceneName)
    {
        LocalPlayerController.CanStartGame(SceneName);
    }

    public void Leave()
    {
        RemovePlayerItem();
    }

    public void LoadScene()
    {
        Debug.Log("LOADING SCENE");
        LoadingScreen.SetActive(true);
    }

}
