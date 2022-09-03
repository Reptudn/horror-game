using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Flashlight : MonoBehaviour
{

    public GameObject lightSource;
    public Animator animator;
    public AudioSource onSound;
    public AudioSource offSound;
    public PlayerObjectController LocalPlayerObject;
    public PlayerInventoryController InventoryController;
    
    void Start()
    {
        LocalPlayerObject = GameObject.Find("LocalGamePlayer").GetComponent<PlayerObjectController>();
        InventoryController =  LocalPlayerObject.GetComponent<PlayerInventoryController>();
        lightSource.SetActive(false);
        animator.SetBool("Light On", false);
    }

    // Update is called once per frame
    void Update()
    {

        if(this.gameObject.activeSelf && transform.parent != null && transform.parent.name == "ItemAnchor"){

            if (Input.GetMouseButtonDown(0))
            { 

                lightSource.SetActive(!lightSource.activeSelf);
                animator.SetBool("Light On", !animator.GetBool("Light On"));
            
                InventoryItemInstance Item = InventoryController.Items[InventoryController.EquippedIndex];
                Item.SetAttribute("Enabled", lightSource.activeSelf);
                InventoryController.TriggerNetworkSync();

                if(lightSource.activeSelf) onSound.Play();
                else offSound.Play();

            }

        }

    }

}
