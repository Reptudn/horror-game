using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InventoryManager : NetworkBehaviour
{

    public List<GameObject> items = new List<GameObject>();

    public GameObject hand;

    public string collectableItems = "Pickup";
    public int allowedPickupDistance = 6;

    public Animator animator;

    int index = 0;
    int itemsInInventory = 0;

    public int maxInventorySize = 1;

    public KeyCode pickupKey = KeyCode.E;

    private PlayerInventoryController InventoryController;

    void Start()
    {

        InventoryController = GetComponent<PlayerInventoryController>();
        itemsInInventory = items.Count;
        if (items.Count > 0){ Show(items[index]); }

    }

    public List<GameObject> GetInventoryItems(){
        return items;
    }

    public void AddItemToInventory(GameObject item){

        if(maxInventorySize <= itemsInInventory) {
            Debug.Log("Inventory full");
            return;
        }

        if (item.GetComponent<InventoryItemRefrence>() != null){

            List<InventoryItemInstance> _Items = new List<InventoryItemInstance>(InventoryController.Items);
            _Items.Add(item.GetComponent<InventoryItemRefrence>().Item);
            InventoryController.ChangeInventoryContents(_Items);

        } else {

            Debug.LogWarning("Item is missing an Item Refrence");

        }

        item.transform.position = hand.transform.position;
        item.transform.SetParent(hand.transform);
        item.transform.rotation = hand.transform.rotation;
        item.transform.localRotation = Quaternion.Euler(0f,0f,0f);

        items.Add(item);
        index = itemsInInventory;
        itemsInInventory++;
        Show(item);
    }

    public void RemoveItemFromInventory(GameObject item){
        
    }

    void Update()
    {

        if (hasAuthority)
        {

            if(items.Count > 0)
            {

                if(Input.GetAxis("Mouse ScrollWheel") > 0){ //scroll up
                    if(index < items.Count - 1) index++;
                    Show(items[index]);
                }

                if(Input.GetAxis("Mouse ScrollWheel") < 0){ //scroll down
                    if(index < items.Count && index > 0) index--;
                    Show(items[index]);
                }
            }

            if(Input.GetKeyDown(pickupKey)){

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit)){

                    if(hit.transform.gameObject.tag == collectableItems && hit.distance <= allowedPickupDistance) {

                        AddItemToInventory(hit.transform.gameObject);
                        Debug.DrawLine(Camera.main.transform.position, hit.point, Color.red, 5);

                    }
                }     

            }

        }
        
    }

    void Show(GameObject toShow){
        
        foreach(GameObject o in items){
            if(o == toShow) { o.SetActive(true); SetHoldingAnimationToggle(); }
            else o.SetActive(false);
        }
        
    }

    void SetHoldingAnimationToggle(){
        animator.SetBool("holdingItem", true);
        animator.SetLayerWeight(4, 1f);
    }

}