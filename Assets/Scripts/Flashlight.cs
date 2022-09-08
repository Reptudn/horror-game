using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Flashlight : NetworkBehaviour
{

    public GameObject lightSource;
    public Animator animator;
    public AudioSource onSound;
    public AudioSource offSound;
    public PlayerObjectController LocalPlayerObject;
    public PlayerInventoryController InventoryController;
    public GameObject PlayerObject = null;
    
    
    void Start()
    {
        LocalPlayerObject = GameObject.Find("LocalGamePlayer").GetComponent<PlayerObjectController>();
        InventoryController =  LocalPlayerObject.GetComponent<PlayerInventoryController>();
        lightSource.SetActive(false);
        animator.SetBool("Light On", false);

        PlayerObject = FindOwner();

    }

    void Toggle()
    {

    }

    void VisualUpdate(bool State)
    {

        Debug.Log("Visual Update");

        lightSource.SetActive(State);
        animator.SetBool("Light On", State);
        if (State) onSound.Play();
        else offSound.Play();
    }

    GameObject FindOwner()
    {
        GameObject Player = null;
        GameObject Parent = transform.gameObject;

        while (Player == null && Parent != null)
        {
            if (Parent.GetComponent<PlayerObjectController>() != null)
            {
                Player = Parent;
            }
            else
            {
                Parent = transform.parent != null ? Parent.transform.parent.gameObject : null;
            }
        }

        return Player;

    }

    bool IsLocalPlayer() 
    { 
        if (PlayerObject != null) { return LocalPlayerObject == PlayerObject.GetComponent<PlayerObjectController>(); }
        return false;
    }

    void Update()
    {

        //Debug.Log("Authority: " + IsLocalPlayer());
        if (IsLocalPlayer())
        {

            if (this.gameObject.activeSelf && transform.parent != null && transform.parent.name == "ItemAnchor"){

                if (Input.GetMouseButtonDown(0))
                { 

                    VisualUpdate(!lightSource.activeSelf);
                
                    InventoryItemInstance Item = InventoryController.Items[InventoryController.EquippedIndex];
                    Item.SetAttribute("Enabled", lightSource.activeSelf);
                    InventoryController.TriggerNetworkSync();

                }

            }

        }

    }

}
