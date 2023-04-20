using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUtils : MonoBehaviour
{

    public static GameObject? GetPlayerById(ulong id)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            var playerController = p.GetComponent<PlayerObjectController>();
            if (playerController != null)
            {
                if (playerController.PlayerSteamId == id) return p;
            }
        }
        return null;
    }

    public static GameObject[] GetPlayers()
    {
        return GameObject.FindGameObjectsWithTag("Player");
    }

}
