using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    [Header("Menu Base Containers")]
    public GameObject home;
    public GameObject host;
    public GameObject join;
    public GameObject options;
    public GameObject credits;

    private GameObject[] menuItems;

    // Start is called before the first frame update
    void Start()
    {
        menuItems = new GameObject[5];
        menuItems[0] = home;
        menuItems[1] = host;
        menuItems[2] = join;
        menuItems[3] = options;
        menuItems[4] = credits;

        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true; 

        Show(home);
    }

    public void Home(){
        Show(home);
    }

    public void HostGame(){
        Show(host);
    }

    public void JoinGame(){
        Show(join);
    }

    public void QuitGame(){

        //other quit game logic

        Application.Quit();
    }

    public void Options(){
        Show(options);
    }

    public void Credits(){
        Show(credits);
    }

    private void Show(GameObject toShow){

        foreach(GameObject o in menuItems){
            if(o == toShow) o.SetActive(true);
            else o.SetActive(false);
        }

    }
}
