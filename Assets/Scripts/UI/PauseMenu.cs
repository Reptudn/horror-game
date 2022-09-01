using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [Header("Main")]
    public GameObject pauseMenuContainer;

    public GameObject[] nonOptionsMenu;
    public GameObject optionsMenu;

    [Header("Other Components")]
    public GameObject crosshairContainer;
    public GameObject interactionContainer;

    public PlayerController playerController;
    public KeyCode pauseMenuKey = KeyCode.Escape;

    bool opened = false;

    // Start is called before the first frame update
    void Start()
    {

        if(SceneManager.GetActiveScene().name == "MainMenu") { pauseMenuContainer.SetActive(false); return; } else {

            pauseMenuContainer.SetActive(false);
            crosshairContainer.SetActive(true);
            interactionContainer.SetActive(true);
            optionsMenu.SetActive(false);
        }
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

            if(opened) {
                Cursor.lockState = CursorLockMode.None;
                foreach(var o in nonOptionsMenu) o.SetActive(true);
            }
            else { Cursor.lockState = CursorLockMode.Locked; 
                pauseMenuContainer.SetActive(false);
                optionsMenu.SetActive(false); 
                crosshairContainer.SetActive(true);
                interactionContainer.SetActive(true);
            }

        }

    }

    
    public void Options(){

        optionsMenu.SetActive(true);

        foreach(var o in nonOptionsMenu) o.SetActive(false);

    }

    public void Back(){
        transform.gameObject.SetActive(false);
        pauseMenuContainer.SetActive(true);
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
