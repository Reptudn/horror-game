using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUD_Handler : MonoBehaviour
{

    public GameObject hud;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!(SceneManager.GetActiveScene().name == "MainMenu")) hud.SetActive(true);
    }
}
