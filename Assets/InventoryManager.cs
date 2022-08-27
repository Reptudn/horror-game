using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public List<GameObject> items;

    public GameObject hand;

    public Camera camera;

    public string collectableItems = "Pickup";
    public int allowedPickupDistance = 6;

    int index = 0;
    int itemsInInventory = 0;


    void Start()
    {

        Show(items[index]);
     
        for(int i = 0; i < items.Count; i++){
            if(items[i] == null) {
                GameObject placeholder = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                placeholder.name = "placeholder";
                placeholder.SetActive(false);
                items[i] = new GameObject();

            }

            AddItemToInventory(items[i]);
            
        }

        itemsInInventory = items.Count;

    }

    public List<GameObject> GetInventoryItems(){
        return items;
    }

    public void AddItemToInventory(GameObject item){

        if(items.Count <= itemsInInventory) return;

        item.transform.position = hand.transform.position;
        item.transform.SetParent(hand.transform);

        items[itemsInInventory] = item;
        index = itemsInInventory;
        itemsInInventory++;
        Show(item);

        Debug.Log("Item added to inventory: " + item.name + ", " + itemsInInventory);
    }

    public void RemoveItemFromInventory(GameObject item){
        
    }

    void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0){ //scroll up
            if(index < items.Count - 1) index++;
            Debug.Log(index);
            Show(items[index]);
        }

        if(Input.GetAxis("Mouse ScrollWheel") < 0){ //scroll down
            if(index < items.Count && index > 0) index--;
            Debug.Log(index);
            Show(items[index]);
        }

        if(Input.GetKeyDown(KeyCode.E)){

            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit)){

                if(hit.transform.gameObject.tag == collectableItems && hit.distance <= allowedPickupDistance) {

                    AddItemToInventory(hit.transform.gameObject);
                    Debug.DrawLine(camera.transform.position, hit.point, Color.red, 5);

                }
            }     

        }
        
    }

    void LogItemsInventory(){
        Debug.Log("--------------------");
        foreach(GameObject i in items){
            Debug.Log(i.name);
        }
        Debug.Log(itemsInInventory);
        Debug.Log("--------------------");
    }

    void Show(GameObject toShow){
        
        foreach(GameObject o in items){
            if(o == toShow) o.SetActive(true);
            else o.SetActive(false);
        }
        
    }

}
