using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [Header("Container")]
    public GameObject menuContainer;

    // Start is called before the first frame update
    void Start()
    {
        menuContainer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Escape)){

            ShowHideMenu();

        }

    }

    public void ResumeGame(){
        ShowHideMenu();
    }

    public void LeaveGame(){

        //networking leave game logic

    }

    public void Options(){

        //options leave game logic

    }

    private void ShowHideMenu(){

        if(SceneManager.GetActiveScene().name == "MainMenu") return;

        menuContainer.SetActive(!menuContainer.activeSelf);

        if(menuContainer.activeSelf){

            Cursor.lockState = CursorLockMode.None;

        } else {
            
            Cursor.lockState = CursorLockMode.Locked;

        }

    }
}
