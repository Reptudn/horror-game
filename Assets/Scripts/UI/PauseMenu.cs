using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [Header("Main")]
    public GameObject pauseMenuContainer;

    [Header("Other Components")]
    public GameObject crosshairContainer;
    public GameObject interactionContainer;

    public PlayerController playerController;
    public KeyCode pauseMenuKey = KeyCode.Escape;

    bool opened = false;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenuContainer.SetActive(false);
        crosshairContainer.SetActive(true);
        interactionContainer.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(SceneManager.GetActiveScene().name == "MainMenu") return;

        if(Input.GetKeyDown(pauseMenuKey)){

            crosshairContainer.SetActive(!crosshairContainer.activeSelf);
            interactionContainer.SetActive(!interactionContainer.activeSelf);

            pauseMenuContainer.SetActive(!pauseMenuContainer.activeSelf);

            opened = !opened;
            playerController.paused = opened;

            if(opened) Cursor.lockState = CursorLockMode.None;
            else Cursor.lockState = CursorLockMode.Locked;

        }

    }

    public void Resume(){

        crosshairContainer.SetActive(true);
        interactionContainer.SetActive(true);

        pauseMenuContainer.SetActive(false);

        opened = !opened;
        playerController.paused = opened;
        Cursor.lockState = CursorLockMode.Locked;

    }

}
