using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Torch : MonoBehaviour
{

    public PlayerObjectController LocalPlayerObject;
    public PlayerInventoryController InventoryController;
    public GameObject torchLight;
    public GameObject PlayerObject = null;

    public bool State = false;

    void Start(){
        torchLight.SetActive(false);

        LocalPlayerObject = GameObject.Find("LocalGamePlayer").GetComponent<PlayerObjectController>();
        PlayerObject = FindOwner();
        if (PlayerObject != null)
        {
            InventoryController =  PlayerObject.GetComponent<PlayerInventoryController>();
        }
    }

    // Update is called once per frame
    void Update()
    {   

        Debug.Log("local: " + IsLocalPlayer());

        if (!IsLocalPlayer())
        {

            Debug.Log("local player");

            if (this.gameObject.activeSelf && transform.parent != null && transform.parent.name == "ItemAnchor"){

                if (Input.GetMouseButtonDown(1))
                { 
                    Debug.Log("Toggle torch");
                    torchLight.SetActive(!torchLight.activeSelf);
                }

            }

        } 

        else if (InventoryController != null)
        {
            InventoryItemInstance Item = InventoryController.Items[InventoryController.EquippedIndex];
            bool NewState = Item.GetAttributeBool("EnabledTorch");

            
            if (NewState != State)
            {
                State = NewState;
            }
        }

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
}
